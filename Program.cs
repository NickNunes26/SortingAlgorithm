using CsvHelper;
using System.Globalization;
using SortingAlgorithm;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Windows.Forms;
using static SortingAlgorithm.HandleIncomingData;
using System.Threading;
using System.Runtime.InteropServices;

//There are a few distinctions that need to be clarified for this code:
//Departments = Sales, Devs and Backoffice.
//Teams = Box Web, Mobile, Spain, Sweden, HR etc...
//A Department contains multiple Teams
//This code assumes that each Team is unique to a Department.

List<Employee> employees = GetList();

//This block of code will set how many groups will we have. It check how many people per group and how many groups are necessary.
SetGroups setGroups = new SetGroups(employees);

do
{
    setGroups.CheckHowDivisionWillHappen(employees.Count);
    Console.WriteLine(String.Format("You will have {0} groups with {1} people and {2} groups with {3} people",
        setGroups.NumberOfGroupsMax, setGroups.NumberOfPeoplePerGroupMax, setGroups.NumberOfGroupsMin, setGroups.NumberOfPeoplePerGroupMin));
    Console.WriteLine("Are you okay with this? Y/N");
    if (Console.ReadLine() == "Y")
        break;

} while (true);


//This next block of code will create the groups and assign "seats" i.e. how many devs per group, how many sales per group, etc...
List<Groups> groups = setGroups.GroupCreator();

WriteCSV(groups);