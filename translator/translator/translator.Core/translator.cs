using Furion.DatabaseAccessor;

namespace translator.Core
{
    public class voiceText : IEntity
    {
        /// <summary>
        /// 手工定义 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
    }
}