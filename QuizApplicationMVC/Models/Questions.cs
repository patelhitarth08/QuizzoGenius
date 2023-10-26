namespace QuizApplicationMVC.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public string QuestionsName { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectOption { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

    }
}
