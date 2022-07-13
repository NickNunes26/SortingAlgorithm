using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Dialogs;



namespace SortingAlgorithm
{

    //This class will handle all the incoming data
    public static class HandleIncomingData
    {

        //This chunk of code Opens the .csv file. Maybe try to get a working Excel next?
        [STAThread]
        public static string OpenFile()
        {
            var filePath = string.Empty;

            Thread t = new Thread((ThreadStart)(() =>
            {
                using (var openFileDialog = new CommonOpenFileDialog())
                {
                    openFileDialog.AllowNonFileSystemItems = true;
                    openFileDialog.Multiselect = false;
                    openFileDialog.IsFolderPicker = false;
                    openFileDialog.Title = "Select file";

                    if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                    }
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return filePath;
        }

        //This part of the code will make a list of Employees from the file opened. It uses the previous function.
        public static List<Employee> GetList()
        {
            var list = new List<Employee>();

            using (var stream = File.OpenRead(OpenFile()))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                list = csv.GetRecords<Employee>().ToList();
            }

            return list;
        }

        public static string SelectFolder()
        {
            var filePath = string.Empty;

            Thread t = new Thread((ThreadStart)(() =>
            {
                using (var openFolderDialog = new CommonOpenFileDialog())
                {
                    openFolderDialog.AllowNonFileSystemItems = true;
                    openFolderDialog.Multiselect = false;
                    openFolderDialog.IsFolderPicker = true;
                    openFolderDialog.Title = "Select folder";

                    if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        //Get the path of specified file
                        filePath = openFolderDialog.FileName;

                    }
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return filePath;

        }

        public static void WriteCSV(List<Groups> groups)
        {

            string path = SelectFolder();

            var k = 0;

            foreach (var g in groups)
            {
                List<ReadyToImportEmployees> importList = new List<ReadyToImportEmployees>();

                foreach (var d in g.Employees)
                    importList.Add(new ReadyToImportEmployees(d, g));


                using (var writer = new StreamWriter(path + "\\Group_" + g.GroupNumber + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(importList);
                }
            }

        }

    }
}