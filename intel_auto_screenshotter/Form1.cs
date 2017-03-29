using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Media.Imaging;
using System.IO;


namespace intel_auto_screenshotter
{
    public partial class Form1 : Form
    {
        

        [SecurityPermission(SecurityAction.Demand,
    Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;
            if ((m.Msg == WM_NCHITTEST) &&
                (m.Result.ToInt32() == HTCLIENT))
            {
                m.Result = (IntPtr)HTCAPTION;
            }
        }
        //varialbes
        int intervalmin;
        string pass;
        int count = 1;
        int hour=0,minute=0;
        string errm;
        int butime=0;
        DateTime dtNow= DateTime.Now;
        System.Windows.Forms.Timer timer;
        string[] ipass = new string[9999];
        int month, day, h, m;
        List<string> imageh = new List<string>();

        public Form1()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            
            month = dtNow.Month;
            day = dtNow.Day;
            h = dtNow.Hour;
            m = dtNow.Minute;
            Console.WriteLine("start timer");
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("exit clicked");
            //exit application
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("CONFIRM button clicked");
            errm = "";
            if (interval.Text == "")
            {
                intervalmin = 0;
            }else {
                intervalmin = int.Parse(interval.Text);
            }

            if (intervalmin > 60)
            {
                Console.WriteLine("get error for over 60");
                errm = "ERROR \r\nPlease type number of interval less than 60";
                err.Text = errm;
            }
            else if (intervalmin < 1)
            {
                Console.WriteLine("get error for less then 1");
                errm = "ERROR \r\nPlease type number of interval more than 0";
                err.Text = errm;
            }
            if (refe.Text == "")
            {
                if (errm != "") errm = errm+"\r\nPlease select the folder to save image";
                else errm= "ERROR\r\nPlease select the folder to save image";
                err.Text = errm;
            }
            else if (intervalmin > 0 && intervalmin < 61)
            {
                if (intervalmin == 60) hour = 1;
                else minute = intervalmin;
                
                err.Text = errm;
                Console.WriteLine("info");
                Console.WriteLine("interval : " + intervalmin);
                Console.WriteLine("folder pass : " + pass);
                TopMost = true;
                //hide forms
                text1.Hide();
                interval.Hide();
                err.Hide();
                label1.Hide();
                button3.Hide();
                refe.Hide();
                button1.Hide();
                time.Show();
                stop.Show();
                //screen_shot();
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(screen_shot);
                timer.Interval = intervalmin*60000;
                timer.Enabled = true; // timer.Start()と同じ
                //record start date
                month = dtNow.Month;
                day = dtNow.Day;
                h = dtNow.Hour;
                m = dtNow.Minute;
            }  
        }
        private void screen_shot(object sender, EventArgs e)
        {
            Console.WriteLine("running ss sys");
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gr = Graphics.FromImage(bm);
            //getting date value
            dtNow = DateTime.Now;
            int m = dtNow.Month;
            int d = dtNow.Day;
            int h = dtNow.Hour;
            int min = dtNow.Minute;
            gr.CopyFromScreen(new Point(0, 0), new Point(0, 0), bm.Size);
            bm.Save(pass + "\\" + m.ToString("00") + "_" + d.ToString("00") + "_" + h.ToString("00") + "_" + min.ToString("00") + ".png", System.Drawing.Imaging.ImageFormat.Png);
            imageh.Add( pass + "\\" + m.ToString("00") + "_" + d.ToString("00") + "_" + h.ToString("00") + "_" + min.ToString("00") + ".png");
            //update web site
            SendKeys.SendWait("{F5}");
            SendKeys.Flush();  
            bm.Dispose();
            gr.Dispose();
            Console.WriteLine("take picture : " + count);
            count++;
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("click reference");
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = "Select the directry to save the image";
            folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                refe.Text=folderBrowserDialog1.SelectedPath;
                pass = refe.Text;
            }
            folderBrowserDialog1.Dispose();
        }

        private void text1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now;
            time.Text = datetime.ToShortDateString()+" "+ datetime.ToShortTimeString();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            Console.WriteLine("click stop timer");
            TopMost = false;
            text1.Show();
            interval.Show();
            err.Show();
            label1.Show();
            button3.Show();
            refe.Show();
            button1.Show();
            time.Hide();
            stop.Hide();
            timer2.Stop();
            timer.Dispose();
            gif_anime(pass,ipass);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime datetime_now = DateTime.Now;
            int m2 = datetime_now.Minute+minute;
            int h2 = datetime_now.Hour + hour;
            DateTime datetime_set = new DateTime(datetime_now.Year, datetime_now.Month, datetime_now.Day, h2, m2-1,30);
            int time = 0;
            time = (((datetime_set.Hour - datetime_now.Hour) * 3600) + ((datetime_set.Minute - datetime_now.Minute) * 60) + (datetime_set.Second - datetime_now.Second));
            if (butime != time)
            {
                Console.WriteLine("time left : " + time);
                butime = time;
            }
            if (datetime_now.ToLongTimeString() == datetime_set.ToLongTimeString())
            {
                Console.WriteLine("stopping timer 2");
                timer2.Stop();
            }
        }
        private void gif_anime(string savePath, string[] image) {
            string animepass = savePath + "//anime_" + month.ToString("00") + "_" + day.ToString("00") + "_" + h.ToString("00") + "_" + m.ToString("00") + ".gif";
            Boolean err = false;
            //open write files

            FileStream outputFileStrm = new FileStream(animepass,
                FileMode.Create, FileAccess.Write, FileShare.None);
            try
            {
                //GifBitmapEncoderを作成する
                Console.WriteLine("start gif sys");
                GifBitmapEncoder encoder = new GifBitmapEncoder();
                Console.WriteLine("array num : " + imageh.Count);
                string[] imageFiles = new string[imageh.Count];
                //copy
                for (int c1 = 0; c1 < imageh.Count; c1++)
                {
                    imageFiles[c1] = imageh[c1];
                }
                
                    foreach (string f in imageFiles)
                    {

                        //画像ファイルからBitmapFrameを作成する
                        BitmapFrame bmpFrame =
                            BitmapFrame.Create(new Uri(f, UriKind.RelativeOrAbsolute));
                        //フレームに追加する
                        encoder.Frames.Add(bmpFrame);
                    }


                //save
                encoder.Save(outputFileStrm);
                //close
                outputFileStrm.Close();

                Array.Clear(ipass, 0, 9999);
                imageh.Clear();
            }
            catch (Exception e)
            {
                outputFileStrm.Close();
                Console.WriteLine("getting some error");
                Console.WriteLine(e);
                err = true;
            }
            if (err == true) {
                
                File.Delete(animepass);
            }
        }
    }
}
