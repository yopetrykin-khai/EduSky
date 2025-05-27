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
        public List<StudentAnswer> Answers { get; set; } = new();
        public event Action<string, string>? AnswerSubmitted;
        public void SubmitAnswer(string login, string filePath)
        {
            var existing = Answers.FirstOrDefault(a => a.StudentLogin == login);
            if (existing != null)
            {
                existing.FilePath = filePath;
            }
            else
            {
                Answers.Add(new StudentAnswer
                {
                    StudentLogin = login,
                    FilePath = filePath
                });
            }
            AnswerSubmitted?.Invoke(login, filePath);
        }
    }
}
