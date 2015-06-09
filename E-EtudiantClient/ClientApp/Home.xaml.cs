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
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public static IChatingService Server;
        private static DuplexChannelFactory<IChatingService> _chanelFactory;
        public List<Cour> Courlist, MesCours, ProfCoursList;
      
        public static Room room; 
        public Home()
        {
            InitializeComponent();
            _chanelFactory = new DuplexChannelFactory<IChatingService>(new CallBack(), "ChatingServicesEndPoint");
            Server = _chanelFactory.CreateChannel();
            Task task = Task.Run((Action)updateLists);
           
             
        }

        public  void updateLists()
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
               CoursPrevuListBox.Items.Clear();
               CoursOnlinListBox.Items.Clear();
                MesCourListBox.Items.Clear();
                Courlist = Server.getCours();
                ProfCoursList = Server.getCoursByProf(LoginLabel.Content.ToString());


               foreach (var cour in Courlist)
                {
                    if (cour.etats == "prevu")
                    {
                        CoursPrevuListBox.Items.Add(new CoursPrevu
                        {
                            id = cour.id.ToString(),
                            Image = @"ImageSource\index.jpg",
                            Text = cour.titre,
                            Date = cour.date,
                            NameProf = Server.GetProfNameByCourId(cour.id.ToString()),
                            Prix = cour.prix.ToString()
                        });


                    }
                    else if (cour.etats == "encours")
                    {

                        CoursOnlinListBox.Items.Add(new CoursOnline
                        {
                            id = cour.id.ToString(),
                            Image = @"ImageSource\index.jpg",
                            Text = cour.titre,
                            Date = cour.date,
                            NameProf = Server.GetProfNameByCourId(cour.id.ToString()),
                            Prix = cour.prix.ToString()
                        });
                    }


                }
                foreach (var cp in ProfCoursList)
                {
                    MesCourListBox.Items.Add(new MesCours
                    {
                        id = cp.id.ToString(),
                        Image = @"ImageSource\test.png",
                        Text = cp.titre,
                        Date = cp.date,
                        NameProf = cp.etats,
                        Prix = cp.prix.ToString()

                    });
                }
            }));
        }

      public  void updateListsLocal()
        {
      
      }

        private void Window_Closed(object sender, EventArgs e)
        {
            int result = Server.Logout(LoginLabel.Content.ToString());


        }

       

        private void LogoutItem_Click(object sender, RoutedEventArgs e)
        {
            
            Login l = new Login();
            l.LoginTextBox.Text = LoginLabel.Content.ToString();
            this.Close();
            l.Show();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    

        private void CoursOnlinListBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if(CoursOnlinListBox.SelectedItem != null )
            {
           
                CoursOnline c = CoursOnlinListBox.SelectedItem as CoursOnline;
            
                MessageBox.Show(c.id);
                
                room = new Room();
                
                room.UserNik.Content = LoginLabel.Content.ToString();
                
                room.RoomID.Content = c.id.ToString();
                
                Server.UserJointRoom(LoginLabel.Content.ToString(), c.id.ToString());
                           room.updateOnlineList();



                           room.Show();
            }

            }

        private void creatCourButton_Click(object sender, RoutedEventArgs e)
        {
            CreatCourWindow w = new CreatCourWindow();
            w.LoginLabel.Content = LoginLabel.Content.ToString();
            w.ShowDialog();
        }

     

        private void MesCourListBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MesCourListBox.SelectedItem != null )
            {
                MesCourListBox.ContextMenu.IsOpen = true;
            }else
            {
                
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MesCourListBox.SelectedItem != null)
            {
                MesCours c = MesCourListBox.SelectedItem as MesCours;
                Server.OpenCours(c.id.ToString(), LoginLabel.Content.ToString());
                MessageBox.Show("Cours Demarrer !");
                Task task = Task.Run((Action)Login.h.updateLists);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (MesCourListBox.SelectedItem != null)
            {
                MesCours c = MesCourListBox.SelectedItem as MesCours;
                if (c.NameProf == "encours")
                {
                    MessageBox.Show("le cours est en cours vous pouver pas le supprimer !!");
                   
                }else
                {
                    Server.DelCour(c.id, LoginLabel.Content.ToString());
                    MessageBox.Show("Cours Supprimer !");
                    Task task = Task.Run((Action)Login.h.updateLists);
                }
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (MesCourListBox.SelectedItem != null)
            {
                MesCours c = MesCourListBox.SelectedItem as MesCours;
                if (c.NameProf == "encours")
                {
                    Server.CloseCoure(c.id, LoginLabel.Content.ToString());             
                    Task task = Task.Run((Action)Login.h.updateLists);
                    MessageBox.Show("Cours arrete !");

                }else
                {
                    MessageBox.Show("le cours est deja en arret");
                }
            }
        }
        

       
    }
}
