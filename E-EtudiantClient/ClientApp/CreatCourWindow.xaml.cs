using ChatingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
    /// Logique d'interaction pour CreatCourWindow.xaml
    /// </summary>
    public partial class CreatCourWindow : Window
    {
        public static IChatingService Server;
        private static DuplexChannelFactory<IChatingService> _chanelFactory;
        public CreatCourWindow()
        {
            InitializeComponent();
            _chanelFactory = new DuplexChannelFactory<IChatingService>(new CallBack(), "ChatingServicesEndPoint");
            Server = _chanelFactory.CreateChannel();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Run((Action)creatCours);

        }

        private void creatCours()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
            try
            {
             
                Cour c = new Cour()
                  {
                      titre = titreTextBox.Text.ToString(),
                      duree = int.Parse(DureeTextBox.Text),
                      date = DateTextBox.Text.ToString(),
                      prix = int.Parse(PrixTextBox.Text)
                  };
               

                    Server.CreatCours(c, LoginLabel.Content.ToString());
                    MessageBox.Show("cours Created ");
                    Task task = Task.Run((Action)Login.h.updateLists); 
                    this.Close();
               
            }catch(Exception e)
            {
                MessageBox.Show("err Creating Cours : "+e.ToString());
            }
           }));
        }
    }
}
