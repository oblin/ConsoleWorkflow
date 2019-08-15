using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AbhCare.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DefaultClinic { get; set; }
        public bool IsApproved { get; set; }

        private string displayName;
        [StringLength(20)]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                    return this.UserName;
                else return displayName;
            }
            set
            {
                displayName = value;
            }
        }
    }
}
