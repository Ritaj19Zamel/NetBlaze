using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Responses
{
    public sealed class OtpTargetUserDto
    {
        public long Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
