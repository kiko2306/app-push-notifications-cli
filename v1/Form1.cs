using PusherClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {

        private string MyID;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                //Creating pusher object with authentication
                Pusher pusher = new Pusher("Development", new PusherOptions
                {
                    Host = "localhost:6001",
                });

                //Connecting to web socket
                await pusher.ConnectAsync().ConfigureAwait(false);

                this.MyID = pusher.SocketID;

                //Subscribing to channel
                Channel channel = await pusher.SubscribeAsync("home").ConfigureAwait(false);

                if (channel.IsSubscribed)
                {
                    //Binding to an event
                    channel.Bind("App\\Events\\NewMessage", (PusherEvent eventResponse) =>
                    {
                        // Deserialize json if server returns json values
                        Debug.WriteLine(eventResponse.Data);

                        this.textBox1.Invoke((MethodInvoker)delegate
                        {
                            textBox1.Text = textBox1.Text + eventResponse.Data + Environment.NewLine;
                        });

                    });
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("An exception occurred.");
            }
        }
    }
}
