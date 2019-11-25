using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Diagnostics; // Cung cap  ten cac lop trong he thong, bo dem hieu xuat
using System.Data;
using System.Data.SqlClient;

namespace TaskManger
{
    public partial class Form1 : MetroForm
    {
       
        List<GetProcess> programProcesses = new List<GetProcess>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            renderProcessesOnListView();
           
        }
        public void renderProcessesOnListView()
        {
            
            Process[] proce = Process.GetProcesses();


            GetProcess processss = new GetProcess();
            
            foreach (Process pre in proce)
            {
                processss = new GetProcess()
                {
                    Name = pre.ProcessName.ToString(),
                    Id = pre.Id.ToString(),
                    Status = pre.Responding.ToString(),
                    Memory = pre.PagedMemorySize64.ToString(),
                    UserName = pre.Id.ToString(),
                    Depciption = pre.Id.ToString()
                };
              
                programProcesses.Add(processss);

            }

            foreach (GetProcess tien in programProcesses)
            {
                
                ListViewItem newitem = new ListViewItem() { Text = tien.Id };

                newitem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tien.Name });

                string trangthai = (tien.Status);             
                newitem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = trangthai });

                string tennguon =(tien.UserName);
                newitem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = tennguon });

                string bonho = (tien.Memory);
                newitem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = bonho });

                string mota =(tien.Depciption);
                newitem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = mota });

                listView1.Items.Add(newitem);
                
            }
        
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

           
             this.Refresh();

        }

        private void Btn_EndTask_Click(object sender, EventArgs e)
        {

            EndTask.KillProcess((listView1.SelectedItems[0].SubItems[0].Text));
            listView1.SelectedItems[0].Remove();
            

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




