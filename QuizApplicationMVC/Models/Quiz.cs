namespace QuizApplicationMVC.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public Users? User { get; set; }
        public List<Questions>? Questions { get; set; }
        public List<QuizUserHistory>? quizUserHistory { get; set; }

    }
}
