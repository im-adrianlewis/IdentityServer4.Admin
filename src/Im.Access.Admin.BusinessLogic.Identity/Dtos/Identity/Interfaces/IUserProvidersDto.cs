using System.Collections.Generic;

namespace Im.Access.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IUserProvidersDto : IUserProviderDto
    {
        List<IUserProviderDto> Providers { get; }
    }
}
