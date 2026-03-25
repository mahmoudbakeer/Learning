using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_Events_Exercise01
{
    public static class GradingSystem
    {

        public static void ShowStudentsPerformance(List<clsStudent> listStudents,
            Func<List<int>, double> calculateGradesResult,
            Predicate<double> CheckIfPassed,
            Action<clsStudent, double, bool> DisplayPerformance)
        {
            foreach (clsStudent student in listStudents)
            {
                double res = calculateGradesResult(student.Grades);
                bool isPassed = CheckIfPassed(res);
                DisplayPerformance.Invoke(student, res, isPassed);
            }
        }
    }
}
