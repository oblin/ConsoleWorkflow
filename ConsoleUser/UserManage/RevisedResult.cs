using AbhCare.Identity.Models;
using System.Collections.Generic;

namespace AbhCare.Workflow.UserManage
{
    public class RevisedResult
    {
        public List<ApplicationUser> ActivedUsers { get; internal set; }
        
        public List<ApplicationUser> InActivedUsers { get; internal set; }

        public List<ApplicationUser> NewUsers { get; internal set; }

        public List<ApplicationUser> DisabledUsers { get; set; }

        public List<ApplicationUser> EnabledUsers { get; set; }
    }
}