using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BitSY.Commands;
using BitSY.Models;
using BitSY.Services;
using BitSY.ViewModels.Base;
using Convert = BitSY.Models.Convert;

namespace BitSY.ViewModels.View;

public class MainWindowViewModel : ViewModel
{
    public BitmapStenographer BitmapStenographer { get; }


    #region ImageSource

    private ImageSource? _imageSource;

    public ImageSource? ImageSource
    {
        get => _imageSource;
        set => Set(ref _imageSource, value);
    }

    #endregion

    #region Key

    private string? _key;

    public string? Key
    {
        get => _key;
        set => Set(ref _key, value);
    }

    #endregion

    #region TargetText

    private string? _targetText;

    public string? TargetText
    {
        get => _targetText;
        set => Set(ref _targetText, value);
    }

    #endregion

    public MainWindowViewModel()
    {
        BitmapStenographer = new BitmapStenographer(54);
    }

    #region OpenImageCommand

    private RoutedCommand? _openImageCommand;

    public RoutedCommand OpenImageCommand => _openImageCommand ??= new RoutedCommand(o =>
    {
        const string filesFilter = "Image file (*.bmp) | *.bmp";

        var path = DialogService.OpenFileDialog(filesFilter); //TODO проверка на нал

        if (path is null)
            return;

        ImageSource = new BitmapImage(new Uri(path));

        BitmapStenographer.BitmapSource = File.ReadAllBytes(path);
    });

    #endregion


    #region SaveImageCommand

    private RoutedCommand? _saveImageCommand;

    public RoutedCommand SaveImageCommand => _saveImageCommand ??= new RoutedCommand(o =>
        {
            var filename = DialogService.SaveFileDialog();

            if (filename is null)
                return;

            File.WriteAllBytes(filename + ".bmp", BitmapStenographer.BitmapSource); //TODO проверка на нал
        },
        _ => ImageSource is not null && BitmapStenographer.BitmapSource is {Length: > 0} //TODO проверить
    );

    #endregion


    #region SaveKeyCommand

    private RoutedCommand? _saveKeyCommand;

    public RoutedCommand SaveKeyCommand => _saveKeyCommand ??= new RoutedCommand(o =>
        {
            var filename = DialogService.SaveFileDialog();

            if (filename is null)
                return;

            File.WriteAllText(filename + ".key", Key);
        },
        _ => Key is {Length: > 0}
    );

    #endregion

    #region EncryptImageCommand

    private RoutedCommand? _encryptImageCommand;

    public RoutedCommand EncryptImageCommand => _encryptImageCommand ??= new RoutedCommand(o =>
        {
            var encryptedData = StringCipher.EncryptToBytes(TargetText, Key);

            var encryptedBits = Convert.BytesToBits(encryptedData);

            BitmapStenographer.WriteTextLenghtToBitmap(encryptedBits.Length);

            BitmapStenographer.WriteBitsInBitmap(encryptedBits);

            MessageBox.Show($"Информация зашифрована! Для того что бы сохранить данные, нажмите файл->сохранить..",
                "BitSY", MessageBoxButton.OK, MessageBoxImage.Information);
        },
        _ => ImageSource is not null
             && BitmapStenographer.BitmapSource is {Length: > 0}
             && !string.IsNullOrEmpty(TargetText)
             && !string.IsNullOrEmpty(Key));

    #endregion


    #region DecryptImageCommand

    private RoutedCommand? _decryptImageCommand;

    public RoutedCommand DecryptImageCommand => _decryptImageCommand ??= new RoutedCommand(o =>
        {
            if (Key is not {Length: > 0})
            {
                var path = DialogService.OpenFileDialog("Key file (*.key) | *.key");
                if (path is null)
                    return;

                Key = File.ReadAllText(path);
            }

            var textLenght = BitmapStenographer.ReadTextLenghtFromBitmap();

            byte[] encryptedText;
            try
            {
                var encryptedImage = BitmapStenographer.ReadBitsFromBitmap(textLenght);
                encryptedText = Convert.BitsToBytes(encryptedImage);
            }
            catch (Exception)
            {
                MessageBox.Show($"Невозможно считать длину!", "BitSY", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }


            string encryptedData;

            try
            {
                encryptedData = StringCipher.DecryptFromBytes(encryptedText, Key);
            }
            catch (CryptographicException)
            {
                MessageBox.Show($"Изображение невозможно расшифровать!", "BitSY", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            TargetText = encryptedData;
        },
        o => ImageSource is not null
             && BitmapStenographer.BitmapSource is {Length: > 0});

    #endregion


    #region CloseMainWindowCommand //TODO fix that trash solution

    private RoutedCommand? _closeMainWindowCommand;

    public RoutedCommand CloseMainWindowCommand => _closeMainWindowCommand ??= new RoutedCommand(
        o => { Application.Current.Shutdown(); }
    );

    #endregion


    #region MinimazeMainWindowCommand //TODO fix that trash solution

    private RoutedCommand? _minimazeMainWindowCommand;

    public RoutedCommand MinimazeMainWindowCommand => _minimazeMainWindowCommand ??= new RoutedCommand(
        o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; }
    );

    #endregion


    #region SetEncryptionMode //TODO fix that trash solution

    private RoutedCommand? _setEncryptionMode;

    public RoutedCommand SetEncryptionMode => _setEncryptionMode ??= new RoutedCommand(
        o => { }
    );

    #endregion


    #region SetDecryptionMode //TODO fix that trash solution

    private RoutedCommand? _setDecryptionMode;

    public RoutedCommand SetDecryptionMode => _setDecryptionMode ??= new RoutedCommand(
        o => { TargetText = ""; }
    );

    #endregion
};