using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DUWENINK.ConfigManagerHelper
{
    public class ConfigManagerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConfigPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultConfigDirectory { get; set; } = "ConfigurationFiles";
        /// <summary>
        /// 
        /// </summary>
        public string CustomConfigDirectory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}
