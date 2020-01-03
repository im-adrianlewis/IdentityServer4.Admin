using Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserProvidersDto<TUserDtoKey> : UserProviderDto<TUserDtoKey>, IUserProvidersDto
    {
        public UserProvidersDto()
        {
            Providers = new List<UserProviderDto<TUserDtoKey>>();
        }

        public List<UserProviderDto<TUserDtoKey>> Providers { get; set; }

        List<IUserProviderDto> IUserProvidersDto.Providers => Providers.Cast<IUserProviderDto>().ToList();
    }
}
