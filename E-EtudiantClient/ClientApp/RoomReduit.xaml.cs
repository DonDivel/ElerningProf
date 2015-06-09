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
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Logique d'interaction pour RoomReduit.xaml
    /// </summary>
    public partial class RoomReduit : Window
    {
        public RoomReduit()
        {
            InitializeComponent();
        }

        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Home.room.Show();
        }
    }
}
