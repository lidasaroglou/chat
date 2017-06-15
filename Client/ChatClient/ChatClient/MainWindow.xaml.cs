using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using Contract;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<string> Message;
        TcpClient tcpClient;
        public MainWindow()
        {
            InitializeComponent();

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect("192.168.11.100", 5002);

                Stream stm = tcpClient.GetStream();

                String useraname = "giannis";
                var writer = new StreamWriter(stm) { AutoFlush = true };

                writer.WriteLine(useraname);


                while (true)
                {
                    var reader = new StreamReader(stm);
                    var line = await reader.ReadLineAsync();
                    var msg = JsonConvert.DeserializeObject<JSONserialize>(line);
                    output.Text += msg.Text+Environment.NewLine;

                }

            }
            catch (Exception ex)
            {

            }

        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key != Key.Return) return;

            String text = ((TextBox)sender).Text;

            if (text[0]=='@')
            {
                int index = text.IndexOf(" ");
                var username = text.Substring(1, index - 1);
                var msg = new JSONserialize();
                msg.Type = Contract.MessageType.Unicast;
                msg.Text = text;
                msg.Receiver = username;

                var writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                writer.WriteLine(JsonConvert.SerializeObject(msg));

            } else
            {
                var msg = new JSONserialize();
                msg.Type = Contract.MessageType.Broadcast;
                msg.Text = text;

                var writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                writer.WriteLine(JsonConvert.SerializeObject(msg));
            }

            
            
            //Message?.Invoke(this, ((TextBox)sender).Text);
            //((TextBox)sender).Text = "";
        }
    }
}
