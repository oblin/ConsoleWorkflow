using System;
using System.Collections.Generic;

namespace ConsoleUserWorkflow
{
    public class Approval
    {
        public string Id => Guid.NewGuid().ToString();

        public List<User> UserList { get; set; }
        public int Pointer { get; set; }
        public bool IsCancel { get; set; }
    }

    public class User
    {
        public string Id => Guid.NewGuid().ToString();
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
    }
}