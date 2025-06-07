using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizSystem_backend.Models
{
    public class TeacherSubjectClass
    {
        public int TeacherId { get; set; }

        public int SubjectId { get; set; }

        public int ClassId { get; set; }

        public virtual Teacher Teacher { get; set; } = null!;

        public virtual Subject Subject { get; set; } = null!;

        public virtual Class Class { get; set; } = null!;
    }
}
