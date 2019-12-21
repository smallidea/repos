using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Repos.Dto;
using Repos.Lib;
using Repos.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Repos.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        #region 私有成员
        private const string _api = "https://api.github.com/users/leansoftx/repos";
        private readonly static SqlHelper _sqlHelper = new SqlHelper(AppConfigurtaionServices.Configuration.GetConnectionString("ReposConnectionStr"));
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger) : base(logger)
        {
            
        }
        #endregion

        #region view
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region API
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public IActionResult GetData()
        {
            // 从数据库中查找
            var list = getReposWithDb();

            // 如果没有就去解析API
            if (list == null || list.Count <= 0)
            {
                list = getReposWithApi();
                insertRpos4Db(list);
            }

            return Ok(list);
        }

        /// <summary>
        /// 提交选中的
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SendMyRepos([FromBody]SendParam param)
        {
            var result = send(param.Ids);
            return Ok(result);
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private bool send(int[] ids)
        {
            StringBuilder sb = new StringBuilder("delete from [MyRepos];");
            foreach (var id in ids)
            {
                sb.Append($"insert into MyRepos([ID], [CreateTime]) values({id}, getdate());");
            }
            var result = _sqlHelper.ExecuteNonQuery(sb.ToString());
            return result > 0;
        }

        /// <summary>
        /// 从数据库中获取信息
        /// </summary>
        /// <returns></returns>
        private List<RepoDto> getReposWithDb()
        {
            var sql = @"select a.[ID], [CodeNo], [Name], [FullName], [GitUrl], [Content], (case when my.ID is null then 0 else 1 end) IsCheck 
                        from[AllRepos] a
                        left join[MyRepos] my on my.ID = a.ID
                        order by [Name]";
            var dt = _sqlHelper.ExecuteDataTable(sql);
            var tmpList = from a in dt.AsEnumerable()
                          select new RepoDto()
                          {
                              Id = a.Field<int>("ID"),
                              CodeNo = a.Field<string>("CodeNo"),
                              Name = a.Field<string>("Name"),
                              FullName = a.Field<string>("FullName"),
                              GitUrl = a.Field<string>("GitUrl"),
                              Content = a.Field<string>("Content"),
                              IsCheck = a.Field<int>("IsCheck") == 1
                          };

            return tmpList.ToList();
        }

        /// <summary>
        /// 从API中获取信息
        /// </summary>
        /// <returns></returns>
        private List<RepoDto> getReposWithApi()
        {
            List<RepoDto> list = new List<RepoDto>();

            HttpClient myHttpClient = new HttpClient();
            myHttpClient.DefaultRequestHeaders.Add("User-Agent", "leansoftX.Repos"); // 请求git的api接口需要带上应用信息，否则会报错
            var response = myHttpClient.GetAsync(_api).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var array = JasonHelper.ConvertToObj<JArray>(json);

            foreach (var item in array)
            {
                dynamic repo = item;
                RepoDto reposDto = new RepoDto
                {
                    Name = repo.name,
                    CodeNo = repo.node_id,
                    FullName = repo.full_name,
                    Id = repo.id,
                    GitUrl = repo.git_url,
                    Content = JasonHelper.ConvertToStr(repo)
                };
                list.Add(reposDto);
            }

            return list;
        }

        /// <summary>
        /// 提交数据到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool insertRpos4Db(List<RepoDto> list)
        {
            if (list == null || list.Count <= 0) return false;

            int rows = 0;
            foreach (var item in list)
            {
                string sql2 = $@"insert into [dbo].[AllRepos]([ID], [CodeNo], [Name], [FullName], [Content], [GitUrl], [CreateTime])
values(@ID,@CodeNo,@Name,@FullName,@Content,@GitUrl,getdate());";
                rows += _sqlHelper.ExecuteNonQuery(sql2, new SqlParameter[]
                {
                    new SqlParameter("@ID", item.Id),
                    new SqlParameter("@CodeNo", item.CodeNo),
                    new SqlParameter("@Name", item.Name),
                    new SqlParameter("@FullName",item.FullName),
                    new SqlParameter("@Content", item.Content),
                    new SqlParameter("@GitUrl", item.GitUrl),
                });
            }
            return rows > 0;
        }
        #endregion
    }

    /// <summary>
    /// 提交参数
    /// </summary>
    public class SendParam
    {
        public int[] Ids { get; set; }
    }

}
