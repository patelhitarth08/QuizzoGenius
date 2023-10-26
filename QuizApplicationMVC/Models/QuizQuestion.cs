namespace QuizApplicationMVC.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public int QuizId { get; set; } // Foreign key
        public string QuestionName { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectOption { get; set; }

        public Quiz? Quiz { get; set; }
    }
}
