namespace AppControle.Models
{
    public class RelUserWork
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int WorkId { get; set; }
        public Work? Work { get; set; }
    }
}
