using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace Chat_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IHubProxy _proxy;
        private void Form1_Load(object sender, EventArgs e)
        {
            HubConnection con = new HubConnection("http://localhost:58415");
            _proxy = con.CreateHubProxy("chat");
            con.Start();
            _proxy.On<Message>("newMessage", (m) => listBox1.Invoke(new Action(() => listBox1.Items.Add(m.name + " : " + m.message))));
            _proxy.On<string, string>("newMember", (n, g) => listBox1.Invoke(new Action(() => listBox1.Items.Add(n + " join to " + g))));
            _proxy.On<string, string, string>("group", (n,m,g) => listBox1.Invoke(new Action(() => listBox1.Items.Add(n + " : " + m))));
        }
       
        private void send_Click(object sender, EventArgs e)
        {
            _proxy.Invoke("sendMessage", new Message()
            {
               name=txt_name.Text,message=txt_message.Text
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _proxy.Invoke("joinMember", txt_group.Text,txt_name.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _proxy.Invoke("sendGroup", txt_group.Text, txt_message.Text,txt_name.Text);
        }
    }
}
