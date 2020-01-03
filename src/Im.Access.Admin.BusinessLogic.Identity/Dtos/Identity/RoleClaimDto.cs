using System.ComponentModel.DataAnnotations;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleClaimDto<TRoleDtoKey> : BaseRoleClaimDto<TRoleDtoKey>, IRoleClaimDto
    {
        [Required]
        public string ClaimType { get; set; }


        [Required]
        public string ClaimValue { get; set; }
    }
}
