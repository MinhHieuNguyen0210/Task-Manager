using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Cung cap  ten cac lop trong he thong, bo dem hieu xuat
using System.Management; // Cung cap quyen truy cap vao he thong quan ly(dung luong,muc use CPU,,,,)
using System.Dynamic; // Cung cap 1 lop co so chi dinh cac hanh vi thay doi trong thoi gian chay 
using System.Windows.Forms;
using System.Drawing;
namespace TaskManger
{

    public class GetProcess
    {

        private string processId;
        private string processName;
        private string processUserName;
        private string processDeciption;
        private string processMemory;
        private string processStatus;


        public string Id
        {
            get { return processId; }
            set { processId = value; }
        }
        public string Name
        {
            get { return processName; }
            set { processName = value; }
        }
        public string Status
        {
            get
            {
               
                if(processStatus == "True")
                {
                    return "Responding";
                }
                else
                {
                    return "Not Responding";
                }

               
            }
            set { processStatus = value; }
        }

        public string UserName
        {
            get
            {
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                 processUserName = "Unknown";
                
                foreach (ManagementObject obj in processList)
                {

                    string[] argList = new string[] { string.Empty, string.Empty };
                    int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                    if (returnVal == 0)
                    {

                        processUserName = argList[0];

                        return processUserName;

                    }
                }
                return processUserName;
            }
            set { processUserName = value; }

        }
        public string Depciption
        {
            get
            {
                string query = "Select * From Win32_Process Where ProcessID = " + processId;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection processList = searcher.Get();

                processDeciption = "";
               
                foreach (ManagementObject obj in processList)
                {
                    if (obj["ExecutablePath"] != null)
                    {
                        try
                        {
                            FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                             processDeciption = info.FileDescription;
                            return processDeciption;
                        }
                        catch { }
                    }
                }
                return processDeciption;
            }
            set { processDeciption = value; }
        }
        public string Memory
        {
            get
            {
                long number =Convert.ToInt64 (processMemory);
                List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

                for (int i = 0; i < suffixes.Count; i++)
                {
                    long temp = number / (int)Math.Pow(1024, i + 1);

                    if (temp == 0)
                    {
                        return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                    }
                }

                return processMemory;
            
            }
            set { processMemory = value; }
        }

    }

}




