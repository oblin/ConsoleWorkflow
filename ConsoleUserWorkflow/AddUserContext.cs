using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleUserWorkflow
{
    public class AddUserContext : StepBody
    {
        public List<User> UserList { get; set; }
        public User User { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            UserList.Add(User);
            Console.WriteLine($"user count: {UserList.Count}");
            return ExecutionResult.Next();
        }
    }
}