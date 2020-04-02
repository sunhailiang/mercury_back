using System;

namespace Characters
{
    /// <summary>
    /// 表示一段历程、事迹
    /// </summary>
    public interface IDeedsBase
    {
        /// <summary>
        /// 日期
        /// </summary>
        string Date { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        string Titel { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        string SubTitel { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        string Content { get; set; }
    }

}
