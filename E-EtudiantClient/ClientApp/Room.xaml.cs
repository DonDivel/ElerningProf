using ChatingInterfaces;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCSharpWebCam;


namespace ClientApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class Room : Window
    {
        public Task task;
        
        public static IChatingService Server;
        private static DuplexChannelFactory<IChatingService> _chanelFactory;
        private static bool IsSharing;
        public static bool Camera= false;
        public static String Username;
        public static String RoomId;
        private BufferedWaveProvider bufferedWaveProvider, bufferedWaveProviderRecive;
        private WaveOut player;
        public bool IsLisning;
        public bool IsRecording;
        private WaveInEvent recorder;
        public WaveFormat newFormat;
        public ImageBrush HandIcon, MicroIcon, MicroIconOnHold;
        private  List<ImgUser> UsernamesConected = new List<ImgUser>();
        List<User> Onlinelist;
        RoomReduit r;
        WebCam webcam;
        private object _locker = new object();
        public Room()
        {
            InitializeComponent();



            webcam = new WebCam();
            


            IsLisning = false;
            IsRecording = false;
            
             MicroIcon = new ImageBrush();
              MicroIconOnHold = new ImageBrush();
              
          
          
            MicroIcon.ImageSource = new BitmapImage(new Uri(@"..\..\ImageSource\microphone.png", UriKind.Relative));
            MicroIconOnHold.ImageSource = new BitmapImage(new Uri(@"..\..\ImageSource\microphoneOnHold.png", UriKind.Relative));
            
            MicroButton.Background = MicroIcon;
       
            _chanelFactory = new DuplexChannelFactory<IChatingService>(new CallBack(), "ChatingServicesEndPoint");
            Server = _chanelFactory.CreateChannel();
          
             newFormat = new WaveFormat(48000, 16, 1);
            bufferedWaveProvider = new BufferedWaveProvider(newFormat);
            player = new WaveOut();
            player.Init(bufferedWaveProvider);
           Onlinelist = new List<User>();




           r = new RoomReduit();


        }
    

        public void takeMessage(string message, string username)
        {
           
           
    
         ReciveListBox.Items.Add(username + " : " + message + "\n");
    
         

        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            

            this.Dispatcher.Invoke((Action)(() =>
    {
        Server.sendMessageToAll(SendTextbox.Text, UserNik.Content.ToString(), RoomID.Content.ToString());
    }));
            ReciveListBox.Items.Add("You" + " : " + SendTextbox.Text + "\n");

            SendTextbox.Text = "";
            
        }


        public void addUser(string username)
        {
            updateOnlineList();
          

         

         
               

        }
        public void removeUser(string username)
        {

            updateOnlineList();
            

        }




        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsLisning = false;
            IsSharing = false;
            Server.UserLeaveRoom(UserNik.Content.ToString(), RoomID.Content.ToString());
        }

        public void SetScreen(byte[] screen)
        {
            
          

        }

        

   


        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }



        private static void MyFunction()
        {
            
          
            while (IsSharing)
            {
                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics graphics = Graphics.FromImage(bitmap as System.Drawing.Image);
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                Server.SendScreen(Username, ImageToByte(bitmap), RoomId);
                 Console.WriteLine(  "\n"+Username+" Is Sending Data with room  id  "+RoomId );
            }
        }

    

      










        internal void StartLisning(byte[] b, int offset, int bsize)
        {
          
                bufferedWaveProvider.AddSamples(b, offset, bsize);

               
                if (!IsLisning)
                {
                    player.Play();
                    IsLisning = true;
                }
            
          
           
        }

        private void MicroButton_Click(object sender, RoutedEventArgs e)
        {
            Username = UserNik.Content.ToString();
            RoomId = RoomID.Content.ToString();
            Task task = Task.Run((Action)talk);
          
        }
      

        private void Window_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
           
        }



        void talk() 
        {
            if (IsRecording)
            {
                recorder.StopRecording();
              
                IsRecording = false;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Server.StopSendVoice(UserNik.Content.ToString(), RoomID.Content.ToString());
                }));

            }
            else
            {
                

             
                recorder = new WaveInEvent();
                recorder.WaveFormat = newFormat;
                recorder.DataAvailable +=new EventHandler<WaveInEventArgs>(RecorderOnDataAvailable);
                recorder.StartRecording();
                IsRecording = true;
               
            }
        }

    
        private void RecorderOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            Server.SendVoice(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded, Username, RoomId);
            Console.WriteLine("data sending " + Username + "" + RoomId);
        }




        internal void UserRaisedHand(string username)
        {
            
          
           
        }

      

        public void updateOnlineList()
        {
            OnlineListBox.Items.Clear();
            Onlinelist = Server.getOnlineList(RoomID.Content.ToString());
            foreach(var user in Onlinelist)
            {
                OnlineListBox.Items.Add(new ImgUser() {Username = user.pseudo, Image = user.icon });
            }
            
        }

        private void CmeraButton_Click(object sender, RoutedEventArgs e)
        {
           
                if (!Camera)
                {
                    
                    Username = UserNik.Content.ToString();
                    RoomId = RoomID.Content.ToString();
                    
                    Camera = true;
                    webcam.InitializeWebCam(ref CameraBox, Username, RoomId);
                    
                     webcam.Start();
                    
                    CmeraButton.Background = System.Windows.Media.Brushes.Blue;
                    r.CameraButton.Background = System.Windows.Media.Brushes.Blue;
                    CmeraButton.Content = "Camera ON";
                 
                }
                else
                {
                   
                    
                    Camera = false;
                
                    webcam.Stop();
                    CmeraButton.Background = System.Windows.Media.Brushes.Gray;
                    CmeraButton.Content = "Camera OFF";
                    CameraBox.Source = new BitmapImage(new Uri(@"..\..\ImageSource\no_webcam.png", UriKind.Relative)); ;
                     
                }
           
            

           
        }

        private void OptionsButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            webcam.ResolutionSetting();
            webcam.AdvanceSetting();
        }

        private void paratgebutton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSharing)
            {
                IsSharing = true;
                Username = UserNik.Content.ToString();
                RoomId = RoomID.Content.ToString();

                Task task = Task.Run((Action)MyFunction);
                paratgebutton.Background = System.Windows.Media.Brushes.Blue;
                paratgebutton.Content = "Partage D'ecrant On";
            }
            else
            {
                IsSharing = false;
                paratgebutton.Background = System.Windows.Media.Brushes.Gray;
                paratgebutton.Content = "Partage D'ecrant Off";
            }
        }

        private void ReduitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
           
            r.Show();
            
            
        }



        internal void stopLisning()
        {
            IsLisning = false;
            player.Stop();
        }
    }
}
