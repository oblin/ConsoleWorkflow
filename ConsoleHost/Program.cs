using AbhCare.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WorkflowCore.Interface;

namespace ConsoleHost
{
    class Program
    {
        static IWorkflowHost _host;
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            _host = serviceProvider.GetService<IWorkflowHost>();
            _host.RegisterWorkflow<ExeWorkflow, ExeWorkItem>();
            //_host.OnStepError += Workflow_OnStepError;
            _host.Start();

            for (int i = 0; i < 3; i++)
            {
                ExeWorkItem exeParam = CreateNewWorkItem();
                exeParam.WorkflowId = _host.StartWorkflow("ExeWorkflow", exeParam).Result;
                exeParam.StartHandler += ExeParam_StartHandler;
                exeParam.ErrorHandler += ExeParam_ErrorHandler;
                exeParam.FinishHandler += ExeParam_FinishHandler;
                Console.WriteLine("workflow id: " + exeParam.WorkflowId + " started");
            }

            FileWatcher();

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            _host.Stop();
        }

        private static void ExeParam_FinishHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} had been Finished, foreign key: {item.ErrorMessage}");
        }

        private static string _path = @"D:\Practices\Others\ConsoleWorkflow\pb_console_test\pb_console_test.exe";
        private static ExeWorkItem CreateNewWorkItem()
        {
            return new ExeWorkItem
            {
                Id = Guid.NewGuid().ToString(),
                Params = new string[] { "fk100", "AYN" },
                Path = _path
            };
        }

        private static void ExeParam_StartHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} had been Started");
        }
        private static void ExeParam_ErrorHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} at {item.Step} step had been cancel, message: {item.ErrorMessage}");
        }

        private static void Workflow_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            var exeParam = workflow.Data as ExeWorkItem;
            
            Console.WriteLine($"workflow id: {workflow.Id} FAILED! exe params = {exeParam.Id} - {String.Join(',', exeParam.Params)}");
            Console.WriteLine($"Step: {step.Name}, reason: {exception.Message}");
        }

        private static void FileWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.CreationTime
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Path = "d:\\Temp\\";
            watcher.Filter = "*.txt";
            Console.WriteLine($"DetectFileResult: {watcher.Path}");

            //Subscribe to the Created event.
            watcher.Created += Watcher_FileCreated;

            watcher.EnableRaisingEvents = true;
        }

        private static void Watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            var filename = Path.GetFileNameWithoutExtension(e.FullPath);
            if (filename.Length != 36)
            {
                Console.WriteLine($"{filename} is not a workflow event id");
                return;
            }
            Watcher_FileCreated_Parameter(filename);
        }

        private static void Watcher_FileCreated_Parameter(string id)
        {
            Console.WriteLine("File had been created -> " + id);
            string eventData = "ForeignKey";
            _host.PublishEvent("FileCreated", id, eventData);

        }
        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder.AddDebug());
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            //services.AddSingleton<ExeWorkflowService>();

            return services.BuildServiceProvider();
        }
    }
}
