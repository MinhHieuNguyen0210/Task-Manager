using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Diagnostics; // Cung cap  ten cac lop trong he thong, bo dem hieu xuat
using System.Management; // Cung cap quyen truy cap vao he thong quan ly(dung luong,muc use CPU,,,,)
using System.Dynamic; // Cung cap 1 lop co so chi dinh cac hanh vi thay doi trong thoi gian chay 
namespace TaskManger
{ 

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        GetProcess get = new GetProcess();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            renderProcessesOnListView();

            timer1.Start();
        }
        public void renderProcessesOnListView()
        {
            GetProcess getProcess = new GetProcess();
            
            Process[] processList = Process.GetProcesses();

            ImageList Imagelist = new ImageList();

            foreach (Process process in processList)
            {
                string status = (process.Responding == true ? "Responding" : "Not responding");
                dynamic extraProcessInfo = GetProcessExtraInformation(process.Id);

                string[] row = {
                    
                    process.Id.ToString(),
                    
                    process.ProcessName,
                    
                    status,
                   
                    extraProcessInfo.Username,
                    
                    BytesToReadableValue(process.PrivateMemorySize64),
                    
                    extraProcessInfo.Description
                };

                try
                {
                    Imagelist.Images.Add(
                        
                        process.Id.ToString(),
                        
                        Icon.ExtractAssociatedIcon(process.MainModule.FileName).ToBitmap()
                    );
                }
                catch { }

                ListViewItem item = new ListViewItem(row)
                {
                  
                    ImageIndex = Imagelist.Images.IndexOfKey(process.Id.ToString())
                };

         
                listView1.Items.Add(item);
            }

            listView1.LargeImageList = Imagelist;
            listView1.SmallImageList = Imagelist;
        }

      
        public string BytesToReadableValue(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }

        public ExpandoObject GetProcessExtraInformation(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            { 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                   // response.Username = argList[0];
                    response.Username = argList[1] + "\\" + argList[0];
                }

                if (obj["ExecutablePath"] != null)
                {
                    try
                    {
                        FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                        response.Description = info.FileDescription;
                    }
                    catch { }
                }
            }

            return response;
        }
    




private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void MetroProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void TabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e) // Thiet lap lai sau 1 khoang thoi gian 
        {
            float fcpu = performanceCounter1.NextValue();// performanceCounter la thanh phan truy cap Hieu Nang, Nextvalue tra ve gtr 
            float fram = performanceCounter2.NextValue();
            pbRAM.Value = (int)fcpu; 
            pbCPU.Value = (int)fram;
            lblRam.Text = string.Format("{0:0.00}%", fcpu); // Sau 1 khoang thoi gian thi cap nhat lai Ram va Cpu
            lblCPU.Text = string.Format("{0:0.00}%", fram);

        }

        private void Btn_EndTask_Click(object sender, EventArgs e)
        {
           Process processes = Process.GetCurrentProcess();
           
                EndTask endTask = new EndTask();
                
                endTask.KillProcess((listView1.SelectedItems[0].SubItems[0].Text));
          listView1.SelectedItems[0].Remove();
            //this.Refresh();
        }

        private void MetroProgressBar2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        public void ListView1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }
    }
}  

