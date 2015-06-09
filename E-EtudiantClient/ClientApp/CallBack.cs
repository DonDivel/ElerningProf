using ChatingInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    [CallbackBehavior (ConcurrencyMode= ConcurrencyMode.Multiple )  ]
   public class CallBack : Iclient
    {

        public void Getmessage(string message, string username)
        {

           Home.room.takeMessage(message, username);
            
        }



      
        public void GetUpdates(string username, string action)
        {
           
        }

    

        public void AddUsername(string username)
        {
            //((Room)Application.Current.Windows.OfType<Room>()).addUser(username);
            Home.room.addUser(username);
        }

        public void RemoveUsername(string username)
        {
            Home.room.removeUser(username);
        }




        public void UpdateRoomOnlineList(List<string> username)
        {
            throw new NotImplementedException();
        }


        public void GetScreen(byte[] image)
        {
            Home.room.SetScreen(image);
        }


       


        public void GetVoice(byte[] b, int offset, int bsize)
        {
            Home.room.StartLisning(b,offset, bsize);
        }


        public void userRaisedHand(string username)
        {
            Home.room.UserRaisedHand(username);
        }


        public void GetNotification(string notif)
        {
            throw new NotImplementedException();
        }

        public void GetQuistion(string username, string question)
        {
            throw new NotImplementedException();
        }





        public void GetCamera(byte[] image)
        {
            throw new NotImplementedException();
        }


        public void StopLisning()
        {
            Home.room.stopLisning();
        }


        public void updateCoursLists()
        {
            Login.h.updateLists();

            
        }
    }
}
