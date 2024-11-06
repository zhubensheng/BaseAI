using Admin.Domain.Common;
using Admin.Model.SystemModel;
using Admin.Repositorie.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Repositorie.ISystem
{
    [ServiceDescription(typeof(IUserTable_Repositories), ServiceLifetime.Scoped)]
    public class UserTable_Repositories : Repository<UserTable>, IUserTable_Repositories
    {
    }
}
