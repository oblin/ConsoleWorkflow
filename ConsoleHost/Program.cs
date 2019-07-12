using AbhCare.Workflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WorkflowCore.Interface;
using WorkflowCore.Models.LifeCycleEvents;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureServices();

            var hostService = serviceProvider.GetService<ExeWorkflowService>();
            var fileLocation = serviceProvider.GetService<IUdWorkflowConfig>();
            // TODO: make it @ appsettings
            hostService.Start();

            var item1 = CreateWorkItem1(fileLocation.OutputFolder);
            ExeWorkItem exeParam = hostService.Add(item1.Id, item1.Params);

            //exeParam.StartHandler += ExeParam_StartHandler;
            //exeParam.ErrorHandler += ExeParam_ErrorHandler;
            //exeParam.FinishHandler += ExeParam_FinishHandler;
            Thread.Sleep(1000);
            var item2 = CreateWorkItem2(fileLocation.OutputFolder);
            exeParam = hostService.Add(item2.Id, item2.Params);

            var item3 = CreateWorkItem3(fileLocation.OutputFolder);
            hostService.Add(item3.Id, item3.Params);

            var item4 = CreateWorkItem4(fileLocation.OutputFolder);
            hostService.Add(item4.Id, item4.Params);

            Console.ReadLine();
            Console.ReadLine();
            hostService.Stop();
        }

        private static NisExeWorkItem CreateWorkItem1(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415", 
                "NEAA", "測試PriceName", "20190705", "0900", "1", "2998490d-b958-4e67-a519-7734b71bd6fb", 
                1, "A", output);
        }

        private static NisExeWorkItem CreateWorkItem2(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "2998490d-b958-4e67-a519-7734b71bd6fb",
                1, "A", output);
        }

        private static NisExeWorkItem CreateWorkItem3(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "3998490d-b958-4e67-a519-7734b71bd6fb",
                1, "E", output);
        }

        private static NisExeWorkItem CreateWorkItem4(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "4998490d-b958-4e67-a519-7734b71bd6fb",
                1, "E", output);
        }

        private static void ExeParam_FinishHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} had been Finished, foreign key: {item.ErrorMessage}");
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


        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(logger => logger.AddDebug());
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddSingleton<ExeWorkflowService>();

            var configs = new ConfigurationBuilder().AddInMemoryCollection().Build();

            configs["path"] = @"D:\999\nis_order\nis_order.exe";
            configs["output"] = @"D:\Temp\";
            configs["backup"] = @"D:\999\Backup\";
            configs["event"] = "FileCreated";

            services.AddTransient<IUdWorkflowConfig, UdWorkflowConfig>(_ => {
                var fileService = new UdWorkflowConfig();
                fileService.ExePath = configs["path"];
                fileService.OutputFolder = configs["output"];
                fileService.BackupFolder = configs["backup"];
                fileService.EventName = configs["event"];
                return fileService;
            });
            services.AddTransient<InvokeUdExe>();   // 將 InvokedUdExe 加入才可以進行 DI

            return services.BuildServiceProvider();
        }
    }
}
