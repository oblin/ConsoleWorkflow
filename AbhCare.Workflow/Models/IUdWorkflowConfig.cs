using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbhCare.Workflow
{
    public class UdWorkflowConfig : IUdWorkflowConfig
    {
        public string ExePath { get; set; }
        public string OutputFolder { get; set; }
        public string EventName { get; set; }
        public string BackupFolder { get; set; }
        public string OrderPath { get; set; }
    }

    /// <summary>
    /// 設定 UD 執行檔的參數
    /// </summary>
    public interface IUdWorkflowConfig
    {
        /// <summary>
        /// UD 執行檔路徑
        /// </summary>
        string ExePath { get; set; }
        /// <summary>
        /// UD 執行檔的輸出目錄
        /// </summary>
        string OutputFolder { get; set; }
        /// <summary>
        /// 呼叫 workflow 的 Event Name
        /// </summary>
        string EventName { get; set; }
        string BackupFolder { get; set; }
        string OrderPath { get; set; }
    }
}
