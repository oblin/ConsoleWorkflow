using System;
using System.IO;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class DetectFileResult : StepBody
    {
        private readonly IWorkflowHost _host;

        public DetectFileResult(IWorkflowHost host)
        {
            _host = host;
        }

        public string Location { get; set; }
        public string FileName { get; set; }
        public string WorkflowId { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(Location);
            watcher.Filter = $"{FileName}.*";
            Console.WriteLine($"DetectFileResult: {watcher.Path} - {FileName}");

            //Subscribe to the Created event.
            watcher.Created += Watcher_FileCreated;

            watcher.EnableRaisingEvents = true;

            return ExecutionResult.Next();
        }

        private void Watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File had been created -> " + e.Name);

            //_host.PublishEvent("FileCreated", WorkflowId, null);

        }
    }
}