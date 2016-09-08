using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Net.Http;
using System.Windows.Forms;

namespace TelemetryUI_v1._0._1
{
    public partial class initUI : Form
    {
        public mainUI mainUI = new mainUI();
        SerialPort arduino;

        public initUI()
        {
            InitializeComponent();
            this.comboBox1.Items.AddRange(SerialPort.GetPortNames());
            StartButton.Enabled = false;
        }


        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            mainUI.Close();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            searchForArduino.RunWorkerAsync();
            this.statusLabel.Text = "Searching for Arduino...";
            this.StartButton.Enabled = false;
            this.comboBox1.Enabled = false;
        }

        private void searchForArduino_DoWork(object sender, DoWorkEventArgs e)
        {
            Boolean shouldContinue = true;
            do
            {
                try
                {
                    arduino.Open();
                    shouldContinue = false;
                }
                catch (System.IO.IOException)
                {
                    System.Threading.Thread.Sleep(350);
                }
            } while (shouldContinue);
            e.Result = true;
        } // looks for an arduino

        private void searchForArduino_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.StartButton.Text = "Log";
            this.StartButton.Enabled = true;
            this.StopButton.Enabled = true;
            this.statusLabel.Text = "Arduino found! Press Start.";
            try {
                readData.RunWorkerAsync(); }
            catch (Exception)
            {
            }
            mainUI.Show();
        }

        public static class filenames
        {
            public static string f1 { get; set; }
            public static string f2 { get; set; }
            public static string f3 { get; set; }
            public static string f4 { get; set; }
            public static string f5 { get; set; }
        }


        private void readData_DoWork(object sender, DoWorkEventArgs e)
        {
            //mainUI currentUI = (mainUI)e.Argument;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Start();
            float timeStamp = 0;

            string destinationPath = @"c:\TelemetryLogs";
            string currentDateTime = DateTime.Today.ToString("D") + " " + DateTime.Now.ToShortTimeString().Replace(':','-');

            string f1 = destinationPath + "\\" + currentDateTime + "1885.txt";
            string f2 = destinationPath + "\\" + currentDateTime + "1886.txt";
            string f3 = destinationPath + "\\" + currentDateTime + "1887.txt";
            string f4 = destinationPath + "\\" + currentDateTime + "1888.txt";
            string f5 = destinationPath + "\\" + currentDateTime + "1889.txt";

            filenames.f1 = f1;
            filenames.f2 = f2;
            filenames.f3 = f3;
            filenames.f4 = f4;
            filenames.f5 = f5;
            do
            {
                string messageID = arduino.ReadLine();

                mainUI.BeginInvoke((MethodInvoker)delegate
                {
                    mainUI.check.Text = messageID;
                });

                Dictionary<string, string> outDictionary = new Dictionary<string, string>();
                //lambda vs time, thermocouples

                if (messageID.StartsWith("ID 1885"))

                {
                    outDictionary = ParseIDtoDictionary(arduino, "RPM", "Speed", "throttle", "BAT"); //Switch speed w/ FP, OP with throttle

                    string toWrite = timeStamp.ToString() + "," + outDictionary["RPM"] + "," + outDictionary["Speed"] + "," + outDictionary["throttle"] + "," + outDictionary["BAT"];
                    printToFile(f1, toWrite);

                    mainUI.BeginInvoke((MethodInvoker)delegate
                    {
                        float speed = float.Parse(outDictionary["Speed"]);
                        float RPM = float.Parse(outDictionary["RPM"])/1000;

                        mainUI.rpmE.Value = RPM;
                        mainUI.KRPMvTime.Series["Series1"].Points.AddXY(timeStamp, RPM);
                        mainUI.KRPMvTime.Update();

                        mainUI.SpeedE.Value = speed;
                        mainUI.SpeedvsTime.Series["Series1"].Points.AddXY(timeStamp, speed);
                        mainUI.SpeedvsTime.Series[0].Points.AddXY(timeStamp, speed);
                        mainUI.SpeedvsTime.Update();

                        mainUI.SpeedvsRPM.Series["Series1"].Points.AddXY(speed, RPM);
                        mainUI.SpeedvsRPM.Update();

                        mainUI.Battery.Value = float.Parse(outDictionary["BAT"])/100;
                        mainUI.throttle.Text =  outDictionary["throttle"];
                        mainUI.ThrottlevsRPM.Series["Series1"].Points.AddXY(Convert.ToDouble(outDictionary["throttle"]), RPM);
                        mainUI.ThrottlevsRPM.Update();
                    });
                }

                else if (messageID.StartsWith("ID 1886"))
                {
                    outDictionary = ParseIDtoDictionary(arduino, "O2", "LongG", "RLWS", "LatG");

                    string toWrite = timeStamp.ToString() + "," + outDictionary["O2"] + "," + outDictionary["LongG"] + "," + outDictionary["RLWS"] + "," + outDictionary["LatG"];
                    printToFile(f2, toWrite);

                    mainUI.BeginInvoke((MethodInvoker)delegate
                    {
                        double latG = Convert.ToDouble(outDictionary["LatG"]);

                        mainUI.O2.Text = outDictionary["O2"];
                        mainUI.LambdavT.Series["Series1"].Points.AddXY(timeStamp, Convert.ToDouble(outDictionary["O2"]));

                        mainUI.LatG.Text = outDictionary["LatG"];
                        mainUI.LatGScroll.Value = Convert.ToInt32(20 * latG + 50);
                        mainUI.LatGvTime.Series["Series1"].Points.AddXY(timeStamp, latG);

                        double longG = Convert.ToDouble(outDictionary["LongG"]);
                        mainUI.LongGC.Text = outDictionary["LongG"];
                        mainUI.LongGScroll.Value = Convert.ToInt32(20 * longG + 50);
                        mainUI.LongGvTime.Series["Series1"].Points.AddXY(timeStamp, longG);

                        mainUI.GTrackpad.Series["Series1"].MarkerSize = 20;
                        mainUI.GTrackpad.Series["Series1"].Points.Clear();
                        mainUI.GTrackpad.Series["Series1"].Points.AddXY(latG, longG);
                    });

                }
                else if (messageID.StartsWith("ID 1887"))
                {
                    outDictionary = ParseIDtoDictionary(arduino, "EngineTemp", "Gear", "OP", "SteeringAngle");

                    string toWrite = timeStamp.ToString() + "," + outDictionary["EngineTemp"] + "," + outDictionary["Gear"] + "," + outDictionary["OP"] + "," + outDictionary["SteeringAngle"];
                    printToFile(f3, toWrite);

                    mainUI.BeginInvoke((MethodInvoker)delegate
                    {
                        mainUI.GearLabel.Text =  outDictionary["Gear"];
                        mainUI.OilPres.Text = outDictionary["OP"];
                        mainUI.EngineTemp.Text =  outDictionary["EngineTemp"];

                        mainUI.ETvT.Series["Series1"].Points.AddXY(timeStamp, Convert.ToDouble(outDictionary["EngineTemp"]));
                        //add steering angle
                    });
                }
                else if (messageID.StartsWith("ID 1888"))
                {
                    outDictionary = ParseIDtoDictionary(arduino, "WSFL", "WSFR", "WSRL", "WSRR");

                    string toWrite = timeStamp.ToString() + "," + outDictionary["WSFL"] + "," + outDictionary["WSFR"] + "," + outDictionary["WSRL"] + "," + outDictionary["WSRR"];
                    printToFile(f4, toWrite);

                    mainUI.BeginInvoke((MethodInvoker)delegate
                    {
                        mainUI.wsFL.Text = outDictionary["WSFL"];
                        mainUI.wsFR.Text = outDictionary["WSFR"];
                        mainUI.wsRL.Text = outDictionary["WSRL"];
                        mainUI.wsRR.Text = outDictionary["WSRR"];
                    });
                }
                else if (messageID.StartsWith("ID 1889"))
                {
                    outDictionary = ParseIDtoDictionary(arduino, "BP Front", "BP Rear", "fuelPres");

                    string toWrite = timeStamp.ToString() + "," + outDictionary["BP Front"] + "," + outDictionary["BP Rear"] + "," + outDictionary["fuelPres"];
                    printToFile(f5, toWrite);

                    mainUI.BeginInvoke((MethodInvoker)delegate
                    {
                        mainUI.BPF.Text = outDictionary["BP Front"];
                        mainUI.BPR.Text = outDictionary["BP Rear"];
                        mainUI.fuelPres.Text = outDictionary["fuelPres"];
                    });
                }
                timeStamp += timer.Interval;
                PostToThingWorx(outDictionary);
            } while (true);
        }


        static private Dictionary<string,string> ParseIDtoDictionary(SerialPort arduino, string A, string B, string C)
        {
            Dictionary<string, string> returnThis = new Dictionary<string,string>()
            {
                {A, readLineSplitted(arduino) },
                {B, readLineSplitted(arduino) },
                {C, readLineSplitted(arduino) },
            };
            return returnThis;
        }

        static private Dictionary<string, string> ParseIDtoDictionary(SerialPort arduino, string A, string B, string C, string D)
        {
            Dictionary<string, string> returnThis = new Dictionary<string, string>()
            {
                {A, readLineSplitted(arduino) },
                {B, readLineSplitted(arduino) },
                {C, readLineSplitted(arduino) },
                {D, readLineSplitted(arduino) },
            };
            return returnThis;
        }

        static private string readLineSplitted(SerialPort myDevice)
        {
            String[] temp = myDevice.ReadLine().Split(':');
            return temp.Length > 1 ? temp[1] : "0";
        }

        static async void PostToThingWorx(Dictionary<String, String> values)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://gatek.cloud.thingworx.com/Thingworx/Things/CarThing/Services/UpdateValues/" +
                    "?appKey=97ac7acf-4dd0-4992-bc04-d31c5b8e16f9&method=post&x-thingworx-session=true", content);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            arduino = new SerialPort(this.comboBox1.SelectedItem.ToString(), 9600);
            StartButton.Enabled = true;
        }

        private void printToFile(string filename, string writeThis)
        {
            System.IO.File.AppendAllText(@filename, writeThis + Environment.NewLine);
        }

        private void initUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            var window = MessageBox.Show(this, "Do you want to save logged data?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (window == DialogResult.No)
            {
                try {
                    System.IO.File.Delete(filenames.f1);
                    System.IO.File.Delete(filenames.f2);
                    System.IO.File.Delete(filenames.f3);
                    System.IO.File.Delete(filenames.f4);
                    System.IO.File.Delete(filenames.f5);
                }
                catch (System.ArgumentNullException)
                { }
            }
        }
    }
}
