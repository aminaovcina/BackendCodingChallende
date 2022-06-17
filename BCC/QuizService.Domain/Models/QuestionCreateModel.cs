namespace QuizService.Domain.Models;

public class QuestionCreateModel
{
    public QuestionCreateModel(string text)
    {
        Text = text;
    }

    public string Text { get; set; }
}