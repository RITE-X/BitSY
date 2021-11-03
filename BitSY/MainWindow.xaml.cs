using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace BitSY
{
    public partial class MainWindow : Window
    {
        private string _password;
        private byte[] _targetImageInBytes;

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

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void SaveImage_OnClick(object sender, RoutedEventArgs e)
        {
            if (_targetImageInBytes is null)
            {
                MessageBox.Show("Изображение отсутствует!", "BitSY", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName + ".bmp", _targetImageInBytes);
            }
        }

        private void SaveKey_OnClick(object sender, RoutedEventArgs e)
        {
            if (_password is null)
            {
                MessageBox.Show("Ключ еще не был сгенерирован!", "BitSY", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName + ".key", _password);
            }
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image file (*.bmp) | *.bmp"
            };

            if (openFileDialog.ShowDialog() != true) return;

            TargetImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            _targetImageInBytes = File.ReadAllBytes(openFileDialog.FileName);
        }

        private int AvailableSpace(int byteCount)
        {
            return (byteCount * 2 / 8) - 58;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetMode(Mode.Encryption);
        }

        private void EncryptMode_OnClick(object sender, RoutedEventArgs e)
        {
            SetMode(Mode.Encryption);
        }

        private void DEncryptMode_OnClick(object sender, RoutedEventArgs e)
        {
            SetMode(Mode.Decryption);
        }

        private void SetMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.Encryption:
                    DecryptionButton.IsEnabled = false;
                    EncryptionButton.IsEnabled = true;
                    TargetText.IsReadOnly = false;
                    break;
                case Mode.Decryption:
                    DecryptionButton.IsEnabled = true;
                    EncryptionButton.IsEnabled = false;
                    TargetText.IsReadOnly = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }


        private void EncryptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_targetImageInBytes is null)
            {
                MessageBox.Show("Изображение отсутствует!", "BitSY", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var userText =
                new TextRange(TargetText.Document.ContentStart, TargetText.Document.ContentEnd).Text[..^2]; //For removing \r\n

            if (string.IsNullOrEmpty(userText))
            {
                MessageBox.Show("Текст для шифрования отсутствует!", "BitSY", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }


            if (string.IsNullOrEmpty(KeyText.Text))
            {
                MessageBox.Show("Ключ шифрования отсутствует!", "BitSY", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _password = KeyText.Text;

            var availableSpace = AvailableSpace(_targetImageInBytes.Length);

            if (userText.Length > availableSpace)//fix
            {
                MessageBox.Show(
                    $"Количество символов превышает допустимое значение {availableSpace} байт, к текущему изображению",
                    "BitSY", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //var bitesCount = (userText.Length+ (userText.Length%16==0? 16: 16- (userText.Length%16))) * 8;

            var encryptedData = StringCipher.EncryptToBytes(userText, _password);

            var encryptedBits = Convert.BytesToBits(encryptedData);

            Stenographer.WriteTextLenghtToBitmap(ref _targetImageInBytes, encryptedBits.Length);

            Stenographer.WriteBitsInBitmap(encryptedBits, ref _targetImageInBytes);


          


            MessageBox.Show($"Информация зашифрована! Для того что бы сохранить данные, нажмите файл->сохранить..",
                "BitSY", MessageBoxButton.OK, MessageBoxImage.Information);
            TargetText.Document.Blocks.Clear();
        }

        private void DecryptionButton_Click(object sender, RoutedEventArgs e)
        {
            var password = KeyText.Text;

            if (password is not { Length: > 0 })
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Key file (*.key) | *.key"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    _password = File.ReadAllText(openFileDialog.FileName);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _password = password;
            }

            var encryptedImage = Stenographer.ReadBitsFromBitmap(_targetImageInBytes, Stenographer.ReadTextLenghtFromBitmap(_targetImageInBytes));
            var encryptedText = Convert.BitsToBytes(encryptedImage);
            var encryptedData = StringCipher.DecryptFromBytes(encryptedText, _password);

            TargetText.Document.Blocks.Clear();
            TargetText.Document.Blocks.Add(new Paragraph(new Run(encryptedData)));
        }
    }
}