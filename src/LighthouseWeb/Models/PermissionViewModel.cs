namespace LighthouseWeb.Models
{
    public class PermissionViewModel
    {
        public string Directory { get; set; }
        public string User { get; set; }
        public Access Access { get; set; }
        public string Rights { get; set; }
        public string LoggedInUser { get; set; }
    }

    public enum Access
    {
        Allowed,
        Denied
    }
}