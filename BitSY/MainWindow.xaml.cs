using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Microsoft.Win32;

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


        private void SaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            //var saveFileDialog = new SaveFileDialog();
            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    saveFileDialog.FileName;
            //}
        }

        private string _targetImagePath;
        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image file (*.bmp) | *.bmp"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                TargetImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                _targetImagePath = openFileDialog.FileName;
            }
        }

        private void EncryptionButton_Click(object sender, RoutedEventArgs e)
        {

          //  var image = File.ReadAllBytes(_targetImagePath);
          //  var textToEncrypt = new TextRange(TargetText.Document.ContentStart, TargetText.Document.ContentEnd).Text;
          //  var textInBytes = Encoding.ASCII.GetBytes(textToEncrypt);


          //  using (var aesAlg = Aes.Create())
          //  {

          //  }

          //  StringCipher.EncryptToBytes()


          //var textInBites=   Convert.BytesToBits(textInBytes);






          //  //var textInBites = new bool[textInBytes.Length * 8];

          //  //int iterator = 0;
          //  //for (int i = 0; i < textInBytes.Length; i++)
          //  //{
          //  //    var character = Convert.ByteToBits(textInBytes[i]);
          //  //    for (int j = 0; j < 8; j++)
          //  //    {
          //  //        textInBites[iterator] = character[j];
          //  //        iterator++;
          //  //    }
          //  //}

            
          //  iterator = 0;
          //  for (var i = 54; i < image.Length && iterator != textInBites.Length; i++)//TODO считывать по 8 байт
          //  {
          //      var color = Convert.ByteToBits(image[i]);
          //      for (var j = color.Length - 2; j < color.Length; j++)
          //      {
          //          color[j] = textInBites[iterator];
          //          iterator++;
          //      }

          //      image[i] = Convert.BitsToByte(color);
          //  }

          //  File.WriteAllBytes(@"C:\Users\olego\Desktop\hhh.bmp", image);

          //  var s = File.ReadAllBytes(@"C:\Users\olego\Desktop\hhh.bmp");


          //  var myWord = new bool[textInBytes.Length * 8];

        
          //  iterator = 0;
          //  for (var i = 54; i < textInBites.Length /2+54; i++)
          //  {
          //      var color = Convert.ByteToBits(s[i]);
          //      for (var j = color.Length - 2; j < color.Length; j++)
          //      {
          //          myWord[iterator] = color[j];
          //          iterator++;
          //      }
          //  }



          //  byte[] world = new byte[textInBytes.Length];


          //  int g = 0;
          //  for (int i = 0; i < world.Length; i++)
          //  {
          //      var gf = new BitArray(8);
          //      for (int j = 0; j < 8; j++)
          //      {
          //          gf[j] = myWord[g];
          //          g++;
          //      }

          //      world[i] = Convert.BitsToByte(gf);
          //  }

          //  var h = Encoding.ASCII.GetString(world);
        }

        private void DecryptionButton_Click(object sender, RoutedEventArgs e)
        {

        }


     

    }
}
