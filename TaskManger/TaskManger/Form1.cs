using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Diagnostics; // Cap cap ten cac lop trong he thong, bo dem hieu xuat
using System.Management; // Cung cap quyen truy cap vao he thong quan ly(dung luong,muc use CPU,,,,)
using System.Dynamic; // Cung cap 1 lop co so chi dinh cac hanh vi thay doi trong thoi gian chay 

namespace TaskManger
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            renderProcessesOnListView();
            timer1.Start();
        }

        public void renderProcessesOnListView() // Ham lay danh sach cac process 
        {
            // Tao Mang de luu tru cac processes
            Process[] processList = Process.GetProcesses();

            // Tao hinh anh cua moi process
            ImageList Imagelist = new ImageList();

            // Tao vong lap cua moi Process va Show thong tin cua moi process
            foreach (Process process in processList)
            {
                // Trang thai cua moi Process, dang hoat dong la Running va nguoc lai la Suspended
                string status = (process.Responding == true ? "Running" : "Suspended");

                // Ham truy xuat doi tuong cua Process gom UserName va Desciption
                dynamic extraProcessInfo = GetProcessExtraInformation(process.Id);

                // Tao mang thong tin luu tru cac Process tren man hinh
                string[] row = {
                    // 1 Process name
                    process.ProcessName,
                    // 2 Process ID
                    process.Id.ToString(),
                    // 3 Process status
                    status,
                    // 4 Ten Username luu tru cac Process
                    extraProcessInfo.Username,
                    // 5 Memory 
                    BytesToReadableValue(process.PrivateMemorySize64) ,
                    // 6 Phan mo ta cua cac Process
                    extraProcessInfo.Description
                };


              // Neu co Icon thi Dat Icon cho moi Process, khong thi thoi
                try
                {
                    Imagelist.Images.Add(
                        // Xac dinh Id cua moi Process va them Icon vao
                        process.Id.ToString(),
                        // Them Icon vao danh sach
                        Icon.ExtractAssociatedIcon(process.MainModule.FileName).ToBitmap()
                    );
                }
                catch { }

                //  Tao 1 muc moi de them vao cac danh sach cua Process
                ListViewItem item = new ListViewItem(row)
               {
                    // Dat Icon cua Process giong lan thu Try - Catch truoc do
                    ImageIndex = Imagelist.Images.IndexOfKey(process.Id.ToString())
               };

                // Them Item vao
                listView1.Items.Add(item);
            }
                // Dat lai hinh anh tao truoc do
            listView1.LargeImageList = Imagelist;
            listView1.SmallImageList = Imagelist;
        }

        
        public string BytesToReadableValue(long number) // lay gia tri bo nho cua Tung Process
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

        public ExpandoObject GetProcessExtraInformation(int processId) // lay thong tin Id va Desciption tu Process
        {
            //ExpandoObject : Đại diện cho một đối tượng mà các thành viên có thể được thêm và xóa động khi chạy.
            // Su dung Truy Van query den Win_32
            string query = "Select * From Win32_Process Where ProcessID = " + processId;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            // dung de liet ke tat ca o dia,cac ung dung dang chay va dung,,,
            ManagementObjectCollection processList = searcher.Get();
            //liet ke cac doi tuong quan ly thuoc loai duoc chi dinh thong qua ManagementClass

            // Tao 1 so doi tuong dong de luu tru 1 so thuoc tinh tren no
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Truy xuat thong tin ten  UserName
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));//GetOwner la lay ten user va domain trong qtr dang chay win32
                if (returnVal == 0) // tra ve 0 la truy cap thanh cong 
                {
                    // return UserName 
                   // response.Username = argList[1];

                    // Ngoai ra minh co the lay ten cua (PC\Username)
                    response.Username = argList[1] + "\\" + argList[0];
                }

                // Truy xuat Desciption (mo ta) cua Process neu co
                if (obj["ExecutablePath"] != null) // ExecutablePath : Chi dan duong den tep thuc thi cua chuong trinh
                {
                    try
                    {
                        FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                        //  FileVersionInfo : Cung cap thong tin phien ban co 1 tep vat ly tren dia
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
            // 
            float fcpu = performanceCounter1.NextValue();// performanceCounter la thanh phan truy cap Hieu Nang, Nextvalue tra ve gtr 
            float fram = performanceCounter2.NextValue();
            pbRAM.Value = (int)fcpu; // 
            pbCPU.Value = (int)fram;
            lblRam.Text = string.Format("{0:0.00}%", fcpu); // Sau 1 khoang thoi gian thi cap nhat lai Ram va Cpu
            lblCPU.Text = string.Format("{0:0.00}%", fram);
        }

        private void Btn_EndTask_Click(object sender, EventArgs e)
        {
            Process[] process = Process.GetProcesses(); // Lay cac tien trinh 
            foreach (Process pro in process)
            {
                if (listView1.SelectedItems[0].SubItems[0].Text == pro.ProcessName) // Chon item tai cot Name cua Process va dong ung dung 
                {
                    pro.Kill(); // dung de dong ung dung
                    break;
                }
            }
        }
    }
}  

