using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; // for file access
using System.Security.AccessControl; // for DirectorySecurity
using System.Security.Principal; // for windowsidentity

// a test class for learninghow to manipulate files in c# & .Net
//  written 10/6/17 as a precursor for palying with git, git lfs, & how it interacts with atlassians sourcetree.

    // for commit.
namespace fileoperations
{
    class Program
    {
        private static void AllowAccessToDirectory(string directoryPath)
        {
            try
            {
                DirectorySecurity ds = Directory.GetAccessControl(directoryPath);
                string userName = WindowsIdentity.GetCurrent().Name;

                Console.WriteLine("username (" + userName + ")");
                // remove the deny access rule
                FileSystemAccessRule fsarDeny = new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Deny);
                ds.RemoveAccessRule(fsarDeny);

                // add an allow rule
                FileSystemAccessRule fsarAllow = new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Allow);
                ds.AddAccessRule(fsarAllow);

                Directory.SetAccessControl(directoryPath, ds);
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception trying to give access to path (" + directoryPath + ") exception = (" + e.Message + ")");
            }
        }



        static void Main(string[] args)
        {

            const int numberOfLines = 1024 * 1024 * 2;
            string fullPath = Directory.GetCurrentDirectory();
            //Console.WriteLine("Adding full file control to directory (" + fullPath + ")");
            //AllowAccessToDirectory(fullPath);

            string fullFilePath = Path.Combine(fullPath, "testfile.txt" );
            Console.WriteLine("writing to file (" + fullFilePath + ")");
            string contents = "contents";
            try
            {
                if (!File.Exists(fullFilePath))
                {
                    StreamWriter newFileStream = File.CreateText(fullFilePath);
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        newFileStream.WriteLine("line (" + i + ")" + contents);
                    }
                    newFileStream.Close();
                }
                else
                {
                    StreamWriter oldFileStream = File.AppendText(fullFilePath);
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        oldFileStream.WriteLine("line (" + i + ")" + contents);
                    }
                    oldFileStream.Close();
                }
            }
            catch (UnauthorizedAccessException ue)
            {
                Console.WriteLine("Unauthorised Access Exception caught writing a file. exception = (" + ue.Message + ")");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught writing a file. exception = (" + e.Message + ")");
            }

            Console.WriteLine("finished writing to file (" + fullFilePath + ")");
        }
    }
}
