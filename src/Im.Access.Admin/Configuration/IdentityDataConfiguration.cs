using Im.Access.Admin.Configuration.Identity;
using System.Collections.Generic;

namespace Im.Access.Admin.Configuration
{
    public class IdentityDataConfiguration
    {
       public List<Role> Roles { get; set; }
       public List<User> Users { get; set; }
    }
}
