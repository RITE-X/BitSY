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

namespace BitSY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //BitmapSource s = new BitmapImage(new Uri(@"C:\Users\olego\Desktop\Расписание звонков.jpg"));
            //suka.Source = s;
        }




        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

   

        private void TopMenu_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();//TODO FIX
        }

        private void KeyText_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
         
        }
    }
}
