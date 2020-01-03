﻿using System.Collections.Generic;
using System.Linq;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using Im.Access.Admin.BusinessLogic.Shared.Dtos.Common;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey> : BaseUserRolesDto<TUserDtoKey, TRoleDtoKey>, IUserRolesDto
        where TRoleDto : RoleDto<TRoleDtoKey>
    {
        public UserRolesDto()
        {
           Roles = new List<TRoleDto>(); 
        }

        public string UserName { get; set; }

        public List<SelectItemDto> RolesList { get; set; }

        public List<TRoleDto> Roles { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        List<IRoleDto> IUserRolesDto.Roles => Roles.Cast<IRoleDto>().ToList();
    }
}
