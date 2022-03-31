using Microsoft.Win32;

namespace BitSY.Services;

public static class DialogService
{
    public static string? OpenFileDialog(string? filter= null)
    {
        var dialog = filter is null ? new OpenFileDialog() : new OpenFileDialog{Filter = filter};

        return dialog.ShowDialog() is true ? dialog.FileName : null;
    }

    public static string? SaveFileDialog()
    {
        var saveFileDialog = new SaveFileDialog();
        return saveFileDialog.ShowDialog() is true ? saveFileDialog.FileName : null;
    }
}