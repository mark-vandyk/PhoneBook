using System;
using System.Diagnostics;
using System.IO;

namespace PhoneBook.Entities
{
    public static class DbConfig
    {

        public static string DataDirectory
        {
            get
            {
                var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                return Debugger.IsAttached ? Path.GetFullPath(Path.Combine(directoryPath, "..//..//..//..//PhoneBook.Db")) : directoryPath;
            }
        }

        public static string PhoneBookDbConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB; " +
                "AttachDbFilename=" + DataDirectory + "\\PhoneBook_Primary.mdf;" +
                " Integrated Security=True; Connect Timeout=30;";
    }
}
