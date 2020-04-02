using JupiterClient;
using MercurySurvey.Models;
using MercurySurvey.Models.QuestionMetaData;
using MercurySurvey.Models.TraditionalMedicalConstitution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MercurySurvey.Controllers
{
    /// <summary>
    /// 答题控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class QuestionnaireController : ControllerBase
    {
        private readonly NoteContext noteContext;

        private static readonly List<Ticket> ticketsBook = new List<Ticket>();

        #region 私有方法
        /// <summary>
        /// 查找调查问卷
        /// </summary>
        /// <param name="guid">调查问卷的Guid</param>
        /// <param name="questionnaire">查找到的问卷</param>
        /// <returns></returns>
        private bool TryGetQuestionnaire(Guid guid, out Questionnaire questionnaire)
        {
            Questionnaire result = (from s in noteContext.Questionnaire
                                    where s.QuestionnaireGuid == guid
                                    orderby s.Ctime descending
                                    where s.Version != Guid.Empty
                                    select s).FirstOrDefault();
            questionnaire = result;
            return questionnaire != null;
        }

        /// <summary>
        ///查找问题队列
        /// </summary>
        /// <param name="question"></param>
        private IQueue<Question> SeekQuestionQueue(Guid guid)
        {
            string queuePointer = (from s in noteContext.Answer where s.AnswerGuid == guid select s.Content).FirstOrDefault();
            return new BasicQueue<Question>(Guid.Parse(queuePointer), Guid.Parse((from claim in HttpContext.User.Claims
                                                                                  where claim.Type == "guid"
                                                                                  select claim.Value).FirstOrDefault()), noteContext);
        }

        /// <summary>
        /// 构建问题队列
        /// </summary>
        /// <param name="questionnaire">指定调查问卷</param>
        /// <returns></returns>
        private IQueue<Question> QuestionQueueBuilder(Questionnaire questionnaire)
        {
            Guid currentUserGuid = Guid.Parse((from claim in HttpContext.User.Claims
                                               where claim.Type == "guid"
                                               select claim.Value).FirstOrDefault());
            List<Guid> guids = JsonConvert.DeserializeObject<List<Guid>>(questionnaire.Question);
            IOrderedQueryable<Question> questionRecords = from s in noteContext.Question
                                                          where s.Version != Guid.Empty
                                                          orderby s.Ctime descending
                                                          select s;
            IEnumerable<Question> orderd = from guid in guids
                                           join question in questionRecords on guid equals question.QuestionGuid
                                           select question;
            IQueue<Question> questionQueue = new BasicQueue<Question>(Guid.NewGuid(), currentUserGuid, noteContext, orderd.ToArray());
            return questionQueue;

        }

        /// <summary>
        /// 构建前端问题元数据
        /// </summary>
        /// <param name="queue">问题队列</param>
        /// <returns></returns>
        private IQuestion QuestionContentBuilder(IQueue<Question> queue)
        {
            if (queue.Count == 0)
            {
                throw new InvalidOperationException("请求的资源不存在");
            }
            else
            {
                int count = queue.Count;
                Question x = queue.Dequeue();
                IQuestion result;
                switch (x.Type)
                {
                    case nameof(ToastNotice):
                        {
                            result = ParseQuestionContent<ToastNotice>(x);
                            break;
                        }
                    case nameof(BooleanOption):
                        {
                            result = ParseQuestionContent<BooleanOption>(x);
                            break;
                        }
                    case nameof(SingleOption):
                        {
                            result = ParseQuestionContent<SingleOption>(x);
                            break;
                        }
                    case nameof(Conclusion)://判断题型是否为最后一个题
                        {
                            if (queue.Count == 0)
                            {
                                result = ParseQuestionContent<BreakConclusion>(x);
                                break;
                            }
                            else
                            {
                                result = ParseQuestionContent<Conclusion>(x);
                                break;
                            }
                        }
                    case nameof(MultiOption):
                        {
                            result = ParseQuestionContent<MultiOption>(x);
                            break;
                        }
                    case nameof(TextBox):
                        {
                            result = ParseQuestionContent<TextBox>(x);
                            break;
                        }
                    case nameof(PhotoGraph):
                        {
                            result = ParseQuestionContent<PhotoGraph>(x);
                            break;
                        }
                    case nameof(UploadFiles):
                        {
                            result = ParseQuestionContent<UploadFiles>(x);
                            break;
                        }
                    case nameof(Scoring):
                        {
                            result = ParseQuestionContent<Scoring>(x);
                            break;
                        }
                    default:
                        throw new InvalidOperationException("请求的资源不存在");
                }
                result.Guid = Guid.NewGuid();
                result.Progress = count.ToString();
                CreateAnswerRecord(result.Guid, x, queue.Guid);
                return result;

                ///// <summary>
                ///// 问题预处理，在answer表中创建记录
                ///// </summary>
                ///// <param name="queue"></param>
                ///// <returns></returns>
                void CreateAnswerRecord(Guid answerGuid, Question question, Guid queueGuid)
                {
                    if (Guid.TryParse((from claim in HttpContext.User.Claims
                                       where claim.Type == "guid"
                                       select claim.Value).FirstOrDefault(), out Guid userID))
                    {
                        Answer answer = new Answer
                        {
                            AnswerGuid = answerGuid,
                            Userid = userID,
                            RecordId = 0,
                            Ctime = DateTime.Now,
                            Mtime = DateTime.MinValue,
                            QuestionVersion = question.Version,
                            QuestionGuid = question.QuestionGuid,
                            Content = queueGuid.ToString()
                        };
                        noteContext.Answer.Add(answer);
                        noteContext.SaveChanges();
                    }
                }

                /// <summary>
                /// 解析问题前端元数据
                /// </summary>
                /// <typeparam name="T">题型</typeparam>
                /// <param name="question">问题的后端数据</param>
                /// <returns></returns>
                T ParseQuestionContent<T>(Question question) where T : IQuestion
                {
                    return JsonConvert.DeserializeObject<T>(question.Content);
                }

            }
        }

        /// <summary>
        /// 处理用户回答，将答案加入Answer表
        /// </summary>
        /// <param name="content"></param>
        /// <param name="answerGuid"></param>
        private bool SaveAnswer(Guid answerGuid, string content)
        {
            Answer result = (from s in noteContext.Answer
                             where s.AnswerGuid == answerGuid
                             select s).FirstOrDefault();
            if (result == default)
            {
                return false;
            }
            result.Mtime = DateTime.Now;
            result.Content = content;
            noteContext.Answer.Update(result);
            return noteContext.SaveChanges() == 1;
        }

        /// <summary>
        /// 其他组件通过此方法创建Ticket，匿名用户可以通过Ticket取回token，开始问卷
        /// </summary>
        /// <param name="questionnaireGuid"></param>
        /// <param name="userGuid"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        internal static Guid CreateTicket(Guid questionnaireGuid, Guid userGuid, string bearerToken)
        {
            lock (ticketsBook)
            {
                Ticket newTicket = new Ticket
                {
                    TicketID = Guid.NewGuid(),
                    QuestionnaireGuid = questionnaireGuid,
                    UserGuid = userGuid,
                    TicketingTime = DateTime.Now,
                    BearerToken = bearerToken
                };

                ticketsBook.Add(newTicket);

                return newTicket.TicketID;
            }
        }

        /// <summary>
        /// 查找时间超过24小时的Questionnaire记录会被删除
        /// </summary>
        public void RemoveTimeOutQueues()
        {
            var result = (from s in noteContext.Answer
                          select new
                          {
                              questionnaireGuid = s.QuestionnaireGuid,
                              mtime = s.Mtime
                          }).Select(a => new
                          {
                              a.questionnaireGuid,
                              mtime = ((DateTime.Now.Hour - a.mtime.Hour) >= 24)
                          });

            foreach (var item in result)
            {
                IQueryable<QuestionQueue> result1 = from s in noteContext.QuestionQueue
                                                    where s.QuestionnaireGuid == item.questionnaireGuid
                                                    select s;
                if (result1.Count() != 0)
                {
                    noteContext.Remove(result1);
                    noteContext.SaveChanges();
                };
            }
        }
        #endregion

        #region 控制器方法
        /// <summary>
        /// 开始答卷
        /// </summary>
        /// <param name="guid">问卷会传递过来的questionnaireguid</param>
        /// <param name="answer">结果表</param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        public IActionResult BeginTheAnswer([FromBody]AnswerViewModel answer)
        {
            //硬编码中医辩证体质问卷，此处应修改
            if (answer.Guid.ToString() == "b546b709-2b2b-4f6e-9f1f-64f281de8d5b")
            {
                Guid currentUserGuid = Guid.Parse((from claim in HttpContext.User.Claims
                                                   where claim.Type == "guid"
                                                   select claim.Value).FirstOrDefault());

                IQueue<Question> queue = new IntelligenceQueue(Guid.NewGuid(), currentUserGuid, noteContext);
                return Ok(QuestionContentBuilder(queue));
            }
            //判断guid是否为问卷guid
            else if (TryGetQuestionnaire(answer.Guid, out Questionnaire questionnaire))
            {
                //构建问题队列
                IQueue<Question> queue = QuestionQueueBuilder(questionnaire);
                //构建并返回题目元数据
                return Ok(QuestionContentBuilder(queue));
            }
            else if (answer.Guid != Guid.Empty)
            {
                //查找此answerGuid指向的QuestionQueue
                IQueue<Question> queue = SeekQuestionQueue(answer.Guid);
                string content = JsonConvert.SerializeObject(answer.Answer);

                if (queue.Count > 0 && SaveAnswer(answer.Guid, content))
                {
                    //构建并返回题目元数据
                    return Ok(QuestionContentBuilder(queue));
                }
                else
                {
                    return NotFound("问题队列为空，请不要提交BreakConclusion的GUID");
                }
            }
            else
            {
                return NotFound("错误的参数");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        /// <summary>
        /// 匿名用户提供ticket通过当前方法进入问卷
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="questionnaireGuid"></param>
        /// <returns>MercuryHtmlRenderedView</returns>
        public IActionResult StartByTicket(Guid ticketID)
        {
            lock (ticketsBook)
            {
                //查找所有签发超过15分钟的（过期）门票
                IEnumerable<Ticket> expiredTickets = from ticket in ticketsBook
                                                     where ticket.TicketingTime + TimeSpan.FromMinutes(15d) < DateTime.Now
                                                     select ticket;

                //如果存在过期门票，全部移除
                if (expiredTickets.Any())
                {
                    foreach (Ticket expiredTicket in expiredTickets)
                    {
                        ticketsBook.Remove(expiredTicket);
                    }
                }

                var currentTicket = ticketsBook.FirstOrDefault(x => x.TicketID == ticketID);

                if (currentTicket != default)
                {
                    ticketsBook.Remove(currentTicket);
                    EntryPointViewModel vm = new EntryPointViewModel
                    {
                        Guid = currentTicket.QuestionnaireGuid,
                        Authorization = currentTicket.BearerToken
                    };
                    return Ok(vm);
                }
                else
                {
                    return NotFound("无效的TicketID");
                }
            }
           
        }
        #endregion

        public QuestionnaireController(NoteContext db)
        {
            noteContext = db;
        }

        /// <summary>
        /// 这个队列专用于中医辩证体质调查问卷，未来应当拆分
        /// </summary>
        private class IntelligenceQueue : BasicQueue<Question>
        {
            public IntelligenceQueue(Guid queueGuid, Guid currentUser, NoteContext db) : base(queueGuid, currentUser, db)
            {
#warning 此处不应该硬编码，未来修改时拆分至其他项目
                Guid questionnaireGuid = Guid.Parse("b546b709-2b2b-4f6e-9f1f-64f281de8d5b");
                Questionnaire qn = (from questionnaire in db.Questionnaire
                                    where questionnaire.Version != Guid.Empty
                                    where questionnaire.QuestionnaireGuid == questionnaireGuid
                                    orderby questionnaire.Ctime
                                    select questionnaire).First();

                UserInformationRecord user = db.UserInformation.First(x => x.UserGuid == currentUser);
                List<Guid> questionList = JsonConvert.DeserializeObject<List<Guid>>(qn.Question);
                if (user.UserGender == UserInformationRecord.Gender.Man)
                {
                    //如果为男性，移除白带异常问题
                    questionList.Remove(Guid.Parse("23E317C9-D691-402E-8249-C0E4D3ED78D0"));
                }
                else
                {
                    //如果为女性，移除阴囊潮湿问题
                    questionList.Remove(Guid.Parse("A6C1DBC2-233D-4FA8-927B-8BAED0F709B3"));
                }

                //查找出用户已经答过的问题
                IQueryable<Guid> answeredQuestions = from answer in db.Answer
                                                     where answer.Userid == currentUser
                                                     where answer.Mtime != DateTime.MinValue
                                                     where questionList.Contains(answer.QuestionGuid)
                                                     select answer.QuestionGuid;
                if (answeredQuestions.Any())
                {
                    //移除
                    foreach (Guid answeredQuestion in answeredQuestions)
                    {
                        questionList.Remove(answeredQuestion);
                        //Console.WriteLine("移除了一个记录：" + answeredQuestion);
                    }
                }
                //Console.WriteLine($"该用户还有{questionList.Count}道题没有完成");
                IOrderedQueryable<Question> questionRecords = from s in db.Question
                                                              where s.Version != Guid.Empty
                                                              orderby s.Ctime descending
                                                              select s;
                foreach (Guid a in questionList)
                {
                    QuestionQueue queues = new QuestionQueue
                    {
                        QueueGuid = guid,
                        QuestionVersion = questionRecords.First(x => x.QuestionGuid == a).Version,
                        QueueUser = base.user,
                        Ctime = DateTime.Now
                    };
                    db.QuestionQueue.Add(queues);
                }

                db.QuestionQueue.Add(new QuestionQueue
                {
                    QueueGuid = guid,
                    QuestionVersion = Guid.Parse("CB849EFF-6C6F-433B-BB46-535570D6158B"),
                    Ctime = DateTime.Now,
                    QueueUser = base.user,
                    DynamicContent = JsonConvert.SerializeObject(new Conclusion
                    {
                        Link = $"Result/{base.user.ToString()}",
                        Title = "您的体质似乎是这样的……",
                        Options = new Dictionary<string, BooleanOption.Option>
                        {
                            {"Left",new BooleanOption.Option{Text="哦？"}},
                            {"Right",new BooleanOption.Option{Text="我想知道"}},
                        }
                    })
                });

                db.SaveChanges();

            }
        }

        #region 定义Ticket模型
        /// <summary>
        /// 门票，匿名用户通过一次性的ticketID访问MercurySurvey，获取已经渲染了Bearer Token的Razor页面
        /// </summary>
        internal class Ticket
        {
            /// <summary>
            /// 来自Jupiter Api的Bearer Token
            /// </summary>
            internal string BearerToken { get; set; }

            /// <summary>
            /// 门票ID
            /// </summary>
            internal Guid TicketID { get; set; }

            /// <summary>
            /// 用户ID用以和当前门票关联
            /// </summary>
            internal Guid UserGuid { get; set; }

            /// <summary>
            /// 问卷ID用以和当前门票关联
            /// </summary>
            internal Guid QuestionnaireGuid { get; set; }

            /// <summary>
            /// 记录门票创建时间
            /// </summary>
            internal DateTime TicketingTime { get; set; }
        }
        #endregion

        #region 实现一种读写发生在数据库中的Queue
        private class BasicQueue<T> : IQueue<T> where T : Question
        {
            protected readonly NoteContext noteContext;

            protected Guid guid;

            protected Guid user;

            public BasicQueue(Guid queueGuid, Guid currentUser, NoteContext db)
            {
                guid = queueGuid;
                user = currentUser;
                noteContext = db;
            }

            public BasicQueue(Guid queueGuid, Guid currentUser, NoteContext db, T[] array) : this(queueGuid, currentUser, db)
            {
                foreach (T a in array)
                {
                    QuestionQueue queues = new QuestionQueue
                    {
                        QueueGuid = guid,
                        QuestionVersion = a.Version,
                        QueueUser = user,
                        Ctime = DateTime.Now
                    };
                    noteContext.QuestionQueue.Add(queues);
                }
                noteContext.SaveChanges();
            }

            /// <summary>
            /// 队列指针
            /// </summary>
            public Guid Guid => guid;

            /// <summary>
            /// 获取队列长度
            /// </summary>
            int IQueue<T>.Count
            {
                get
                {
                    int result = (from s in noteContext.QuestionQueue
                                  where s.QueueGuid == guid
                                  select s).Count();
                    return result;
                }
            }

            /// <summary>
            /// 出队
            /// </summary>
            /// <returns></returns>
            public T Dequeue()
            {
                QuestionQueue queueRecord = (from s in noteContext.QuestionQueue
                                             where s.QueueGuid == guid
                                             orderby s.ID ascending
                                             select s).FirstOrDefault();
                noteContext.QuestionQueue.Remove(queueRecord);
                noteContext.SaveChanges();
                Question result = (from s in noteContext.Question where s.Version == queueRecord.QuestionVersion select s).FirstOrDefault();
                if (queueRecord.DynamicContent != null)
                {
                    result.Content = queueRecord.DynamicContent;
                }
                return result as T;

            }

            /// <summary>
            /// 入队
            /// </summary>
            /// <param name="obj"></param>
            public void Enqueue(T obj)
            {
                QuestionQueue queues = new QuestionQueue
                {
                    QueueGuid = guid,
                    QuestionVersion = obj.Version,
                    QueueUser = user,
                    Ctime = DateTime.Now
                };
                if (!string.IsNullOrEmpty(obj.Content))
                {
                    queues.DynamicContent = obj.Content;
                }
                noteContext.QuestionQueue.Add(queues);
                noteContext.SaveChanges();

            }
        }
        #endregion

        private interface IQueue<T> where T : Question
        {
            /// <summary>
            /// 队列指针
            /// </summary>
            Guid Guid { get; }

            /// <summary>
            /// 队列长度
            /// </summary>
            int Count { get; }

            /// <summary>
            /// 入队方法
            /// </summary>
            /// <param name="obj"></param>
            void Enqueue(T obj);

            /// <summary>
            /// 出队方法
            /// </summary>
            /// <returns></returns>
            T Dequeue();
        }
    }
}