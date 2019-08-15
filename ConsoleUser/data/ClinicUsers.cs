namespace AbhCare.Identity.Data
{
    public class ClinicUsers
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string UserId { get; set; }
        public bool IsManageable { get; set; }
    }
}
