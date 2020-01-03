using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Base
{
    public class BaseUserRolesDto<TUserDtoKey, TRoleDtoKey> : IBaseUserRolesDto
    {
        public TUserDtoKey UserId { get; set; }

        public TRoleDtoKey RoleId { get; set; }

        object IBaseUserRolesDto.UserId => UserId;

        object IBaseUserRolesDto.RoleId => RoleId;
    }
}