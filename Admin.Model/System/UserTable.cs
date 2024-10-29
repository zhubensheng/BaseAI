using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model.System
{
    [SugarTable("UserTable")]
    public partial class UserTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int UserId { get; set; }


    }
}
