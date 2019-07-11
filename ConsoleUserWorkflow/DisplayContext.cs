using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleUserWorkflow
{
    public class DisplayContext : StepBody
    {
        public List<User> UserList { get; set; }
        public bool CloseApproval { get; set; }
        public int CurrentPointer { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var user = UserList[CurrentPointer];
            Console.WriteLine($"Current User: {user.Id}, {user.IsApproved}");

            Console.WriteLine("Approval? (y: yes/n: no/a: add)");
            var result = Console.ReadLine();
            switch (result.ToUpper())
            {
                case "Y":
                    user.IsApproved = true;
                    break;
                case "A":
                    // 先設定同意
                    user.IsApproved = true;
                    // 在新增使用者簽核
                    UserList.Add(new User());
                    break;
                case "N":
                    // 否決簽核流程
                    SetAllApprovalToFalse();
                    CloseApproval = true;
                    break;
            }

            return ExecutionResult.Next();
        }

        private void SetAllApprovalToFalse()
        {
            foreach(var user in UserList)
            {
                user.IsApproved = false;
            }
        }
    }
}