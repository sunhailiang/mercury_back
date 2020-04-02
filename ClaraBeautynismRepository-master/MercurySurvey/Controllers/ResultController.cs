using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MercurySurvey.Models;
using Microsoft.Extensions.Configuration;
using JupiterClient;
using System.Collections.Generic;

namespace MercurySurvey.Controllers
{
    /// <summary>
    /// 该控制器服务于查询答案
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[CustomAuthorize]
    public class ResultController : Controller
    {
        private readonly NoteContext noteContext;
        //private readonly IConfiguration configuration;
        //private readonly string sqlstr;

        public ResultController(NoteContext noteContext)
        {
            this.noteContext = noteContext;
            //configuration = config;
            //sqlstr = ConfigurationExtensions.GetConnectionString(configuration, "MercurySqlServer");
        }

        /// <summary>
        /// 返回所有的Result的值分页显示
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageCount">一页总条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult HistoricalAnswer(int pageIndex, int pageCount)
        {
            var result = (from s in noteContext.Answer
                          orderby s.Ctime
                          select s).Skip(pageIndex * pageCount).Take(pageCount);
            if (result.Count() > 0)
            {
                return Json(result);
            }
            else
            {
                return NotFound("返回信息失败");
            }
        }
        /// <summary>
        /// 实现输入姓名和手机号查询出符合要求的用户信息
        /// </summary>
        /// <param name="username">用户的姓名</param>
        /// <param name="phonenumber">用户的电话</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FindMercuryUserinfomation(string username, string phonenumber)
        {
            var result = (from s in noteContext.UserInformation
                          where s.UserName == username || s.PhoneNumber == phonenumber
                          select s).ToList();
            var result1 = result.GroupBy(p => p.PhoneNumber).Select(g => g.First()).ToList();
            if (result1.Any())
            {
                return Json(result1);
            }
            else
            {
                return NotFound("查无此人");
            }

        }

    }




}