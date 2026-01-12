# ===============================
# Build stage (Windows)
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2019 AS build

WORKDIR C:/src

COPY NetBlaze.Api/NetBlaze.Api.csproj NetBlaze.Api/
COPY NetBlaze.Application/NetBlaze.Application.csproj NetBlaze.Application/
COPY NetBlaze.Domain/NetBlaze.Domain.csproj NetBlaze.Domain/
COPY NetBlaze.Infrastructure/NetBlaze.Infrastructure.csproj NetBlaze.Infrastructure/
COPY NetBlaze.SharedKernel/NetBlaze.SharedKernel.csproj NetBlaze.SharedKernel/

RUN dotnet restore NetBlaze.Api/NetBlaze.Api.csproj

COPY . .
WORKDIR C:/src/NetBlaze.Api
RUN dotnet publish -c Release -o C:/app/publish

# ===============================
# Runtime stage (Windows)
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2019

WORKDIR C:/app
COPY --from=build C:/app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "NetBlaze.Api.dll"]
