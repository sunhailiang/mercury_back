namespace Characters
{
    /// <summary>
    /// 带有图片详情的图片对象
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int UUID { get; set; }

        /// <summary>
        /// 图册图片的筛选器
        /// </summary>
        public GraphType Filter { get; set; }

        /// <summary>
        /// 图片的标题
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 图片的uri
        /// </summary>
        public string PhotoUri { get; set; }

        /// <summary>
        /// 图片的描述
        /// </summary>
        public string Description { get; set; }
    }

}