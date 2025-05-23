using EduSky;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace TestEduSkyProj
{


    [TestFixture]
    public class TestEduSky
    {

        [TestClass]
        [DoNotParallelize]
        public class EduSkyTests
        {
            [TestInitialize]
            public void Init()
            {
                EduSkyProgramm.Courses.Clear();
                EduSkyProgramm.Users.Clear();
                EduSkyProgramm.CurrentUser = null;
            }
            [TestMethod]
            public void GuestCanRegisterAsStudent()
            {
                var guest = new Guest
                {
                    Login = "student123",
                    Password = "pass",
                    Name = "Іван",
                    DateOfBirth = new DateTime(2001, 1, 1),
                    GraduatedFrom = "Школа 1"
                };

                var user = guest.Register(UserRole.Student);

                Assert.IsInstanceOfType(user, typeof(RegisteredUser));
                Assert.AreEqual("Іван", user.Name);
                Assert.AreEqual("pass", user.Password);
                Assert.AreEqual("student123", user.Login);
            }

            [TestMethod]
            public void GuestCanRegisterAsTeacher()
            {
                var guest = new Guest
                {
                    Login = "teacher123",
                    Password = "teachpass",
                    Name = "Ольга",
                    DateOfBirth = new DateTime(1980, 5, 10),
                    GraduatedFrom = "Педуніверситет"
                };

                var user = guest.Register(UserRole.Teacher);

                Assert.IsInstanceOfType(user, typeof(Teacher));
                Assert.AreEqual("Ольга", user.Name);
                Assert.AreEqual("teacher123", user.Login);
            }
         
          
            [TestMethod]
            public void TeacherCanCreateCourse()
            {
                EduSkyProgramm.Courses.Clear();

                var teacher = new Teacher { Login = "teach1" };
                bool created = teacher.CreateCourse("Математика", "Курс з алгебри");

                Assert.IsTrue(created);
                Assert.AreEqual(1, EduSkyProgramm.Courses.Count);
                Assert.AreEqual("Математика", EduSkyProgramm.Courses[0].Title);
                Assert.AreEqual("teach1", EduSkyProgramm.Courses[0].TeacherLogin);
            }
            [TestMethod]
            public void TeacherCannotCreateDuplicateCourse()
            {
                var teacher = new Teacher { Login = "teach1" };
                teacher.CreateCourse("Math", "Description");
                bool result = teacher.CreateCourse("Math", "Another Desc");

                Assert.IsFalse(result);
                Assert.AreEqual(1, EduSkyProgramm.Courses.Count);
            }

            [TestMethod]
            public void CourseEnrollAndLeaveEvents()
            {
                var course = new Course();
                string? enrolledLogin = null;
                string? leftLogin = null;

                course.UserEnrolled += login => enrolledLogin = login;
                course.UserLeft += login => leftLogin = login;

                course.EnrollUser("user1");
                Assert.AreEqual("user1", enrolledLogin);

                course.LeaveCourse("user1");
                Assert.AreEqual("user1", leftLogin);
            }

            [TestMethod]
            public void TaskSubmitAnswerFiresEvent()
            {
                var task = new EduSky.Task();
                string? submittedBy = null;
                string? filePath = null;

                task.AnswerSubmitted += (login, path) =>
                {
                    submittedBy = login;
                    filePath = path;
                };

                task.SubmitAnswer("student1", "/path/answer.docx");

                Assert.AreEqual("student1", submittedBy);
                Assert.AreEqual("/path/answer.docx", filePath);
                Assert.AreEqual("/path/answer.docx", task.UserAnswers["student1"]);
            }

            [TestMethod]
            public void AuthServiceLoginWorks()
            {
                EduSky.EduSkyProgramm.Users = new List<User>
            {
                new RegisteredUser { Login = "login1", Password = "123" }
            };

                var authService = new AuthService(new DummyUserService());
                var user = authService.Login("login1", "123");

                Assert.IsNotNull(user);
                Assert.AreEqual("login1", user.Login);
            }

            [TestMethod]
            public void AuthServiceRejectsInvalidLogin()
            {
                EduSky.EduSkyProgramm.Users = new List<User>
            {
                new RegisteredUser { Login = "login1", Password = "123" }
            };

                var authService = new AuthService(new DummyUserService());
                var user = authService.Login("login1", "wrong");

                Assert.IsNull(user);
            }
            [TestMethod]
            public void TeacherCanAddTaskToOwnCourse()
            {
                EduSkyProgramm.Courses.Clear();
                var teacher = new Teacher { Login = "t1" };
                teacher.CreateCourse("Історія", "Курс з історії");

                var result = teacher.AddTaskToCourse("Історія", "Завдання 1", "Опис завдання", "/path/to/file.pdf");

                Assert.IsTrue(result);
                var course = EduSkyProgramm.Courses.First(c => c.Title == "Історія");
                Assert.AreEqual(1, course.Tasks.Count);
                Assert.AreEqual("Завдання 1", course.Tasks[0].Title);
            }

            [TestMethod]
            public void TeacherCannotAddTaskToForeignCourse()
            {
                EduSkyProgramm.Courses.Clear();
                EduSkyProgramm.Courses.Add(new Course { Title = "Музика", TeacherLogin = "other_teacher" });

                var teacher = new Teacher { Login = "t1" };
                var result = teacher.AddTaskToCourse("Музика", "Завдання 1", "Опис", "/file.doc");

                Assert.IsFalse(result);
            }
            [TestMethod]
            public void StudentCanEnrollToCourse()
            {
                var course = new Course { Title = "Інформатика" };
                course.EnrollUser("student1");

                Assert.IsTrue(course.EnrolledUserLogins.Contains("student1"));
            }

            [TestMethod]
            public void StudentCannotEnrollTwice()
            {
                var course = new Course { Title = "Інформатика", EnrolledUserLogins = new List<string> { "student1" } };
                course.EnrollUser("student1");

                Assert.AreEqual(1, course.EnrolledUserLogins.Count);
            }
            [TestMethod]
            public void JsonDataServiceCanSaveAndLoad()
            {
                string tempPath = Path.GetTempFileName();
                var service = new JsonDataService<Course>(tempPath);

                var courseList = new List<Course>
    {
        new Course { Title = "Біологія", Description = "Курс біології", TeacherLogin = "t1" }
    };

                service.Save(courseList);
                var loaded = service.Load();

                Assert.AreEqual(1, loaded.Count);
                Assert.AreEqual("Біологія", loaded[0].Title);

                File.Delete(tempPath);
            }
            [TestMethod]
            public void UserRoleReturnsCorrectRole()
            {
                var student = new RegisteredUser();
                var teacher = new Teacher();
                var guest = new Guest();

                Assert.AreEqual("Student", student.GetRole());
                Assert.AreEqual("Teacher", teacher.GetRole());
                Assert.AreEqual("Guest", guest.GetRole());
            }

        }
        public class DummyUserService : IDataService<User>
        {
            public List<User> Load() => new List<User>();
            public void Save(List<User> items) { }
        }
    }
}
