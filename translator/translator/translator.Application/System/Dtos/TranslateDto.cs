using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace translator.Application.System.Dtos
{
    public class TranslateDto
    {
        
    }
    /// <summary>
    /// 声音返回文字
    /// </summary>
    public class VoiceRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// textval
        /// </summary>
        public string val { get; set; }
    }
}