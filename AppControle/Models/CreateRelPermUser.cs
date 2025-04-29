namespace AppControle.Models
{
    public class CreateRelPermUser
    {
        public int Id { get; set; } 
        public int? UserId { get; set; }
        public int? UserPermissionId { get; set; }
    }
}
