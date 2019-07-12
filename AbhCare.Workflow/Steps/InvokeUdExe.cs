using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class InvokeUdExe : StepBody
    {
        public string[] Params { get; set; }

        public WorkItem WorkItem { get; set; }

        private readonly IUdWorkflowConfig _fileService;

        public InvokeUdExe(IUdWorkflowConfig fileSerivce)
        {
            _fileService = fileSerivce;
            Console.WriteLine($"ExeWorkflow: {fileSerivce.ExePath}");
        }


        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                InvokeExe();
            }
            catch (Exception ex)
            {
                WorkItem.RaiseError("InvokeUdExe", ex.Message);
                throw;
            }

            WorkItem.RaiseStart();

            return ExecutionResult.Next();
        }

        private void InvokeExe()
        {
            var process = new Process();
            process.StartInfo.FileName = _fileService.ExePath;
            process.StartInfo.Arguments = string.Join(" ", Params);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();

            process.WaitForExit();

            Console.WriteLine("Exit of wait.");

            process.Close();

            // for Event handler with Standard output，但 PB 的程式無法成功
            //process.StartInfo.RedirectStandardOutput = true;
            //process.OutputDataReceived += (sender, data) =>
            //{
            //    if (data.Data != null)
            //        Console.WriteLine("Stdout: " + data.Data);
            //};


            //process.StartInfo.RedirectStandardError = true;
            //process.ErrorDataReceived += (sender, data) =>
            //{
            //    if (data.Data != null)
            //        Console.WriteLine("Stderr: " + data.Data + "\n");
            //};

            //process.BeginOutputReadLine();  // Read each line
            //process.BeginErrorReadLine();   // Read each line
        }
    }
}