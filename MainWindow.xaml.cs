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
using System.Windows.Threading;

namespace ScoreCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timerGameTime;
        TimeSpan gameTime;

        public MainWindow()
        {
            InitializeComponent();

            setGameTime(TimeSpan.FromMinutes(5));
        }

        private void Button_Click300(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(3));
        }

        private void Button_Click500(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(5));
        }

        private void Button_Click700(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(7));
        }

        private void Button_Click1000(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(10));
        }

        private void Button_Click2000(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(20));
        }

        private void Button_Click2500(object sender, RoutedEventArgs e)
        {
            setGameTime(TimeSpan.FromMinutes(25));
        }

        private void setGameTime(TimeSpan timeSpan)
        {
            gameTime = timeSpan;
            updateGametimeLabel();
        }

        private void updateGametimeLabel()
        {
            labelGameTime.Content = String.Format("{0,2:00}:{1,2:00}", gameTime.Minutes, gameTime.Seconds);
        }

        private void Button_ClickStart(object sender, RoutedEventArgs e)
        {
            timerGameTime = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Normal, delegate
            {
                updateGametimeLabel();
                if (gameTime == TimeSpan.Zero)
                {
                    timerGameTime.Stop();
                }
                gameTime = gameTime.Add(TimeSpan.FromMilliseconds(-100));
            }, Application.Current.Dispatcher);

            timerGameTime.Start();
        }
    }
}
