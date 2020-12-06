using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace TempGetGUI_Raspberry
{

    public partial class TempGet : Form
    {
        LonHandAPI oAPI = new LonHandAPI();
        string LonHandIP = "";
        double temp = -274;
        bool systemstate;
        DateTime BoilerOnLastMail = DateTime.MinValue;
        int query_errors = 0;
        int query_success = 0;

        Stopwatch Cycle_Counter = new Stopwatch();
        Stopwatch Odometer = new Stopwatch();
        Timer t3 = null;



        public TempGet()
        {
            InitializeComponent();
            this.lbl_current_duration.Text = Int2Clock(0);
            this.lbl_total_duration.Text = Int2Clock(0);
            XmlDocument db = new XmlDocument();
            db.Load("db.xml");
            LonHandIP = db.SelectSingleNode("//@LonHandIP").Value.ToString();

            Timer t1 = new Timer();
            t1.Tick += RefreshTemp_t1;
            t1.Interval = 60000;
            t1.Start();

            Timer t2 = new Timer();
            t2.Tick += RefreshButtonState_t2;
            t2.Interval = 20000;
            t2.Start();

            t3 = new Timer();
            t3.Tick += UpdateLabels_t3;
            t3.Interval = 100;

            Timer t4 = new Timer();
            t4.Tick += CheckIfMailNeeded_t4;
            t4.Interval = 500;
            t4.Start();



            initiate_device();
            RefreshTemp();
        }


        private void initiate_device()
        {
            systemstate = false;
            try
            {
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "/home/pi/Art/therm_init";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                systemstate = true;
                this.label2.ForeColor = Color.Green;

            }
            catch (Exception e1)
            {
                MessageBox.Show("Initializing of device failed.." + "\n\n" + "The error is: " + "\n\n" + e1);
                systemstate = false;
            }
        }



        private void RefreshTemp()
        {
            if (systemstate)
            {
                try
                {
                    DirectoryInfo devicesDir = new DirectoryInfo("/sys/bus/w1/devices");
                    foreach (var deviceDir in devicesDir.EnumerateDirectories("28*"))
                    {
                        StreamReader srTempTextFile = new StreamReader(deviceDir.GetFiles("w1_slave").FirstOrDefault().FullName);
                        string w1slavetext = srTempTextFile.ReadToEnd();
                        srTempTextFile.Close();
                        srTempTextFile.Dispose();
                        string temptext = w1slavetext.Split(new string[] { "t=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        temp = double.Parse(temptext) / 1000;
                        int tempint;
                        tempint = (int)temp;
                        string tempstring = "" + tempint;
                        this.lbl_Temp.Text = tempstring;

                        if (tempint > 44)
                        {
                            this.lbl_Temp.ForeColor = Color.Red;
                            this.label1.ForeColor = Color.Red;
                        }
                        else if (tempint < 45 && tempint > 29)
                        {
                            this.lbl_Temp.ForeColor = Color.Orange;
                            this.label1.ForeColor = Color.Orange;
                        }

                        else if (tempint < 30)
                        {
                            this.lbl_Temp.ForeColor = Color.Blue;
                            this.label1.ForeColor = Color.Blue;
                        }

                    }

                }

                catch (Exception e1)
                {
                    MessageBox.Show("Error occured! The error is : " + "\n\n" + e1);
                    this.lbl_Temp.Text = ("err");
                    this.lbl_Temp.ForeColor = Color.Red;
                }
            }
            else
            {
                MessageBox.Show("Temp Sensor Failure");

                this.lbl_Temp.Text = ("err");
                this.lbl_Temp.ForeColor = Color.Red;
            }
        }

        private void UpdateLabels_t3(object sender, EventArgs e)

        {
            UpdateLabels();
        }

        private void UpdateLabels()

        {
            this.lbl_current_duration.Text = Int2Clock((int)Cycle_Counter.Elapsed.TotalSeconds);
            this.lbl_total_duration.Text = Int2Clock((int)Odometer.Elapsed.TotalSeconds);
            double res_total = Math.Round(Odometer.Elapsed.TotalSeconds * 0.000416, 3);
            double res_current = Math.Round(Cycle_Counter.Elapsed.TotalSeconds * 0.000416, 3);
            this.lbl_total_cost.Text = ("חש " + res_total).ToString();
            this.lbl_current_cost.Text = ("חש " + res_current).ToString();
            Application.DoEvents();
        }


        private void RefreshButtonState_t2(object sender, EventArgs e)
        {
            Status_UpdateButton(LonHandIP);
        }


        private void RefreshTemp_t1(object sender, EventArgs e)
        {
            RefreshTemp();
        }

        private void CheckIfMailNeeded_t4(object sender, EventArgs e)
        {
            if (Cycle_Counter.Elapsed.TotalMinutes > 60)
            {
                if ((DateTime.Now - BoilerOnLastMail).TotalMinutes > 20)
                {
                    if (send_email("Boiler is ON for: " + Int2Clock((int)Cycle_Counter.Elapsed.TotalSeconds)))
                        BoilerOnLastMail = DateTime.Now;
                }
                else
                { }
            }
            else
            { }
        }


        private bool send_email(string body)
        {
            bool res = false;
            var fromAddress = new MailAddress("a******om@gmail.com", "Artiom");
            var toAddress = new MailAddress("ar*******le@hotmail.com", "ART");
            const string fromPassword = "*********";
            string subject = "Boiler System Notification";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtp.Send(message);
                    res = true;
                }
                catch (Exception)
                {
                }

           return res;
        }




        private void Status_UpdateButton(string IP)
        {
            int ButtonStatus = oAPI.Device_Status_Get(IP);
            if (ButtonStatus == 1)
            {
                myButton1.BackgroundImage = Resource1.power_on;
                query_success++;
                myButton1.BackColor = Color.White;

                if (!Cycle_Counter.IsRunning)
                {
                    Cycle_Counter.Start();
                    Odometer.Start();
                    t3.Start();
                }

            }
            else if (ButtonStatus == 0)
            {
                myButton1.BackgroundImage = Resource1.power_off;
                query_success++;
                myButton1.BackColor = Color.White;
                Cycle_Counter.Reset();
                Odometer.Stop();
                t3.Stop();
            }
            else

            {
                query_errors++;
                double res = Math.Round(((double)query_errors / ((double)query_success + (double)query_errors) * 100), 2);
                this.label2.Text = "Errors:" + query_errors + "(" + res + "%)";
                this.label2.ForeColor = Color.Red;

            }

        }


        private string Int2Clock(int Seconds)

        {
            var timespan = TimeSpan.FromSeconds(Seconds);
            return timespan.ToString(@"hh\:mm\:ss");
        }



        private void myButton1_Click(object sender, EventArgs e)

        {
            myButton1.BackColor = Color.Yellow;

            int DeviceStatus = oAPI.Device_Status_Get(LonHandIP);
            if (DeviceStatus == 1)
            {
                oAPI.Device_Status_Set(LonHandIP, false);
                query_success++;
            }
            else if (DeviceStatus == 0)
            {
                oAPI.Device_Status_Set(LonHandIP, true);
                query_success++;
            }

            else
            {
                query_errors++;
                double res = Math.Round(((double)query_errors / ((double)query_success + (double)query_errors) * 100), 2);
                this.label2.Text = "Errors:" + query_errors + "(" + res + "%)";
                this.label2.ForeColor = Color.Red;
            }

            Status_UpdateButton(LonHandIP);
            myButton1.BackColor = Color.White;
        }



        private void TempGet_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Odometer.Reset();
            query_success = 0;
            query_errors = 0;
            this.label2.Text = "No Errors";
            this.label2.ForeColor = Color.Green;
            UpdateLabels();
        }
  
    }
}
