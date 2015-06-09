using ChatingInterfaces;
using ClientApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WebCam_Capture;


namespace WPFCSharpWebCam
{
    //Design by Pongsakorn Poosankam
    class WebCam
    {
        public static IChatingService Server;
        private static DuplexChannelFactory<IChatingService> _chanelFactory;
        public WebCamCapture webcam;
        bool camera= false;
        private System.Windows.Controls.Image _FrameImage;
        private int FrameNumber = 70;
        public string roomID, Username;
        private object _locker = new object();
        public  void InitializeWebCam(ref System.Windows.Controls.Image ImageControl, string username, string roomId)
        {
            webcam = new WebCamCapture();
          
                
                webcam.FrameNumber = ((ulong)(0ul));
                webcam.TimeToCapture_milliseconds = FrameNumber;
                webcam.ImageCaptured += (sender, args) =>
                {
                    Task.Factory.StartNew(() => { lock (_locker)  webcam_ImageCaptured(sender, args); });
                    
                };

                _FrameImage = ImageControl;

            
            _chanelFactory = new DuplexChannelFactory<IChatingService>(new CallBack(), "ChatingServicesEndPoint");
            Server = _chanelFactory.CreateChannel();
            roomID = roomId;
            Username = username;
        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
         //   _FrameImage.Source = Helper.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage);

            Server.SendCamera(Username, imageToByteArray(e.WebCamImage), roomID);
            //_FrameImage = e.WebCamImage;
        Console.WriteLine("\n" + Username + " Is Sending Camera with room  id  " + roomID);
  
        }

        public  static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        public void Start()
        {
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.Start(0);
        }

        public void Stop()
        {
            webcam.Stop();
        }
       

        public void Continue()
        {
            // change the capture time frame
            webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            webcam.Start(this.webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            webcam.Config();
        }

        public void AdvanceSetting()
        {
            webcam.Config2();
        }

    }
}
