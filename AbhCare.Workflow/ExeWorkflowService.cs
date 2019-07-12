using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models.LifeCycleEvents;

namespace AbhCare.Workflow
{
    public class ExeWorkflowService
    {
        private readonly IWorkflowHost _host;
        private readonly IUdWorkflowConfig _fileConfig;

        public ExeWorkflowService(IWorkflowHost host, IUdWorkflowConfig config)
        {
            _host = host;
            _fileConfig = config;
        }

        /// <summary>
        /// 啟動 ExeWorkflow 與 檔案產生監測，請注意只能執行一次，建議在 Startup 中執行 
        /// </summary>
        /// <param name="folder">執行檔產生檔案的目錄</param>
        /// <param name="eventName">呼叫的 ExeWorkflow.Waitfor 的 Event Name </param>
        public void Start()
        {
            var fileWatcher = new FileWatcher(_fileConfig.OutputFolder, _fileConfig.EventName, _fileConfig.BackupFolder);
            fileWatcher.FileDetectHandler += FileWatcher_FileDetectHandler;
            
            _host.RegisterWorkflow<ExeWorkflow, ExeWorkItem>();
            _host.OnLifeCycleEvent += Host_OnLifeCycleEvent;
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

        private void Host_OnLifeCycleEvent(WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent evt)
        {
            //if (evt is WorkflowSuspended)
            //{
            //    var type = evt as WorkflowSuspended;
            //    Console.WriteLine($"Life cycle type: {nameof(WorkflowSuspended)}, workflow id {type.WorkflowInstanceId}");
            //}
            //else if (evt is WorkflowStarted)
            //{
            //    var type = evt as WorkflowStarted;
            //    Console.WriteLine($"Life cycle type: {nameof(WorkflowStarted)}, workflow id {type.WorkflowInstanceId}");
            //}
            if (evt is WorkflowCompleted)
            {
                var type = evt as WorkflowCompleted;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowCompleted)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowError)
            {
                var type = evt as WorkflowError;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowError)}, workflow id {type.WorkflowInstanceId}, message: {type.Message}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            }
            //else if (evt is WorkflowResumed)
            //{
            //    var type = evt as WorkflowResumed;
            //    Console.WriteLine($"Life cycle type: {nameof(WorkflowResumed)}, workflow id {type.WorkflowInstanceId}");
            //}
            //else if (evt is WorkflowTerminated)
            //{
            //    var type = evt as WorkflowTerminated;
            //    Console.WriteLine($"Life cycle type: {nameof(WorkflowTerminated)}, workflow id {type.WorkflowInstanceId}");
            //}
            //else if (evt is StepCompleted)
            //{
            //    var type = evt as StepCompleted;
            //    Console.WriteLine($"Life cycle type: {nameof(StepCompleted)}, workflow id {type.WorkflowInstanceId}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            //}
            //else if (evt is StepStarted)
            //{
            //    var type = evt as StepStarted;
            //    Console.WriteLine($"Life cycle type: {nameof(StepStarted)}, workflow id {type.WorkflowInstanceId}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            //}
        }
    }
}
