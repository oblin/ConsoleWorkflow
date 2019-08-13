using RabbitLogging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models.LifeCycleEvents;

namespace AbhCare.Workflow
{
    public class ExeWorkflowService
    {
        private readonly IWorkflowHost _host;
        private readonly IUdWorkflowConfig _fileConfig;
        private ConcurrentDictionary<string, string> _workflowDataMappings = new ConcurrentDictionary<string, string>();
        private FileWatcher _fileWatcher;
        private readonly RLogger _rLogger;

        public ExeWorkflowService(IWorkflowHost host, IUdWorkflowConfig config, RLogger rLogger)
        {
            _host = host;
            _fileConfig = config;
            _rLogger = rLogger;
        }

        /// <summary>
        /// 啟動 ExeWorkflow 與 檔案產生監測，請注意只能執行一次，建議在 Startup 中執行 
        /// </summary>
        /// <param name="folder">執行檔產生檔案的目錄</param>
        /// <param name="eventName">呼叫的 ExeWorkflow.Waitfor 的 Event Name </param>
        public void Start()
        {
            _fileWatcher = new FileWatcher(_fileConfig.OutputFolder, _fileConfig.EventName, _fileConfig.BackupFolder);
            _fileWatcher.FileDetectHandler += FileWatcher_FileDetectHandler;
            
            _host.RegisterWorkflow<ExeWorkflow, ExeWorkItem>();
            _host.OnLifeCycleEvent += Host_OnLifeCycleEvent;
            _host.Start();
            _fileWatcher.Start();
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
        public ExeWorkItem Add(ExeWorkItem item)
        {
            while(_workflowDataMappings.Count > 7)
            {
                // 一次超過七個 process （可能跟 CPU thread 有關） 會造成無法觸發 Event 的問題
                Thread.Sleep(5000);
            }
            item.WorkflowId = _host.StartWorkflow("ExeWorkflow", item).Result;
            _workflowDataMappings.TryAdd(item.WorkflowId, item.Id);

            _rLogger.WriteDebug($"執行開始 {item.Id} 頻率： {((NisExeWorkItem)item).TakeTime}");

            return item;
        }

        private void FileWatcher_FileDetectHandler(object sender, EventArgs e)
        {
            var fileEvent = (FileEventArgs)e;
            _host.PublishEvent(fileEvent.EventName, fileEvent.WorkflowId, fileEvent.EventData);
        }

        private void Host_OnLifeCycleEvent(WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent evt)
        {
            if (evt is WorkflowCompleted)
            {
                var type = evt as WorkflowCompleted;

                _workflowDataMappings.TryRemove(type.WorkflowInstanceId, out string id);

                var file = Path.Combine(_fileConfig.OutputFolder, type.WorkflowInstanceId + ".txt");
                using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("big5")))
                {
                    var line = sr.ReadLine();

                    _rLogger.WriteDebug($"執行結束 {id}：{line}");
                }

                _fileWatcher.MoveToBackupFolder(file);
                Console.WriteLine($"Life cycle type: {nameof(WorkflowCompleted)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowError)
            {
                var type = evt as WorkflowError;
                _workflowDataMappings.TryRemove(type.WorkflowInstanceId, out string id);
                Console.WriteLine($"Life cycle type: {nameof(WorkflowError)}, workflow id {type.WorkflowInstanceId}, message: {type.Message}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            }
        }
    }
}
