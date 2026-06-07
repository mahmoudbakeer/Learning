namespace CompleteCrud.Entities
{
    public class StudentRepo
    {
        private static List<Student> Students = new List<Student>()
        {
            new Student(1, "Mahmoud", "Hamed"),
            new Student(2, "Samer", "Khalid"),
            new Student(3, "Hani", "Zohir"),
            new Student(4, "Sami", "Habab"),
        };

        public static List<Student> GetStudents() => Students;

        public static Student GetStudent(int id)
        {
            var student = Students.FirstOrDefault(st => st.Id == id);
            return student;
        }

        public static void AddStudent(Student NewStudent)
        {
            if (NewStudent is null)
                return;
            else if (Students.Any(predicate: (student) => student.Id == NewStudent.Id))
                return;
            else
            {
                Students.Add(NewStudent);
                return;
            }
        }

        public static bool UpdateStudent(Student NewStudent)
        {
            if (NewStudent is null)
                return false;
            else if (!Students.Any(predicate: (student) => student.Id == NewStudent.Id))
                return false;
            else
            {
                var UpdatedStudent = Students.FirstOrDefault(student =>
                    student.Id == NewStudent.Id
                );

                UpdatedStudent.FirstName = NewStudent.FirstName;
                UpdatedStudent.LastName = NewStudent.LastName;
                return true;
            }
        }

        public static bool DeleteStudent(int id)
        {
            if (!Students.Any(predicate: (student) => student.Id == id))
                return false;
            else
            {
                var UpdatedStudent = Students.FirstOrDefault(student => student.Id == id);
                if (UpdatedStudent is null)
                    return false;
                Students.Remove(UpdatedStudent);
                return true;
            }
        }
    }
}
