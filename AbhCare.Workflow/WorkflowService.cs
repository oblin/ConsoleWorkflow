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

        public void Start(string folder, string eventName)
        {
            var fileWatcher = new FileWatcher(folder, eventName);
            fileWatcher.FileDetectHandler += FileWatcher_FileDetectHandler;
            _host.RegisterWorkflow<ExeWorkflow, ExeWorkItem>();

            _host.Start();
        }

        public T Add<T>(T item) where T : WorkItem, new()
        {
            item.WorkflowId = _host.StartWorkflow("ExeWorkflow", item).Result;
            return item;
        }

        private void FileWatcher_FileDetectHandler(object sender, EventArgs e)
        {
            var fileEvent = (FileEventArgs)e;
            _host.PublishEvent(fileEvent.EventName, fileEvent.Id, fileEvent.EventData);
        }
    }
}
