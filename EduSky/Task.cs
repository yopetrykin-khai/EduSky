using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class Task
    {
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Enter the name!.");
                if (value.Length > 18)
                    throw new Exception("Name of the course should be less than 18 elements.");
                _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Description can not be empty.");
                _description = value;
            }
        }

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
