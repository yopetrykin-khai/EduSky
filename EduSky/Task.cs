using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string MethodicalFilePath { get; set; }
        public Dictionary<string, string> UserAnswers { get; set; } = new(); // email -> filepath

        public event Action<string, string>? AnswerSubmitted;

        public void SubmitAnswer(string login, string filePath)
        {
            UserAnswers[login] = filePath;
            AnswerSubmitted?.Invoke(login, filePath);
        }
    }
}
