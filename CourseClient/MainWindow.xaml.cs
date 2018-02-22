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

namespace CourseClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyClient cl;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void appendMsg(string data)
        {
            chat.AppendText(data);
        }

        class MyClient : Client
        {
            public MyClient(MainWindow context)
            {
                this.context = context;
            }
            MainWindow context;

            delegate void AppendDeligate(string msg);

            public override void reply(string msg)
            {
                if (!context.chat.Dispatcher.CheckAccess())
                {
                    AppendDeligate ad = new AppendDeligate(reply);
                    context.Dispatcher.Invoke(ad, new object[] { msg });
                }
                else
                {
                    context.chat.AppendText(msg + '\n');
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = Msgbox.Text;
                cl.Send(msg);
            }
            catch
            {
                cl.reply("Failed to send a message, please try connecting again");
            }
        }

        private void Connect_btn_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Connect_btn.Content == "Connect")
            {
                cl = new MyClient(this);
                try
                {
                    Connect_btn.IsEnabled = false;
                    cl.connect();
                    cl.reply("Connected");
                    Connect_btn.Content = "Disconect";
                    Connect_btn.IsEnabled = true;
                    SendBtn.IsEnabled = true;

                }
                catch (Exception e1)
                {
                    cl.reply("Connection failed");
                    Connect_btn.IsEnabled = true;
                    return;
                }
            }
            else
            {
                try
                {
                    Connect_btn.IsEnabled = false;
                    cl.ClientDis();
                    cl.reply("Disconnected");
                    Connect_btn.Content = "Connect";
                    Connect_btn.IsEnabled = true;
                    SendBtn.IsEnabled = false;
                    cl = null;
                }
                catch (Exception e1)
                {
                    cl.reply("You are already disconnected");
                    Connect_btn.Content = "Connect";
                    Connect_btn.IsEnabled = true;
                    cl = null;
                    return;
                }

            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((string)Connect_btn.Content == "Disconect")
            {
                try
                {
                    cl.ClientDis();
                }
                catch (Exception e1)
                {
                }

            }
            MainWindow1.Close();
        }
    }

}
//context.chat.Dispatcher.CheckAccess()