using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MercurySurvey.Models.TraditionalMedicalConstitution;
using Microsoft.EntityFrameworkCore;


namespace MercurySurvey.Models
{
    /// <summary>
    /// MercurySurvey核心数据库上下文
    /// </summary>
    public class NoteContext:DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {

        }

        /// <summary>
        /// 问题表
        /// </summary>
        public DbSet<Question> Question { get; set; }

        /// <summary>
        /// 问卷表
        /// </summary>
        public DbSet<Questionnaire> Questionnaire { get; set; }

        /// <summary>
        /// 结果表（弃用）
        /// </summary>
        public DbSet<Result> Result { get; set; }

        /// <summary>
        /// 答案表
        /// </summary>
        public DbSet<Answer> Answer { get; set; }

        /// <summary>
        /// 队列表
        /// </summary>
        public DbSet<QuestionQueue> QuestionQueue { get; set; }

        /// <summary>
        /// 上传记录表
        /// </summary>
        public DbSet<UploadFile> UploadFile { get; set; }

        /// <summary>
        /// 用户信息表，未来应拆分至其他DLL
        /// </summary>
        public DbSet<UserInformationRecord> UserInformation { get; set; }

    }
}
