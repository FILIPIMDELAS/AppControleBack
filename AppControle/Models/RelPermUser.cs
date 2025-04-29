namespace AppControle.Models
{
    public class RelPermUser
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? UserPermissionId { get; set; }
        public UserPermission? UserPermission { get; set; }
    }
}
