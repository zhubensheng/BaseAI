using Admin.Model.SystemModel;
using Admin.Repositorie.ISystem;
using AntDesign.Charts;
using Microsoft.AspNetCore.Mvc;

namespace Ant_ERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IUserTable_Repositories _repository;

        public InitController(IUserTable_Repositories repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 初始化DB 和表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitTable()
        {
            _repository.GetDB().DbMaintenance.CreateDatabase();
            _repository.GetDB().CodeFirst.InitTables(typeof(UserTable));
            //创建vector插件如果数据库没有则需要提供支持向量的数据库
            _repository.GetDB().Ado.ExecuteCommandAsync($"CREATE EXTENSION IF NOT EXISTS vector;");
            return Ok();
        }
    }
}
