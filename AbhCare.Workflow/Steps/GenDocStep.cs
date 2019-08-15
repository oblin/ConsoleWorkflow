using RabbitLogging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow.Steps
{
    public class GenDocStep : StepBody
    {
        public string[] Params { get; set; }
        public WorkItem WorkItem { get; set; }

        private readonly IUdWorkflowConfig _fileService;
        private readonly RLogger _rLogger;

        public GenDocStep(IUdWorkflowConfig fileSerivce, RLogger rLogger)
        {
            _fileService = fileSerivce;
            _rLogger = rLogger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var filename = Path.Combine(_fileService.OrderPath, WorkItem.Id + ".txt");
            Console.WriteLine($"create file at : {filename}");

            try
            {
                using (var writer = File.CreateText(filename))
                {
                    writer.WriteLine(string.Join(" ", Params));
                }

                //using (var file = new FileStream(filename, FileMode.CreateNew))
                //{
                //    using (StreamWriter outputFile = new StreamWriter(file, Encoding.UTF8))
                //    {
                //        outputFile.WriteLine(string.Join(" ", Params));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Workflow: {WorkItem.Id} start Error");
                WorkItem.RaiseErrorEvent("InvokeUdExe", ex.Message);
                throw;
            }

            WorkItem.RaiseStartEvent();

            return ExecutionResult.Next();
        }
    }
}
