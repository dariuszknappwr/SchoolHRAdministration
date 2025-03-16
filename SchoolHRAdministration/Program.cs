using System;
using HRAdministrationAPI;
using System.Linq;
using System.IO;

namespace SchoolAdministration
{
    public enum EmployeeType
    {
        Teacher,
        HeadOfDepartment,
        DeputyHeadMaster,
        HeadMaster
    }
    class Program
    {
       delegate void LogDel(string text);

       static void Main(string[] args)
        {
            Log log = new Log();
            LogDel logTextToScreenDel, logTextToFileDel;
            logTextToScreenDel = new LogDel(log.LogTextToScreen);
            logTextToFileDel = new LogDel(log.LogTextToFile);

            LogDel multilogDel = logTextToScreenDel + logTextToFileDel;
            LogText(multilogDel, "Hello World");

            List<IEmployee> employees = new List<IEmployee>();

            SeedData(employees);

            Console.WriteLine($"Total Annual Salaries (Including bonus): {employees.Sum(e => e.Salary)}");

            Console.ReadKey();

        }

        static void LogText(LogDel logDel, string text)
        {
            logDel(text);
        }

        public class Log
        {
            public void LogTextToScreen(string text)
            {
                Console.WriteLine($"{DateTime.Now}: {text}");
            }

            public void LogTextToFile(string text)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"), true))
                {
                    sw.WriteLine($"{DateTime.Now}: {text}");
                }
            }
        }
        

        public static void SeedData(List<IEmployee> employees)
        {
            IEmployee teacher1 = EmployeeFactory.GetEmployeeInstance(EmployeeType.Teacher, 1, "Bob", "Fisher", 40000);
            employees.Add(teacher1);

            IEmployee teacher2 = EmployeeFactory.GetEmployeeInstance(EmployeeType.Teacher, 2, "Jenny", "Thomas", 40000);
            employees.Add(teacher2);

            IEmployee headOfDepartment = EmployeeFactory.GetEmployeeInstance(EmployeeType.HeadOfDepartment, 3, "Brenda", "Mullins", 50000);
            employees.Add(headOfDepartment);

            IEmployee deputyHeadMaster = EmployeeFactory.GetEmployeeInstance(EmployeeType.DeputyHeadMaster, 4, "Devlin", "brown", 60000);
            employees.Add(deputyHeadMaster);

            IEmployee headMaster = EmployeeFactory.GetEmployeeInstance(EmployeeType.HeadMaster, 5, "Damien", "Jhones", 80000);
            employees.Add(headMaster);
        }
    }

    public class Teacher: EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.02m); }
    }
    public class HeadOfDepartment: EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.03m); }
    }
    public class DeputyHeadMaster: EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.04m); }
    }
    public class HeadMaster: EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.05m); }
    }

    public static class  EmployeeFactory
    {
        public static IEmployee GetEmployeeInstance(EmployeeType employeeType, int id, string firstName, string lastName, decimal salary)
        {
            IEmployee employee = null;
            switch (employeeType)
            {
                case EmployeeType.Teacher:
                    employee = FactoryPattern<IEmployee, Teacher>.GetInstance();
                    break;
                case EmployeeType.HeadOfDepartment:
                    employee = FactoryPattern<IEmployee, HeadOfDepartment>.GetInstance();
                    break;
                case EmployeeType.DeputyHeadMaster:
                    employee = FactoryPattern<IEmployee, DeputyHeadMaster>.GetInstance();
                    break;
                case EmployeeType.HeadMaster:
                    employee = FactoryPattern<IEmployee, HeadMaster>.GetInstance();
                    break;
                default: 
                    break;
            }
            if(employee != null)
            {
                employee.Id = id;
                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.Salary = salary;
            }else
                {
                throw new NullReferenceException();
            }
            return employee;
        }
    }
}