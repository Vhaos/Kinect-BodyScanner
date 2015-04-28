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

namespace BodyScanner
{
    /// <summary>
    /// Interaction logic for GenderWindow.xaml
    /// </summary>
    public partial class GenderWindow : Window
    {
        public enum GenderType { Male, Female };

        public GenderWindow()
        {
            InitializeComponent();
        }

        private void male_btn_Click(object sender, RoutedEventArgs e)
        {
            // pass male as param to Kinect Window
            Log.Write(Log.Tag.INFO, "Male selected as gender.");
            KinectWindow kw = new KinectWindow(GenderType.Male);
            kw.Show();
            this.Close();
        }

        private void female_btn_Click(object sender, RoutedEventArgs e)
        {
            // pass female as param to Kinect Window 
            Log.Write(Log.Tag.INFO, "Female selected as gender.");
            KinectWindow kw = new KinectWindow(GenderType.Female);
            kw.Show();
            this.Close();
        }
    }
}
