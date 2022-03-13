using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Room
{
    public partial class RoomLauncher2 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        //Socket sck;
        //EndPoint epLocal, epRemote;
       // byte[] buffer;
        string lastMessage;
        bool connected = false;

        public RoomLauncher2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
           // sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            textRemoteIp.Text = "127.0.0.1";
            textRemotePort.Text = "8888";
            //textRemoteIp.Text = GetLocalIP();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                readData = "Conected to Chat Server ...";
                AddMessage();

                string ip = Convert.ToString(textRemoteIp.Text);
                int port = Convert.ToInt32(textRemotePort.Text);
                clientSocket.Connect(ip, port);
                serverStream = clientSocket.GetStream();

                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(DataBox.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(MessageCallBack);
                ctThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void MessageCallBack()
        {
            try
            {
                while (true)
                {
                    serverStream = clientSocket.GetStream();
                    int buffSize = 0;
                    byte[] inStream = new byte[10025];
                    buffSize = clientSocket.ReceiveBufferSize;
                    serverStream.Read(inStream, 0, buffSize);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                    readData = "" + returndata;
                    AddMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void AddMessage()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(AddMessage));
            else
                listMessage.Items.Add(Environment.NewLine + " >> " + readData);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(TextMessage.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void textMessage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    TextMessage.Text = lastMessage;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    TextMessage.Text = lastMessage;
                    break;
            }
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorTheme("Light");
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorTheme("dark");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ver 2.0", "Room", MessageBoxButtons.OK);
        }

        private void ColorTheme(string colorTheme) 
        {
            if (colorTheme == "dark")
            {
                DataBox.BackColor = Color.FromArgb(30, 30, 30);
                DataBox.ForeColor = Color.White;
                this.BackColor = Color.FromArgb(45, 45, 48);
                listMessage.BackColor = Color.FromArgb(30, 30, 30);
                listMessage.ForeColor = Color.White;
                textRemoteIp.BackColor = Color.FromArgb(30, 30, 30);
                textRemoteIp.ForeColor = Color.White;
                textRemotePort.BackColor = Color.FromArgb(30, 30, 30);
                textRemotePort.ForeColor = Color.White;
                TextMessage.BackColor = Color.FromArgb(30, 30, 30);
                TextMessage.ForeColor = Color.White;
                buttonConnect.BackColor = Color.FromArgb(70, 70, 70);
                buttonConnect.ForeColor = Color.White;
                buttonSend.ForeColor = Color.White;
                buttonSend.BackColor = Color.FromArgb(70, 70, 70);
                label1.ForeColor = Color.White;
                //label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
               // groupBox1.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                menuStrip1.ForeColor = Color.White;
                menuStrip1.BackColor = Color.FromArgb(60, 60, 67);

            }
            else 
            {
                this.BackColor = Color.FromArgb(212, 208, 200);
                DataBox.BackColor = Color.White;
                DataBox.ForeColor = Color.Black;
                listMessage.BackColor = Color.White;
                listMessage.ForeColor = Color.Black;
                TextMessage.BackColor = Color.White;
                TextMessage.ForeColor = Color.Black;
                textRemoteIp.BackColor = Color.White;
                textRemoteIp.ForeColor = Color.Black;
                textRemotePort.BackColor = Color.White;
                textRemotePort.ForeColor = Color.Black;
                buttonConnect.ForeColor = Color.Black;
                buttonSend.ForeColor = Color.Black;
                buttonSend.BackColor = Color.FromArgb(212, 208, 200);
                label1.ForeColor = Color.Black;
               // label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
               // groupBox1.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                menuStrip1.ForeColor = Color.Black;
                menuStrip1.BackColor = Color.FromArgb(212, 208, 200);

            }
        }
    }
}
