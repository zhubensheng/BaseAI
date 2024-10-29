using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model.System
{
    [SugarTable("RolesTable")]
    public class  RolesTable
    {
        [Required]
        public int Id { get; set; }
    }
}
