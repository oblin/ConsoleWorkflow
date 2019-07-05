using System;
using System.Diagnostics;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class InvokeUdExe : StepBody
    {
        // TODO: make it @ appsettings
        private readonly string _exePath = @"D:\Practices\Others\ConsoleWorkflow\pb_console_test\pb_console_test.exe";
        public string[] Params { get; set; }

        public WorkItem WorkItem { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            try
            {
                InvokeExe(_exePath);
            }
            catch (Exception ex)
            {
                WorkItem.RaiseError("InvokeUdExe", ex.Message);
                throw;
            }

            WorkItem.RaiseStart();

            return ExecutionResult.Next();
        }

        private void InvokeExe(string exePath)
        {
            var process = new Process();
            process.StartInfo.FileName = exePath;
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