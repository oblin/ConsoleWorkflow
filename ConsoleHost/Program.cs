using AbhCare.Workflow;
using AbhCare.Workflow.Steps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitLogging;
using System;
using System.Text;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //var file = @"D:\999\Backup\3d6653b0-0c99-4093-9527-632444e6475e.txt";
            //using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("big5")))
            //{
            //    var line = sr.ReadLine();

            //    byte[] myEncodedBytes = Encoding.ASCII.GetBytes(line);
            //    string myDecodedString = Encoding.UTF8.GetString(myEncodedBytes);

            //    Console.WriteLine(myDecodedString);
            //}


            IServiceProvider serviceProvider = ConfigureServices();

            var hostService = serviceProvider.GetService<ExeWorkflowService>();
            var fileLocation = serviceProvider.GetService<IUdWorkflowConfig>();
            // TODO: make it @ appsettings
            hostService.Start();

            //var item1 = CreateWorkItem1(fileLocation.OutputFolder);
            //ExeWorkItem exeParam = hostService.Add(item1.Id, item1.Params);

            //var item2 = CreateWorkItem2(fileLocation.OutputFolder);
            //exeParam = hostService.Add(item2.Id, item2.Params);

            //var item3 = CreateWorkItem3(fileLocation.OutputFolder);
            //hostService.Add(item3.Id, item3.Params);

            //var item4 = CreateWorkItem4(fileLocation.OutputFolder);
            //hostService.Add(item4.Id, item4.Params);

            for (var i = 0; i < 2; i++)
            {
                var item = CreateWorkItem(fileLocation.OutputFolder, Guid.NewGuid().ToString(), i);
                hostService.Add(item);
            }

            Console.ReadLine();
            Console.ReadLine();
            hostService.Stop();
        }

        private static NisExeWorkItem CreateWorkItem(string output, string id, int count)
        {
            var fre = "1";
            if (count % 6 == 0)
            {
                fre = "BIW";
            }
            else if (count % 5 == 0)
            {
                fre = "2";
            }
            else if (count % 3 == 0)
            {
                fre = "Q3H";
            }
            else if (count % 4 == 0)
            {
                fre = "Q4H";
            }
            else if (count % 2 == 0)
            {
                fre = "TIW";
            }

            return new NisExeWorkItem("GHN", "I1070000087", "00000280",
                "TEST", "測試中文名稱", "20190705", "0900", fre, id,
                1, "A", output);
        }

        private static NisExeWorkItem CreateWorkItem1(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "5998490d-b958-4e67-a519-7734b71bd6fb",
                1, "D", output);
        }

        private static NisExeWorkItem CreateWorkItem2(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "5198490d-b958-4e67-a519-7734b71bd6fb",
                1, "D", output);
        }

        private static NisExeWorkItem CreateWorkItem3(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "8998490d-b958-4e67-a519-7734b71bd6fb",
                1, "A", output);
        }

        private static NisExeWorkItem CreateWorkItem4(string output)
        {
            // ASN;I1080000055;00000415;NEAA;修改的fee_name;20190705;;;;1;1;source_id_123;A;d:\999\中文\777;test_file_name4
            return new NisExeWorkItem("ASN", "I1080000055", "00000415",
                "NEAA", "測試PriceName", "20190705", "0900", "1", "7998490d-b958-4e67-a519-7734b71bd6fb",
                1, "D", output);
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

            Console.WriteLine($"workflow id: {workflow.Id} FAILED! exe params = {exeParam.Id} - {String.Join(',', exeParam.ComposeParameters())}");
            Console.WriteLine($"Step: {step.Name}, reason: {exception.Message}");
        }


        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(logger => logger.AddDebug());

            var connectionString = @"Server=localhost;Port=5432;Database=workflow;User Id=postgres;Password=490910;";

            services.AddWorkflow(config => config.UsePostgreSQL(connectionString, true, true));
            //services.AddWorkflow();

            services.AddSingleton<ExeWorkflowService>();

            var configs = new ConfigurationBuilder().AddInMemoryCollection().Build();

            configs["exepath"] = @"C:\nis_order\nis_order.exe";
            configs["orderpath"] = @"C:\nis_order\cmd_beg";
            configs["output"] = @"D:\Temp\";
            configs["backup"] = @"D:\999\Backup\";
            configs["event"] = "FileCreated";

            // RLogger
            configs["rlogger:file"] = @"D:\999\workflow";
            configs["rlogger:debug"] = "true";
            configs["rlogger:system"] = "ConsoleHost";

            services.AddTransient<IUdWorkflowConfig, UdWorkflowConfig>(_ =>
            {
                var fileService = new UdWorkflowConfig();
                fileService.ExePath = configs["exepath"];
                fileService.OrderPath = configs["orderpath"];
                fileService.OutputFolder = configs["output"];
                fileService.BackupFolder = configs["backup"];
                fileService.EventName = configs["event"];
                return fileService;
            });
            services.AddTransient<InvokeUdExe>();   // 將 InvokedUdExe 加入才可以進行 DI
            services.AddTransient<GenDocStep>();
            services.AddTransient<NofifyWorkflowTimeout>();
            services.AddSingleton(RLogger.CreateInstance(configs));

            return services.BuildServiceProvider();
        }
    }
}
