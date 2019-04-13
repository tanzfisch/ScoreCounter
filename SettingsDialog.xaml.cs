using System.Windows;
using System;
using System.Windows.Media;

namespace ScoreCounter
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();

            textBoxGameTime.Text = MainWindow.settings.gameTime.TotalSeconds.ToString();
            textBoxBreakTime.Text = MainWindow.settings.breakTime.TotalSeconds.ToString();
            textBoxTimeoutTime.Text = MainWindow.settings.timeoutTime.TotalSeconds.ToString();
        }

        private void Button_ClickOk(object sender, RoutedEventArgs e)
        {
            int value = 0;
            if(!int.TryParse(textBoxGameTime.Text, out value))
            {
                textBoxGameTime.Background = Brushes.Red;
                return;
            }
            MainWindow.settings.gameTime = TimeSpan.FromSeconds(value);

            if (!int.TryParse(textBoxBreakTime.Text, out value))
            {
                textBoxBreakTime.Background = Brushes.Red;
                return;
            }
            MainWindow.settings.breakTime = TimeSpan.FromSeconds(value);

            if (!int.TryParse(textBoxTimeoutTime.Text, out value))
            {
                textBoxTimeoutTime.Background = Brushes.Red;
                return;
            }
            MainWindow.settings.timeoutTime = TimeSpan.FromSeconds(value);

            Close();
        }

        private void Button_ClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
