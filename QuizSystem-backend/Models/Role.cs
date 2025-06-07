namespace QuizSystem_backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
