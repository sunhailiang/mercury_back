using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JupiterClient;
using MercurySurvey.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MercurySurvey.Controllers
{
    /// <summary>
    /// 文件上传控制器，建议未来拆分后在一个独立的服务器上运行
    /// </summary>
    [Route("[controller]/[Action]")]
    [CustomAuthorize]
    public class UploadController : Controller
    {
        private readonly NoteContext noteContext;
        public UploadController(NoteContext db)
        {
            noteContext = db;
        }

        /// <summary>
        /// 文件上传接口，文件保存在运行时用户的家目录/userUploads文件夹中
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> BeginUpload()
        {
            //当前方法，单个上传文件限制为128m

            //获取当前用户
            Guid currentUserGuid = Guid.Parse((from claim in HttpContext.User.Claims
                                               where claim.Type == "guid"
                                               select claim.Value).FirstOrDefault());
            if (currentUserGuid == default)
            {
                return NotFound("找不到用户关联信息");
            }

            //获取当前用户家目录，在win为“我的文档”，linux默认为/home/{username}
            var homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var files = Request.Form.Files;
            var guidFileName = Guid.NewGuid().ToString();
            var storagePath = Path.Combine(Directory.CreateDirectory(Path.Combine(homePath, "userUploads")).FullName, guidFileName + Path.GetExtension(files[0].FileName));
            try
            {
                using (var stream = new FileStream(storagePath, FileMode.CreateNew))
                {
                    await files[0].CopyToAsync(stream);
                }

            }
            catch (IOException)
            {
                return Ok(new { Success = false, Message = "文件IO错误" });
            }


            UploadFile upLoadFile = new UploadFile()
            {
                FileGuid = Guid.NewGuid(),
                UserGuid = currentUserGuid,
                LocalFilePath = storagePath,
                Ctime = DateTime.Now,
            };

            noteContext.UploadFile.Add(upLoadFile);
            noteContext.SaveChanges();

            return Ok(new { Success = true, UploadGuid = guidFileName });
        }
    }
}