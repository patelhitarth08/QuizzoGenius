namespace QuizApplicationMVC.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Quiz>? quizzes { get; set; }
        public List<QuizUserHistory>? quizUserHistory { get; set; }
    }
}
