using System.ComponentModel.DataAnnotations;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserChangePasswordDto<TUserDtoKey> : BaseUserChangePasswordDto<TUserDtoKey>, IUserChangePasswordDto
    {
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
