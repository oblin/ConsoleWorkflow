using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace AbhCare.Workflow
{
    public class ExeWorkflowService
    {
        private readonly IWorkflowHost _host;

        public ExeWorkflowService(IWorkflowHost host)
        {
            _host = host;
        }

        /// <summary>
        /// 啟動 ExeWorkflow 與 檔案產生監測，請注意只能執行一次，建議在 Startup 中執行 
        /// </summary>
        /// <param name="folder">執行檔產生檔案的目錄</param>
        /// <param name="eventName">呼叫的 ExeWorkflow.Waitfor 的 Event Name </param>
        public void Start(string folder, string eventName)
        {
            var fileWatcher = new FileWatcher(folder, eventName);
            fileWatcher.FileDetectHandler += FileWatcher_FileDetectHandler;
            
            _host.RegisterWorkflow<ExeWorkflow, ExeWorkItem>();

            _host.Start();
            fileWatcher.Start();
        }

        public void Stop()
        {
            _host.Stop();
        }

        /// <summary>
        /// 將新的需求傳入 work flow，並啟動執行檔執行
        /// </summary>
        /// <param name="id">物件 ID</param>
        /// <param name="parameters">呼叫執行檔的參數</param>
        /// <returns></returns>
        public ExeWorkItem Add(string id, string[] parameters)
        {
            var item = new ExeWorkItem
            {
                Id = id,
                Params = parameters,
            };
            item.WorkflowId = _host.StartWorkflow("ExeWorkflow", item).Result;
            return item;
        }

        private void FileWatcher_FileDetectHandler(object sender, EventArgs e)
        {
            var fileEvent = (FileEventArgs)e;
            _host.PublishEvent(fileEvent.EventName, fileEvent.WorkflowId, fileEvent.EventData);
        }
    }
}
