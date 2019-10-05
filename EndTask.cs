using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Cung cap  ten cac lop trong he thong, bo dem hieu xuat
using System.Management; // Cung cap quyen truy cap vao he thong quan ly(dung luong,muc use CPU,,,,)
using System.Dynamic; // Cung cap 1 lop co so chi dinh cac hanh vi thay doi trong thoi gian chay 
using System.Drawing;
namespace TaskManger
{

    public class EndTask
    {
       
        public void KillProcess(string id)
        {
            Process[] process = Process.GetProcesses(); // Lay cac tien trinh 
            foreach (Process pro in process)
            {
                if ( id == pro.ProcessName) // Chon item tai cot Name cua Process va dong ung dung 
                {
                    pro.Kill(); // dung de dong ung dung
                   // pro.Id.ToString().Remove();
                    break;
                }
            }
        }
    }
}
