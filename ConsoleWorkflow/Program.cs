using System;
using System.IO;
using System.Threading;

namespace ConsoleWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            var param = ParseArgs(args);
            if (param == null)
                return;

            var output = $"Console Invoked. fk = {param.ForeignKey}, org = {param.Org}";

            Console.Out.WriteLine(output);

            var path = $"D:\\Temp\\{param.Org}\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.CreateText(path + param.ForeignKey);
        }

        private static Param ParseArgs(string[] args)
        {
            switch(args.Length)
            {
                case 2:
                    return new Param(args[0], args[1]);
                default:
                    throw new ArgumentException();
            }
        }
    }

    class Param
    {
        public string ForeignKey { get; set; }
        public string Org { get; set; }

        public Param(string fk, string org)
        {
            this.ForeignKey = fk;
            this.Org = org;
        }
    }
}
