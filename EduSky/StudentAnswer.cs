using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public class StudentAnswer
    {
        public string StudentLogin { get; set; }
        public string FilePath { get; set; }

        private int? _grade;
        public int? Grade
        {
            get => _grade;
            set
            {
                if (value is not null && (value < 1 || value > 10))
                    throw new Exception("Оцінка повинна бути від 1 до 10 балів.");
                _grade = value;
            }
        }
    }
}
