using Grpc.Net.Client;
using SampleClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ClientSide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            this.KeyDown += KeyDownFunc;

            MessageList = new ObservableCollection<Message>();

            this.DataContext = this;
        }



        private ObservableCollection<Message> _messageList;
        public ObservableCollection<Message> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                OnPropertyChanged("MessageList");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void KeyDownFunc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageEntered();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageEntered();
        }

        private void MessageEntered()
        {
            if (UserInputBox.Text.Trim() != "")
            {
                string text = UserInputBox.Text.Trim();
                MessageList.Add(new Message()
                {
                    MessageDetail = "Client says: " + text,
                    Colour = new SolidColorBrush(Colors.Green)

                });
                UserInputBox.Text = "";

                _ = ServerResponse(text);

            }
        }

        private async Task ServerResponse(string messge)
        {
            //using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //var client = new Greeter.GreeterClient(channel);

            try
            {
                var channel = GrpcChannel.ForAddress("http://http://10.49.50.117/24:5116");
                var client = new Greeter.GreeterClient(channel);
                var reply = await client.SayHelloAsync(new HelloRequest { Name = messge });

                MessageList.Add(new Message()
                {
                    MessageDetail = "Server responses: " + reply.Message,
                    Colour = new SolidColorBrush(Colors.Red)

                });
            }catch (Exception ex)
            {
                var abc = 0;
            }
            //using (var channel = GrpcChannel.ForAddress("http://172.26.112.1:7114"))
            //{
            //    var client = new Greeter.GreeterClient(channel);
            //    var reply = await client.SayHelloAsync(new HelloRequest { Name = messge });

            //    MessageList.Add(new Message()
            //    {
            //        MessageDetail = "Server responses: " + reply.Message,
            //        Colour = new SolidColorBrush(Colors.Red)

            //    });
            //}

        }

    }

    public class Message
    {
        public string MessageDetail { get; set; }
        public Brush Colour { get; set; }
    }
}
