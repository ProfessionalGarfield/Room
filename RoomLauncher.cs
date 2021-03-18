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

namespace Room
{
    public partial class RoomLauncher : Form
    {

        Socket sck;
        EndPoint epLocal, epRemote;
        byte[] buffer;
        string lastMessage;
        bool connected = false;

        public RoomLauncher()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            textLocalIp.Text = GetLocalIP();
            textLocalPort.Text = "80";
            textRemoteIp.Text = GetLocalIP();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (connected == false && textRemotePort.Text != string.Empty)
            {
                epLocal = new IPEndPoint(IPAddress.Parse(textLocalIp.Text), Convert.ToInt32(textLocalPort.Text));
                sck.Bind(epLocal);

                epRemote = new IPEndPoint(IPAddress.Parse(textRemoteIp.Text), Convert.ToInt32(textRemotePort.Text));
                sck.Connect(epRemote);
                buffer = new byte[1500];

                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);

                ASCIIEncoding aEncoding = new ASCIIEncoding();
                byte[] sendingMessage = new byte[1500];
                sendingMessage = aEncoding.GetBytes("Connected");

                sck.Send(sendingMessage);
                connected = true;
            }

        }
        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] recivedData = new byte[1500];
                recivedData = (byte[])aResult.AsyncState;

                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string recivedMessage = aEncoding.GetString(recivedData);

                listMessage.Items.Add("Friend: " + recivedMessage);

                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (connected == true)
            {
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                byte[] sendingMessage = new byte[1500];
                sendingMessage = aEncoding.GetBytes(textMessage.Text);

                sck.Send(sendingMessage);

                listMessage.Items.Add("Me: " + textMessage.Text);
                lastMessage = textMessage.Text;
                textMessage.Text = "";
            }
        }

        private void textMessage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    textMessage.Text = lastMessage;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    textMessage.Text = lastMessage;
                    break;
            }
        }

        private string GetLocalIP()
        {

            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
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
            MessageBox.Show("Ver 1.0", "Room", MessageBoxButtons.OK);
        }

        private void ColorTheme(string colorTheme) 
        {
            if (colorTheme == "dark")
            {
                this.BackColor = Color.FromArgb(45, 45, 48);
                listMessage.BackColor = Color.FromArgb(30, 30, 30);
                buttonConnect.ForeColor = Color.White;
                buttonSend.ForeColor = Color.White;
                buttonSend.BackColor = Color.FromArgb(70, 70, 70);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                groupBox1.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                menuStrip1.ForeColor = Color.White;
                menuStrip1.BackColor = Color.FromArgb(60, 60, 67);

            }
            else 
            {
                this.BackColor = Color.FromArgb(212, 208, 200);
                listMessage.BackColor = Color.White;
                buttonConnect.ForeColor = Color.Black;
                buttonSend.ForeColor = Color.Black;
                buttonSend.BackColor = Color.FromArgb(212, 208, 200);
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                groupBox1.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                menuStrip1.ForeColor = Color.Black;
                menuStrip1.BackColor = Color.FromArgb(212, 208, 200);

            }
        }
    }
}
