using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace BitSY
{

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

        private void SaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, _targetImageInBytes);

            }
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image file (*.bmp) | *.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                TargetImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                _targetImageInBytes = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private int AvailableSpace(int byteCount)
        {
            return byteCount * 2 / 8;

        }

        private byte[] Key;
        private byte[] IV;
        private byte[] _targetImageInBytes;

        private string _keyToSave;

        private void EncryptionButton_Click(object sender, RoutedEventArgs e)
        {


            var userText = new TextRange(TargetText.Document.ContentStart, TargetText.Document.ContentEnd).Text[..^2];//For removing \r\n


            var availableSpace = AvailableSpace(_targetImageInBytes.Length);

            if (userText.Length > availableSpace)
            {
                MessageBox.Show($"Количество символов превышает допустимое значение {availableSpace} байт, к текущему изображению", "BitSY", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var aesAlg = Aes.Create())
            {
                Key = aesAlg.Key;
                IV = aesAlg.IV;

                var encyptedData = StringCipher.EncryptToBytes(userText, Key, IV);

                var encyptedBites = Convert.BytesToBits(encyptedData);

                _targetImageInBytes = Stenographer.WriteBitesInBitmap(encyptedBites, _targetImageInBytes);

                //var dencyptedData = StringCipher.DecryptFromBytes(encyptedData, aesAlg.Key, aesAlg.IV);
            }

        }

        private void DecryptionButton_Click(object sender, RoutedEventArgs e)
        {
            var key = new TextRange(KeyText.Document.ContentStart, KeyText.Document.ContentEnd).Text[..^2];

            if (key is { Length: > 0 })
            {
                Regex.Matches(key, "");
            }
            var encyptedImage = Stenographer.ReadBitesFromBitmap(_targetImageInBytes,)
        }

    }
}
