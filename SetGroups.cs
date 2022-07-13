using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SortingAlgorithm.Shuffler;

namespace SortingAlgorithm
{
    //This class is receiving the information from the file and based on the inputs it will generate numbers of groups and numbers of people per group
    //NumberOfPeoplePerGroupMax and Min reference what is the maximum number of people per group, and what are the minimum number of people per group
    //NumberOfGroupsMax and Min reference how many groups will contain the maximum number of people and how many groups will contain the minimum number of people

    public class SetGroups
    {

        public int NumberOfPeoplePerGroupMax { get; set; }
        public int NumberOfPeoplePerGroupMin { get; set; }
        public int NumberOfGroupsMax { get; set; }
        public int NumberOfGroupsMin { get; set; }
        private List<Employee> employees = new List<Employee>();
        private List<Teams> teams = new List<Teams>();
        private List<Departments> departments = new List<Departments>();
        private List<Employee> listOfCaptains = new List<Employee>();

        //The constructor will determine how many teams and departments are in the full list.
        public SetGroups(List<Employee> employeesFromOutside)
        {

            foreach (var i in employeesFromOutside)
            {
                int k = teams.FindIndex(t => t.TeamName == i.Team);
                if (k >= 0)
                {
                    teams[k].Employees.Add(i);
                }
                else
                {
                    Teams t = new Teams(i);
                    teams.Add(t);
                    k = departments.FindIndex(d => d.DepartmentName == i.Department);
                    if (k >= 0)
                        departments[k].TeamsInThisDepartment.Add(t);
                    else
                    {
                        Departments d = new Departments(i, t);
                        departments.Add(d);
                    }
                }

                if (i.IsCaptain)
                    listOfCaptains.Add(i);

            }
            employees = employeesFromOutside;

        }

        //First block will get an intial input from the user. I.E. How they want to divide it.
        public void CheckHowDivisionWillHappen(int qtyOfEmployees)
        {
            Console.WriteLine("Would you like to divide the employees by number of people per group or by number of groups?");
            Console.WriteLine("1. Number of people per group");
            Console.WriteLine("2. Number of groups");

            int i = ChoiceValidation(1, 2);

            if (i == 1)
                GetNumberOfPeoplePerGroup(qtyOfEmployees);
            else
                GetNumberOfGroups(qtyOfEmployees);
        }

        //This will activate if the user wants to set a number X of people per group. It will work if the user wants a min and a max.
        private void GetNumberOfPeoplePerGroup(int qtyOfEmployees)
        {
            Console.WriteLine("Please tell me how many people per group");

            int i = InputValidation();

            var getRemainder = qtyOfEmployees % i;

            if (getRemainder == 0)
            {
                NumberOfPeoplePerGroupMax = i;
                NumberOfPeoplePerGroupMin = 0;
                NumberOfGroupsMax = qtyOfEmployees / i;
                NumberOfGroupsMin = 0;
                return;
            }

            Console.WriteLine("Division of groups will be uneven, would you like to set this number to a maximum number of people per group or to a minimum number of people per group?");
            Console.WriteLine("1. Min");
            Console.WriteLine("2. Max");

            int k = ChoiceValidation(1, 2);

            if (k == 1)
            {
                NumberOfPeoplePerGroupMin = i;
                NumberOfPeoplePerGroupMax = i + 1;
                NumberOfGroupsMax = getRemainder;
                NumberOfGroupsMin = (qtyOfEmployees / i) - getRemainder;
            }
            else
            {
                NumberOfPeoplePerGroupMin = i - 1;
                NumberOfPeoplePerGroupMax = i;
                NumberOfGroupsMin = i - getRemainder;
                NumberOfGroupsMax = (qtyOfEmployees + i - 1) / i - (i - getRemainder);

            }

        }

        //This will activate if the user wants to set a number X of groups. It will work even if the division is uneven
        private void GetNumberOfGroups(int qtyOfEmployees)
        {

            int i;
            bool okayWithThis = false;
            int getRemainder;

            do
            {
                Console.WriteLine("Please tell me how many groups");
                i = InputValidation();
                getRemainder = qtyOfEmployees % i;
                if (getRemainder == 0)
                {
                    NumberOfGroupsMax = i;
                    NumberOfGroupsMin = 0;
                    NumberOfPeoplePerGroupMax = qtyOfEmployees / i;
                    NumberOfPeoplePerGroupMin = 0;
                    return;
                }
                Console.WriteLine("The division of groups will be uneven, are you okay with this? (Y/N)");
                if (Console.ReadLine() == "Y")
                    okayWithThis = true;
            } while (!okayWithThis);

            NumberOfGroupsMax = getRemainder;
            NumberOfGroupsMin = i - getRemainder;
            NumberOfPeoplePerGroupMax = (qtyOfEmployees / i) + 1;
            NumberOfPeoplePerGroupMin = qtyOfEmployees / i;


        }

        //Class that will validate the input from the user
        private int InputValidation()
        {
            bool read = Int32.TryParse(Console.ReadLine(), out int choice);

            while (!read)
            {
                Console.WriteLine("Please select a valid choice");
                read = Int32.TryParse(Console.ReadLine(), out choice);
            }

            return choice;
        }

        //Class that will validate if the choice is a valid one
        private int ChoiceValidation(int min, int max)
        {
            int i = InputValidation();

            while (i < min || i > max)
            {
                Console.WriteLine("Please select a valid choice");
                i = InputValidation();
            }

            return i;

        }

        //This block verify if the number of captains is equal to the number of teams and send to the according class
        private void VerifyCaptains()
        {
            if (listOfCaptains.Count == NumberOfGroupsMax + NumberOfGroupsMin)
                return;

            if (listOfCaptains.Count > NumberOfGroupsMax + NumberOfGroupsMin)
                MoreCaptainsThanTeams();
            else
                LessCaptainsThanTeams();

        }

        //This will activate if more captains were selected
        private void MoreCaptainsThanTeams()
        {
            do
            {
                Console.WriteLine(String.Format("It appears that you have selected more Group Leaders than Groups. Please select {0} leaders to remove",
                Convert.ToString(listOfCaptains.Count - NumberOfGroupsMax - NumberOfGroupsMin)));

                foreach (var loc in listOfCaptains)
                {
                    Console.WriteLine(String.Format(Convert.ToString(listOfCaptains.IndexOf(loc)) + ". " + loc.FirstName + " " + loc.LastName));
                }

                int k = ChoiceValidation(0, listOfCaptains.Count);

                employees.Find(x => x.Id == listOfCaptains[k].Id).IsCaptain = false;
                listOfCaptains.Remove(listOfCaptains[k]);

            } while (listOfCaptains.Count > (NumberOfGroupsMax + NumberOfGroupsMin));

        }

        //THis will trigger if there are more teams than captains
        private void LessCaptainsThanTeams()
        {
            do
            {
                Console.WriteLine(String.Format("It appears that you have selected more Groups than Group leaders. Please select {0} leaders to add",
                Convert.ToString(NumberOfGroupsMax + NumberOfGroupsMin - listOfCaptains.Count)));

                Console.WriteLine("Please type a name or a surname");
                var read = Console.ReadLine();


                var readInput = employees.Where(x => (x.FirstName.Contains(read, StringComparison.CurrentCultureIgnoreCase) || x.LastName.Contains(read, StringComparison.CurrentCultureIgnoreCase)) && x.IsCaptain == false).ToList();

                if (readInput.Count == 0)
                {
                    Console.WriteLine("No user found, please try again");
                    continue;
                }

                if (readInput.Count == 1)
                {
                    Console.WriteLine("User found");
                    Console.WriteLine(String.Format(Convert.ToString("1" + ". " + readInput[0].FirstName + " " + readInput[0].LastName)));

                    employees.Find(x => x.Id == readInput[0].Id).IsCaptain = true;

                    listOfCaptains.Add(employees.Find(x => x.Id == readInput[0].Id));

                    continue;
                }


                Console.WriteLine("Please select one of the matches found");

                foreach (var loc in readInput)
                {
                    Console.WriteLine(String.Format(Convert.ToString(readInput.IndexOf(loc)) + ". " + loc.FirstName + " " + loc.LastName));
                }

                int k = ChoiceValidation(0, readInput.Count);

                employees.Find(x => x.Id == readInput[k].Id).IsCaptain = true;

                listOfCaptains.Add(employees.Find(x => x.Id == readInput[k].Id));

            } while (listOfCaptains.Count < (NumberOfGroupsMax + NumberOfGroupsMin));
        }

        //This class will generate all the groups with the members of the groups.
        public List<Groups> GroupCreator()
        {

            List<Groups> allGroups = new List<Groups>();

            for (var i = 0; i < (NumberOfGroupsMax + NumberOfGroupsMin); i++)
            {
                allGroups.Add(new Groups(i + 1));
                VerifyCaptains();
                allGroups[i].Employees.Add(listOfCaptains[i]);
            }


            Shuffler.Shuffle<Groups>(allGroups);

            int counter = -1;
            var peoplePerGroup = NumberOfPeoplePerGroupMin;
            var totalCounter = listOfCaptains.Count;

            foreach (var d in departments)
            {
                foreach (var t in d.TeamsInThisDepartment)
                {
                    foreach (var x in t.Employees)
                    {

                        if (x.IsCaptain)
                            continue;

                        totalCounter++;

                        if (totalCounter == peoplePerGroup * (NumberOfGroupsMax + NumberOfGroupsMin))
                            peoplePerGroup = NumberOfPeoplePerGroupMax;

                        counter++;

                        if (counter >= allGroups.Count)
                        {
                            counter = 0;
                            Shuffler.Shuffle<Groups>(allGroups);
                        }

                        if (allGroups[counter].Employees.Any())
                        {
                            int testCounter = 0;

                            while (allGroups[counter].Employees.FindIndex(n => n.Team == x.Team) >= 0 || allGroups[counter].Employees.Count >= peoplePerGroup)
                            {
                                counter++;
                                if (counter >= allGroups.Count)
                                {
                                    counter = 0;
                                    Shuffler.Shuffle<Groups>(allGroups);
                                }

                                testCounter++;

                                if (testCounter > 2 * (NumberOfGroupsMax + NumberOfGroupsMin))
                                    break;


                            }

                        }

                        allGroups[counter].Employees.Add(x);


                    }
                }
            }

            return allGroups;
        }

    }
}