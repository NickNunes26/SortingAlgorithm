using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;


namespace SortingAlgorithm
{

    //Class that will receive the list of all employees
    public class Employee
    {
        [Name("id")]
        public int Id { get; set; }
        [Name("FirstName")]
        public string FirstName { get; set; }
        [Name("LastName")]
        public string LastName { get; set; }
        [Name("Email")]
        public string Email { get; set; }
        [Name("Team")]
        public string Team { get; set; }
        [Name("Department")]
        public string Department { get; set; }
        [Name("JoinDate")]
        public DateTime JoinDate { get; set; }
        [Name("IsCaptain")]
        public bool IsCaptain { get; set; }
    }

    //Class with groups
    public class Groups
    {
        public int GroupNumber { get; set; }
        public List<Employee> Employees { get; set; }
        public Dictionary<string, int> DepartmentsInThisGroup { get; set; }
        public List<string> TeamsInThisGroup { get; set; }

        public Groups(int groupNumber)
        {
            GroupNumber = groupNumber;
            Employees = new List<Employee>();
            TeamsInThisGroup = new List<string>();
            DepartmentsInThisGroup = new Dictionary<string, int>();

        }

    }

    public class Teams
    {
        public string TeamName { get; set; }
        public List<Employee> Employees { get; set; }

        public Teams(Employee firstEmployee)
        {
            TeamName = firstEmployee.Team;
            Employees = new List<Employee>();
            Employees.Add(firstEmployee);
        }
    }

    public class Departments
    {
        public string DepartmentName { get; set; }
        public List<Teams> TeamsInThisDepartment { get; set; }
        public int MinNumberOfPeoplePerGroup { get; set; }
        public int LeftOverMembersToBeDistributed { get; set; }


        public Departments(Employee firstEmployee, Teams firstTeam)
        {
            DepartmentName = firstEmployee.Department;
            TeamsInThisDepartment = new List<Teams>();
            TeamsInThisDepartment.Add(firstTeam);
        }

    }

    public class ReadyToImportEmployees
    {
        public string Email { get; set; }
        [Name("First Name")]
        public string FirstName { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Team { get; set; }
        public bool Captain { get; set; }

        public ReadyToImportEmployees(Employee employee, Groups group)
        {
            Email = employee.Email;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Company = "Group " + Convert.ToString(group.GroupNumber);
            Department = employee.Department;
            Team = employee.Team;
            Captain = employee.IsCaptain;


        }

    }

}