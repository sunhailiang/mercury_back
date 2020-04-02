using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercurySurvey.Models
{
    /// <summary>
    /// 渲染流式调查问卷前端页面所需数据
    /// </summary>
    public class EntryPointViewModel
    {
        /// <summary>
        /// 问卷GUID
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// 来自Jupiter的Bearer Token
        /// </summary>
        public string Authorization { get; set; }
    }
}
