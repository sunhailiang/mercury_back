using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MercurySurvey.Models;
using JupiterClient;

namespace MercurySurvey.Controllers
{
    /// <summary>
    /// 该控制器服务于管理人员
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class EditController : ControllerBase
    {
        private readonly NoteContext noteContext;
        //private readonly IConfiguration configuration;
        //private readonly string sqlstr;

        public EditController(NoteContext noteContext/*, IConfiguration config*/)
        {
            this.noteContext = noteContext;
            // configuration = config;
            //sqlstr = ConfigurationExtensions.GetConnectionString(configuration, "MercurySqlServer");
        }

        /// <summary>
        /// Question 添加
        /// </summary>
        /// <param name="question">传递的question表的实体模型</param>
        /// <returns></returns>
        private IActionResult Create(Question question)
        {
            question.QuestionGuid = Guid.NewGuid();
            question.Ctime = DateTime.Now;
            question.Version = Guid.NewGuid();
            noteContext.Question.Add(question);
            int result = noteContext.SaveChanges();
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound("添加失败");
            }
        }
        /// <summary>
        /// question的查询
        /// </summary>
        /// <returns></returns>
        private IActionResult Look()
        {
            var result = (from s in noteContext.Question
                          orderby s.Ctime descending
                          where s.IsEnable == true
                          select s).FirstOrDefault();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("操作失败");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="question">question的实体模型类</param>
        /// <returns></returns>
        private IActionResult Update(Question question)
        {
            var result = (from s in noteContext.Question
                          where s.QuestionGuid == question.QuestionGuid
                          select s).FirstOrDefault();
            if (result.Version == null)
            {
                noteContext.Question.Add(question);
                noteContext.SaveChanges();
                return NotFound("添加成功");
            }
            else
            {
                question.QuestionGuid = Guid.NewGuid();
                question.Ctime = DateTime.Now;
                question.Version = Guid.NewGuid();
                noteContext.Question.Update(question);
                noteContext.SaveChanges();
                return NotFound("修改成功");
            }

        }
        /// <summary>
        /// question的删除
        /// </summary>
        /// <param name="guid">question的questionguid字段</param>
        /// <returns></returns>
        private IActionResult Delete(Guid guid)
        {
            var result = (from s in noteContext.Question
                          where s.QuestionGuid == guid
                          select s).FirstOrDefault();
            result.IsEnable = false;
            noteContext.Question.Update(result);
            int result1 = noteContext.SaveChanges();
            if (result1 == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound("修改失败");
            }
        }
        /// <summary>
        /// Questionnaire的添加
        /// </summary>
        /// <param name="questionnaire">questionnaire的实体数据模型类</param>
        /// <returns></returns>
        private IActionResult CreateQuestionnaire(Questionnaire questionnaire)
        {
            questionnaire.QuestionnaireGuid = Guid.NewGuid();
            questionnaire.Ctime = DateTime.Now;
            questionnaire.Version = Guid.NewGuid();
            noteContext.Questionnaire.Add(questionnaire);
            int result = noteContext.SaveChanges();
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound("添加失败");
            }
        }
        /// <summary>
        /// Questionnaire表的查询
        /// </summary>
        /// <returns></returns>
        private IActionResult LookQuestionnaire()
        {
            var result = (from s in noteContext.Questionnaire
                          orderby s.Ctime descending
                          where s.IsDeleted == false
                          select s).FirstOrDefault();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("操作失败");
            }
        }
        /// <summary>
        /// Questionnaire的修改
        /// </summary>
        /// <param name="questionnaire">questionnair 的实体数据模型类</param>
        /// <returns></returns>
        private IActionResult UpdateQuestionnaire(Questionnaire questionnaire)
        {
            var result = (from s in noteContext.Questionnaire
                          where s.QuestionnaireGuid == questionnaire.QuestionnaireGuid
                          select s).FirstOrDefault();
            if (result.Version == null)
            {
                noteContext.Questionnaire.Add(questionnaire);
                noteContext.SaveChanges();
                return NotFound("添加成功");
            }
            else
            {
                questionnaire.QuestionnaireGuid = Guid.NewGuid();
                questionnaire.Ctime = DateTime.Now;
                questionnaire.Version = Guid.NewGuid();
                noteContext.Questionnaire.Update(questionnaire);
                noteContext.SaveChanges();
                return NotFound("修改成功");
            }
        }
        /// <summary>
        /// Questionnaire的删除
        /// </summary>
        /// <param name="guid">Questionnaire中的QuestionnaireGuid</param>
        /// <returns></returns>
        private IActionResult DeleteQuestionnaire(Guid guid)
        {
            var result = (from s in noteContext.Questionnaire
                          where s.QuestionnaireGuid == guid
                          select s).FirstOrDefault();
            result.IsDeleted = true;
            noteContext.Questionnaire.Update(result);
            int result1 = noteContext.SaveChanges();
            if (result1 == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound("修改失败");
            }

        }

        //private readonly NoteContext noteContext;

        //public EditController(NoteContext noteContext)
        //{
        //    this.noteContext = noteContext;
        //}


        ///// <summary>
        ///// 新建问题
        ///// </summary>
        ///// <param name="question"></param>
        ///// <returns>问题实体数据模型</returns>
        //[HttpPost]
        //public IActionResult Question([FromBody]Question question)
        //{
        //    if (question.QuestionGuid == Guid.Empty)
        //    {
        //        question.QuestionGuid = Guid.NewGuid();
        //        question.Ctime = DateTime.Now;
        //        question.Version = Guid.NewGuid();
        //        noteContext.Question.Add(question);
        //        noteContext.SaveChanges();
        //        return Ok(new { Success = true, Data = question });
        //    }
        //    else
        //    {
        //        var result = (from s in noteContext.Question
        //                      where s.QuestionGuid == question.QuestionGuid
        //                      where s.Version == Guid.Empty
        //                      select s).FirstOrDefault();
        //        if (result != null)
        //        {
        //            result.QuestionGuid = Guid.NewGuid();
        //            result.Content = question.Content;
        //            result.Calculator = question.Calculator;
        //            result.Ctime = DateTime.Now;
        //            result.Version = Guid.NewGuid();

        //            result.Type = question.Type;
        //            noteContext.Question.Add(result);
        //            noteContext.SaveChanges();
        //            return Ok(new { Success = true, Data = result });
        //        }
        //        else
        //        {
        //            return NotFound(new { Success = false, Message = "未能找到该问题" });
        //        }

        //    }
        //}

        ///// <summary>
        ///// 删除问卷
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult DelQuestionnaire(Guid guid)
        //{
        //    var result = (from s in noteContext.Questionnaire
        //                  where s.QuestionnaireGuid == guid
        //                  select s).FirstOrDefault();
        //    if (result.IsDeleted == false)
        //    {
        //        return NotFound("已删除");
        //    }
        //    else
        //    {
        //        result.IsDeleted = false;
        //        noteContext.Questionnaire.Update(result);
        //        noteContext.SaveChanges();
        //        return Ok();
        //    }

        //}

        ///// <summary>
        ///// 分页显示已删除的内容
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult LookDel(int pageIndex, int pageCount)
        //{
        //    var result = (from s in noteContext.Questionnaire
        //                  orderby s.Ctime
        //                  select s).Skip((pageIndex * pageCount)).Take(pageCount);
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound("没有查到需要的数据");
        //    }
        //}

        ///// <summary>
        ///// 修改Question1拉取数据表中的数据
        ///// </summary>
        ///// <param name="questionguid">问题的guid</param>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult EditIssue(Guid questionguid)
        //{
        //    var result = (from s in noteContext.Question
        //                  where s.QuestionGuid == questionguid
        //                  select s).FirstOrDefault();
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound("查找失败");
        //    }

        //}

        ///// <summary>
        ///// 修改Question2修改数据表
        ///// </summary>
        ///// <param name="question">问题的实体数据模型</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult EditIssue([FromBody]Question question)
        //{
        //    noteContext.Question.Update(question);
        //    noteContext.SaveChanges();
        //    return Ok();
        //}

        ///// <summary>
        ///// 查找Question
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult SeleteQuestion(Question question)
        //{
        //    var result = (from s in noteContext.Question
        //                  where s.QuestionGuid == question.QuestionGuid
        //                  select s).FirstOrDefault();
        //    if (result != null)
        //    {
        //        return Ok(new { Success = true, Data = result });
        //    }
        //    else
        //    {
        //        return NotFound("查找失败");
        //    }
        //}
    }
}