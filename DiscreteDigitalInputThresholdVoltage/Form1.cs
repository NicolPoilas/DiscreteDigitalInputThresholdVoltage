using System;
using System.Windows.Forms;
using canTransport;
using Uds;
using SecurityAccess;
using Dongzr.MidiLite;
using System.Text.RegularExpressions;
using System.Threading;

namespace DiscreteDigitalInputThresholdVoltage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BusParamsInit();
            MmTime_init();
            Trans_init();
        }

        can_driver driver = new can_driver();
        canTrans driverTrans = new canTrans();
        SecurityKey securityDriver = new SecurityKey();

        bool rx_success;
        int id;
        byte[] data = new byte[8];
        int dlc;
        long timestamp;

        int Startflag;
        int io;

        int first_state = 0;
        int last_state = 0;
        int num_changed = 0;
        int count = 0;

        #region BusSetting
        private void BusParamsInit()
        {
            string[] channel = new string[0];
            channel = driver.GetChannel();
            comboBoxChannel.Items.Clear();
            comboBoxChannel.Items.AddRange(channel);//add items for comboBox
            comboBoxChannel.SelectedIndex = 0;//default select the first , physical driver always come first
            comboBoxBaudrate.SelectedIndex = 4;//default select 500K   
            comboBoxSession.SelectedIndex = 2;
            comboBoxAccess.SelectedIndex = 4;
            comboBoxIO.SelectedIndex = 0;
        }

        private void BusOffOn_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus On")
            {
                if (driver.OpenChannel(comboBoxChannel.SelectedIndex, comboBoxBaudrate.Text) == true)
                {
                    BusOffOn.Text = "Bus Off";
                    driverTrans.Start();
                    mmTimer.Start();
                    labelBusLoad.Text = "Bus Load:" + driver.BusLoad().ToString() + "%";
                    comboBoxBaudrate.Enabled = false;
                    comboBoxChannel.Enabled = false;
                }
                else
                {
                    MessageBox.Show("打开" + comboBoxChannel.Text + "通道失败!"); //最好能把原因定位出来 给故障编码写入帮助文件
                }
            }
            else
            {
                BusOffOn.Text = "Bus On";
                driverTrans.Stop();
                mmTimer.Stop();
                t_Stop();
                driver.CloseChannel();
                labelBusLoad.Text = "Bus Load:0%";
                buttonSession.Enabled = true;//使能所有发送数据的开关
                buttonAccess.Enabled = true;
                buttonCMD.Text = "Monitor";
                Startflag = 0;
                comboBoxBaudrate.Enabled = true;
                comboBoxChannel.Enabled = true;
                groupBox2.Enabled = true;
                comboBoxIO.Enabled = true;
                buttonDebug.Enabled = true;
                checkBoxChanged.Enabled = true;
            }
        }
        #endregion

        private void buttonSessoin_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus Off")
                driverTrans.CanSendString("10" + comboBoxSession.Text);//默认1003
        }

        private void buttonAccess_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus Off")
                driverTrans.CanSendString("27" + comboBoxAccess.Text);//默认2709
        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus Off")
                driverTrans.CanSendString("3101FFF9");//进入Debug模式
        }

        private void buttonCMD_Click(object sender, EventArgs e)
        {
            if ((buttonCMD.Text == "Monitor") && (BusOffOn.Text == "Bus Off"))
            {
                buttonCMD.Text = "Stop";
                driverTrans.Stop();
                t_Start();
                Startflag = 1;
                groupBox2.Enabled = false;
                comboBoxIO.Enabled = false;
                buttonDebug.Enabled = false;
                checkBoxChanged.Enabled = false;
                count = 0;
                num_changed = 0;
            }
            else
            {
                buttonCMD.Text = "Monitor";
                driverTrans.Start();
                t_Stop();
                Startflag = 0;
                groupBox2.Enabled = true;
                comboBoxIO.Enabled = true;
                buttonDebug.Enabled = true;
                checkBoxChanged.Enabled = true;
            }
        }


        private void comboBoxIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxIO.SelectedItem != null)
            {
                io = comboBoxIO.SelectedIndex;
                first_state = 0;
                last_state = 0;
            }
        }

        /*将十六进制字符串转换成十六进制数组（不足末尾补0），失败返回空数组*/
        byte[] StringToHex(string strings)
        {
            byte[] hex = new byte[0];
            try
            {
                strings = strings.Replace("0x", "");
                strings = strings.Replace("0X", "");
                strings = strings.Replace(" ", "");
                strings = Regex.Replace(strings, @"(?i)[^a-f\d\s]+", "");//表示不可变正则表达式
                if (strings.Length % 2 != 0)
                {
                    strings += "0";
                }
                hex = new byte[strings.Length / 2];
                for (int i = 0; i < hex.Length; i++)
                {
                    hex[i] = Convert.ToByte(strings.Substring(i * 2, 2), 16);
                }
                return hex;
            }
            catch
            {
                return hex;
            }
        }

        /*将十六进制数组转换成十六进制字符串，并以space隔开*/
        public string HexToStrings(byte[] hex, string space)
        {
            string strings = "";
            for (int i = 0; i < hex.Length; i++)//逐字节变为16进制字符，并以space隔开
            {
                strings += hex[i].ToString("X2") + space;
            }
            return strings;
        }

        /*使用事件委托传参*/
        void Trans_init()
        {
            driverTrans.EventTxFarms += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.FarmsEventArgs args = (canTrans.FarmsEventArgs)e1;
                    EventHandler TextBoxDisplayUpdate = delegate
                    {
                        richTextBox1.AppendText("$" + args.ToString() + "\r\n");//发送数据流
                        richTextBox1.ScrollToCaret();
                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventRxFarms += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.FarmsEventArgs args = (canTrans.FarmsEventArgs)e1;
                    EventHandler TextBoxDisplayUpdate = delegate
                    {
                        richTextBox1.AppendText("$" + args.ToString() + "\r\n");//接收数据流
                        richTextBox1.ScrollToCaret();
                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventRxMsgs += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.RxMsgsEventArgs RxMsgs = (canTrans.RxMsgsEventArgs)e1;
                    AutoResponse(StringToHex(RxMsgs.ToString()));
                    EventHandler TextBoxDisplayUpdate = delegate
                    {

                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventError += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.ErrorEventArgs args = (canTrans.ErrorEventArgs)e1;
                    EventHandler TextBoxDisplayUpdate = delegate
                    {

                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );

            driverTrans.tx_id_load = 0x7B0;//发送ID
            driverTrans.rx_id_load = 0x7B8;//接收ID

            driverTrans.CanRead += driver.ReadData;
            driverTrans.CanWrite += driver.WriteData;
        }

        /*判断安全进入*/
        private void AutoResponse(byte[] data)
        {
            if (data[0] == 0x67)
            {
                uint seed = 0;
                byte level;
                uint key = 0;
                if (data.Length == 4)
                {
                    seed = (uint)data[2] << 8
                        | (uint)data[3];
                }
                else if (data.Length == 6)
                {
                    seed = (uint)data[2] << 24
                        | (uint)data[3] << 16
                        | (uint)data[4] << 8
                        | (uint)data[5];
                }
                level = data[1];
                if (seed != 0 && level % 2 != 0)
                {
                    key = securityDriver.UdsCallback_CalcKey(seed, level);
                    if (data.Length == 4)
                    {
                        key &= 0xFFFF;
                        driverTrans.CanSendString("27" + (level + 1).ToString("x2") + key.ToString("x4"));
                    }
                    else if (data.Length == 6)
                    {
                        driverTrans.CanSendString("27" + (level + 1).ToString("x2") + key.ToString("x8"));
                    }
                }
            }
        }

        /*定时器*/
        #region Timer
        public delegate void Tick_10ms();
        public delegate void Tick_20ms();
        public delegate void Tick_50ms();
        public delegate void Tick_100ms();
        public delegate void Tick_200ms();
        public delegate void Tick_1s();
        public delegate void Tick_2s();
        public delegate void Tick_5s();
        public delegate void Tick_60s();
        public Tick_10ms mmtimer_tick_10ms;
        public Tick_10ms mmtimer_tick_20ms;
        public Tick_10ms mmtimer_tick_50ms;
        public Tick_100ms mmtimer_tick_100ms;
        public Tick_200ms mmtimer_tick_200ms;
        public Tick_1s mmtimer_tick_1s;
        public Tick_2s mmtimer_tick_2s;
        public Tick_5s mmtimer_tick_5s;
        public Tick_60s mmtimer_tick_60s;
        public MmTimer mmTimer;
        const int timer_interval = 10;
        int timer_10ms_counter = 0;
        int timer_20ms_counter = 0;
        int timer_50ms_counter = 0;
        int timer_100ms_counter = 0;
        int timer_200ms_counter = 0;
        int timer_1s_counter = 0;
        int timer_2s_counter = 0;
        int timer_5s_counter = 0;
        int timer_60s_counter = 0;

        private void MmTime_init()
        {
            mmTimer = new MmTimer
            {
                Mode = MmTimerMode.Periodic,
                Interval = timer_interval
            };
            mmTimer.Tick += mmTimer_tick;

            mmtimer_tick_10ms += delegate
            {

            };

            mmtimer_tick_20ms += delegate
            {

            };

            mmtimer_tick_50ms += delegate
            {

            };

            mmtimer_tick_100ms += delegate
            {

            };

            mmtimer_tick_200ms += delegate
            {

            };

            mmtimer_tick_1s += delegate
            {
                EventHandler BusLoadUpdate = delegate
                {
                    labelBusLoad.Text = "Bus Load:" + driver.BusLoad().ToString() + "% ";//更新BusLoad
                };
                try { Invoke(BusLoadUpdate); } catch { };
            };

            mmtimer_tick_2s += delegate
            {

            };

            mmtimer_tick_5s += delegate
            {

            };

            mmtimer_tick_60s += delegate
            {

            };
        }

        void mmTimer_tick(object sender, EventArgs e)
        {
            timer_10ms_counter += timer_interval;
            if (timer_10ms_counter >= 10)
            {
                timer_10ms_counter = 0;
                mmtimer_tick_10ms?.Invoke();
            }

            timer_20ms_counter += timer_interval;
            if (timer_20ms_counter >= 10)
            {
                timer_20ms_counter = 0;
                mmtimer_tick_20ms?.Invoke();
            }

            timer_50ms_counter += timer_interval;
            if (timer_50ms_counter >= 50)
            {
                timer_50ms_counter = 0;
                mmtimer_tick_50ms?.Invoke();
            }

            timer_100ms_counter += timer_interval;
            if (timer_100ms_counter >= 100)
            {
                timer_100ms_counter = 0;
                mmtimer_tick_100ms?.Invoke();
            }

            timer_200ms_counter += timer_interval;
            if (timer_200ms_counter >= 200)
            {
                timer_200ms_counter = 0;
                mmtimer_tick_200ms?.Invoke();
            }

            timer_1s_counter += timer_interval;
            if (timer_1s_counter >= 1000)
            {
                timer_1s_counter = 0;
                mmtimer_tick_1s?.Invoke();
            }

            timer_2s_counter += timer_interval;
            if (timer_2s_counter >= 2000)
            {
                timer_2s_counter = 0;
                mmtimer_tick_2s?.Invoke();
            }

            timer_5s_counter += timer_interval;
            if (timer_5s_counter >= 5000)
            {
                timer_5s_counter = 0;
                mmtimer_tick_5s?.Invoke();
            }

            timer_60s_counter += timer_interval;
            if (timer_60s_counter >= 60000)
            {
                timer_60s_counter = 0;
                mmtimer_tick_60s?.Invoke();
            }
        }
        #endregion

        #region thread t_Receive
        Thread t_Receive;
        private void t_Receive_Thread()
        {
            while (true)
            {
                int i = 0;
                while (i < 50)
                {
                    CycleRecieve();
                    i++;
                }
                t_Sleep(20);//休息10ms
            }
        }

        private void CycleRecieve()
        {
            rx_success = driver.ReadData(out id, ref data, out dlc, out timestamp);//接收一帧数据
            if (Startflag == 1)//从这里读取的Buff数据才是有效的
            {
                if ((rx_success) && (id == 0x003) && (dlc == 8))
                {
                    switch (io)
                    {
                        case 0:
                            {
                                if ((data[0] & 0x04) != 0x04)//KEY
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 1:
                            {
                                if ((data[0] & 0x02) != 0x02)//ACC
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 0;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 1;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 2:
                            {
                                if ((data[0] & 0x01) != 0x01)//IGN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 0;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 1;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 3:
                            {
                                if ((data[0] & 0x08) != 0x08)//CRANK
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 4:
                            {
                                if ((data[0] & 0x10) != 0x10)//刹车踏板
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 5:
                            {
                                if ((data[1] & 0x80) != 0x80)//后雨刮停靠
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 6:
                            {
                                if ((data[4] & 0x02) != 0x02)//RESERVED_1_IN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 7:
                            {
                                if ((data[4] & 0x04) != 0x04)//RESERVED_2_IN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 8:
                            {
                                if ((data[4] & 0x08) != 0x08)//RESERVED_3_IN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 9:
                            {
                                if ((data[3] & 0x04) != 0x04)//倒车灯
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 0;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 1;

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 10:
                            {
                                if ((data[3] & 0x02) != 0x02)//离合踏板
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 0;
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                        last_state = 1;

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 11:
                            {
                                if ((data[0] & 0x20) != 0x20)//危险灯报警
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 12:
                            {
                                if ((data[3] & 0x08) != 0x08)//P档
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 13:
                            {
                                if ((data[3] & 0x10) != 0x10)//空挡
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 14:
                            {
                                if ((data[3] & 0x20) != 0x20)//后除霜
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 15:
                            {
                                if ((data[0] & 0x40) != 0x40)//后备箱
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 16:
                            {
                                if ((data[1] & 0x04) != 0x04)//左转向诊断
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 17:
                            {
                                if ((data[2] & 0x08) != 0x088)//右转向诊断
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 18:
                            {
                                if ((data[3] & 0x40) != 0x40)//后视镜加热
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 19:
                            {
                                if ((data[3] & 0x80) != 0x80)//后视镜折叠
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 20:
                            {
                                if ((data[2] & 0x10) != 0x10)//前雨刮1
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 21:
                            {
                                if ((data[2] & 0x20) != 0x20)//前雨刮2
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 22:
                            {
                                if ((data[2] & 0x40) != 0x40)//前雨刮停靠
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 23:
                            {
                                if ((data[0] & 0x80) != 0x80)//TRUNKAJA
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 24:
                            {
                                if ((data[1] & 0x01) != 0x01)//HORN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 25:
                            {
                                if ((data[4] & 0x01) != 0x01)//冬季模式
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 26:
                            {
                                if ((data[1] & 0x02) != 0x02)//后窗锁止
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 27:
                            {
                                if ((data[1] & 0x80) != 0x80)//司机侧门所反馈信号
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 28:
                            {
                                if ((data[2] & 0x01) != 0x01)//乘客侧门锁反馈信号
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 29:
                            {
                                if ((data[2] & 0x02) != 0x02)//左后侧门锁反馈信号
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 30:
                            {
                                if ((data[2] & 0x04) != 0x04)//右后侧门锁反馈信号
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 31:
                            {
                                if ((data[1] & 0x04) != 0x04)//FLAJA
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 32:
                            {
                                if ((data[1] & 0x08) != 0x08)//FRAJA
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 33:
                            {
                                if ((data[1] & 0x10) != 0x10)//RLAJA
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();

                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 34:
                            {
                                if ((data[1] & 0x20) != 0x20)//RRAJA
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();


                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 35:
                            {
                                if ((data[4] & 0x10) != 0x10)//RESERVED_4_IN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                        case 36:
                            {
                                if ((data[3] & 0x01) != 0x01)//RESERVED_5_IN
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  0   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                                else
                                {
                                    EventHandler Display = delegate
                                    {
                                        richTextBoxDisplay.AppendText("当前输入口逻辑状态为:  1   " + DateTime.Now.ToString() + "\r\n");
                                        richTextBoxDisplay.ScrollToCaret();
                                    };
                                    try { Invoke(Display); } catch { };
                                }
                            }
                            break;
                    }
                    if (checkBoxChanged.Checked)
                    {
                        count++;
                        if (count <= 50)
                        {
                            if (last_state != first_state)//连续两次state不一致
                                num_changed++;
                        }
                    }
                    first_state = last_state;
                    EventHandler changed = delegate
                    {
                        labelChanged.Text = "Num of State Changes: " + num_changed.ToString();
                    };
                    try { Invoke(changed); } catch { };
                }
            }
        }

        public void t_Start()
        {
            t_Receive = new Thread(new ThreadStart(t_Receive_Thread));
            t_Receive.IsBackground = true;
            t_Receive.Priority = ThreadPriority.Lowest;
            t_Receive.Start();
        }

        public void t_Stop()
        {
            if (t_Receive != null && t_Receive.IsAlive)
            {
                t_Receive.Abort();
            }
        }

        public void t_Sleep(int timespan)
        {
            if (t_Receive != null && t_Receive.IsAlive)
            {
                Thread.Sleep(timespan);
            }
        }
        #endregion

        private void coverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(richTextBoxDisplay.Text);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxDisplay.Clear();
        }
    }
}