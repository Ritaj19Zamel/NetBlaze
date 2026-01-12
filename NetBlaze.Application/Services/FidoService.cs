using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.Fido.Requests;
using NetBlaze.SharedKernel.Dtos.Fido.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace NetBlaze.Application.Services
{
    public class FidoService : IFidoService
    {
        private readonly UserManager<User> _userManager;
        private readonly Fido2 _fido;
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private const string FidoRegisterCacheKeyPrefix = "fido_register_";
        private const string FidoLoginCacheKeyPrefix = "fido_login_";

        public FidoService(
                IUnitOfWork unitOfWork,
                UserManager<User> userManager,
                Fido2 fido,
                IMemoryCache cache,
                IUserContext userContext)
        {
            _userManager = userManager;
            _fido = fido;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }
        public async Task<ApiResponse<FidoRegisterOptionsResponseDto>> StartFidoRegisterAsync(FidoLoginOptionsRequestDto request)
        {
    

            var user = await _userManager.Users
                .Include(u => u.UserDevices)
                .FirstAsync(u => u.Id == request.UserId && u.IsActive);


            var excludedCredentials = user.UserDevices
                .Where(d => d.IsActive)
                .Select(d => new PublicKeyCredentialDescriptor(d.CredentialId))
                .ToList();

            var options = _fido.RequestNewCredential(new RequestNewCredentialParams
            {
                User = new Fido2User
                {
                    Id = Encoding.UTF8.GetBytes(user.Id.ToString()),
                    Name = user.Email!,
                    DisplayName = user.DisplayName
                },
                ExcludeCredentials = excludedCredentials,
                AuthenticatorSelection = new AuthenticatorSelection
                {
                    AuthenticatorAttachment = null,
                    UserVerification = UserVerificationRequirement.Required,
                    RequireResidentKey = true
                },
                AttestationPreference = AttestationConveyancePreference.Direct 
            });

            _cache.Set($"{FidoRegisterCacheKeyPrefix}{user.Id}", options, TimeSpan.FromMinutes(5));

            return ApiResponse<FidoRegisterOptionsResponseDto>
                .ReturnSuccessResponse(new() { Options = options });
        }


        public async Task<ApiResponse<object>> CompleteFidoRegisterAsync(FidoRegisterCompleteRequestDto dto)
        {


            var user = await _userManager.Users
                .Include(u => u.UserDevices)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }

            if (!_cache.TryGetValue($"{FidoRegisterCacheKeyPrefix}{dto.UserId}", out CredentialCreateOptions options))
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.FidoSessionExpired, HttpStatusCode.BadRequest);
            }

            var attestationRaw = JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(
                    dto.AttestationJson)!;

            var makeParams = new MakeNewCredentialParams
            {
                AttestationResponse = attestationRaw,
                OriginalOptions = options,
                IsCredentialIdUniqueToUserCallback = async (args, ct) =>
                {
                    return !await _userManager.Users
                        .AnyAsync(u => u.UserDevices
                            .Any(d => d.CredentialId.SequenceEqual(args.CredentialId)), ct);
                }
            };

            var result = await _fido.MakeNewCredentialAsync(makeParams);

            var deviceFingerprint = result.AaGuid != Guid.Empty ? result.AaGuid.ToString() : string.Empty;

            if (!string.IsNullOrEmpty(deviceFingerprint))
            {
                var deviceUsedByAnotherUser = await _userManager.Users
                    .Where(u => u.Id != dto.UserId)
                    .SelectMany(u => u.UserDevices)
                    .AnyAsync(d => d.AaGuid == result.AaGuid && d.RevokedAt == null);

                if (deviceUsedByAnotherUser)
                {
                    return ApiResponse<object>.ReturnFailureResponse(
                        Messages.DeviceAlreadyRegistered,
                        HttpStatusCode.Conflict);
                }
            }

            user.UserDevices.Add(new UserDevice
            {
                UserId = user.Id,
                DeviceName = "Primary Device",
                CredentialId = result.Id,
                SignatureCounter = 0,
                CredentialIdBase64 = ToBase64Url(result.Id),
                PublicKey = result.PublicKey,
                CredType = PublicKeyCredentialType.PublicKey.ToString(),
                AaGuid = result.AaGuid
            });

            await _userManager.UpdateAsync(user);

            _cache.Remove($"{FidoRegisterCacheKeyPrefix}{dto.UserId}");

            return ApiResponse<object>.ReturnSuccessResponse(Messages.LoginSuccess);
        }

        public async Task<ApiResponse<FidoLoginOptionsResponseDto>> StartFidoLoginAsync(FidoLoginOptionsRequestDto request)
        {
            var user = await _userManager.Users.Include(u => u.UserDevices)
                .FirstOrDefaultAsync(u => u.Id == request.UserId); 
            
            if (user == null) { 
                return ApiResponse<FidoLoginOptionsResponseDto>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound); 
            }

            var devices = user.UserDevices.Where(d => d.RevokedAt == null).ToList(); 
            
            if (!devices.Any()) { 
                return ApiResponse<FidoLoginOptionsResponseDto>.ReturnFailureResponse(Messages.NoRegisteredDevice); 
            }

            var allowedCredentials = devices.Select(d => new PublicKeyCredentialDescriptor(d.CredentialId)).ToList(); 
            
            var options = _fido.GetAssertionOptions(allowedCredentials, UserVerificationRequirement.Preferred); 
            
            _cache.Set($"{FidoLoginCacheKeyPrefix}{request.UserId}", options, TimeSpan.FromMinutes(5)); 
            
            return ApiResponse<FidoLoginOptionsResponseDto>.ReturnSuccessResponse(new FidoLoginOptionsResponseDto(options));

        }


        public async Task<ApiResponse<object>> CompleteFidoLoginAsync(FidoLoginCompleteRequestDto dto) { 
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString()); 
            
            if (user == null) { 
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound); 
            } 

            if (!_cache.TryGetValue($"{FidoLoginCacheKeyPrefix}{dto.UserId}", out AssertionOptions options)) 
            { 
                return ApiResponse<object>.ReturnFailureResponse(Messages.FidoSessionExpired); 
            } 
            
            var assertionRaw = JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(dto.AttestationJson); 
            
            if (assertionRaw == null) { 
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidFidoAssertion); 
            } 
            
            var credentialIdBase64 = assertionRaw.Id; 
            
            var devices = await _unitOfWork.Repository.GetMultipleAsync<UserDevice>(true, d => d.UserId == dto.UserId && 
            d.RevokedAt == null); 
            
            var device = devices.FirstOrDefault(d => d.CredentialIdBase64 == credentialIdBase64); 
            
            if (device == null) { 
                return ApiResponse<object>.ReturnFailureResponse(Messages.DeviceNotRecognized); 
            } 
            
            var makeAssertionParams = new MakeAssertionParams { 
                AssertionResponse = assertionRaw, 
                OriginalOptions = options, 
                StoredPublicKey = device.PublicKey, 
                StoredSignatureCounter = device.SignatureCounter, 
                IsUserHandleOwnerOfCredentialIdCallback = 
                async (args, ct) => { 
                    var userIdBytes = Encoding.UTF8.GetBytes(user.Id.ToString()); 
                    return args.UserHandle.SequenceEqual(userIdBytes); } 
            }; 
            
            var res = await _fido.MakeAssertionAsync(makeAssertionParams); 
            
            device.SignatureCounter = res.SignCount; 
            
            await _unitOfWork.Repository.CompleteAsync(); 
            
            _cache.Remove($"{FidoLoginCacheKeyPrefix}{dto.UserId}"); 
            
            return ApiResponse<object>.ReturnSuccessResponse(Messages.LoginSuccess); 
        }

        #region HelperFunctions
        private static string ToBase64Url(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
        #endregion

    }
}