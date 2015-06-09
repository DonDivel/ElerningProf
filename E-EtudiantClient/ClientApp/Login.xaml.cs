using ChatingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {


        public static Home h;
           
        public static IChatingService Server;
        private static DuplexChannelFactory<IChatingService> _chanelFactory;
        public Login()
        {
            InitializeComponent();
            _chanelFactory = new DuplexChannelFactory<IChatingService>(new CallBack(), "ChatingServicesEndPoint");
            Server = _chanelFactory.CreateChannel();
           
           
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
          
           
            LogMeIn();
        
          
            
        }

       

        private void InsciptionLabel_MouseLeave(object sender, MouseButtonEventArgs e)
        {
            Inscription c = new Inscription();
            c.Show();
            this.Close();
        }


        public void LogMeIn()
        {
            try
            {
                int returnedValue = Server.Login(LoginTextBox.Text, PasswordText.Password,"prof");
                
                if (returnedValue == 1)
                {
                    MessageBox.Show("Try another Login !!");
                    

                }
                else if (returnedValue == 0)
                {
                    MessageCostom message = new MessageCostom();
                    message.MessageLabel.Content = "You are Conected";
                    message.ShowDialog();
                     h = new Home();

               

                        Prof me = new Prof();
                        me = Server.GetProfInfo(LoginTextBox.Text);
                        h.BonjourLable.Content += me.prenom;
                        h.JetonLable.Content += me.jetons.ToString();
                        h.LoginLabel.Content = LoginTextBox.Text;
                        h.ProfItems.Visibility = Visibility.Hidden;

                    

                    h.Show();
                    this.Close();






                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Serveur inaccessible"+e.ToString());
                             
            }
        }

     

    }
}
