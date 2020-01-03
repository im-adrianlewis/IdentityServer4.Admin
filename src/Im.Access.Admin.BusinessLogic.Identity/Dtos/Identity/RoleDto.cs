using System.ComponentModel.DataAnnotations;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}