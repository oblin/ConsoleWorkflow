using System.Collections.Generic;

namespace AbhCare.Identity.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentClinic { get; set; }

        public string LogoutUrl { get; set; }

        public List<string> Clinics { get; set; }
    }
}
