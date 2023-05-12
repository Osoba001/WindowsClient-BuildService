using System;
using System.Windows;

namespace WindowsClientBuildSelfService.Share.Messages
{
    public class ShowMessageOnMessageBox : IMessages
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
