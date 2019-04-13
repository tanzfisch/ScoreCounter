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
        private TimeSpan gameTime;
        private TimeSpan resetGameTime;
        private DispatcherTimer timerBreakTime;
        private TimeSpan breakTime;
        private TimeSpan resetBreakTime;

        private uint pointsTeam1;
        private uint pointsTeam2;

        public MainWindow()
        {
            InitializeComponent();
            initTimer();

            setResetGameTime(TimeSpan.FromMinutes(5));
            setGameTime(resetGameTime);
            setResetBreakTime(TimeSpan.FromMinutes(2));
            setBreakTime(resetBreakTime);

            initLogos();
        }

        private void setResetBreakTime(TimeSpan timeSpan)
        {
            resetBreakTime = timeSpan;
            buttonResetBreakTime.Content = String.Format("Reset\n{0,2:00}:{1,2:00}", resetBreakTime.Minutes, resetBreakTime.Seconds);
        }

        private void setResetGameTime(TimeSpan timeSpan)
        {
            resetGameTime = timeSpan;
            buttonResetGameTime.Content = String.Format("Reset\n{0,2:00}:{1,2:00}", resetGameTime.Minutes, resetGameTime.Seconds);
        }

        private void initTimer()
        {
            timerGameTime = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Normal, delegate
            {
                updateGameTimeLabel();

                if (gameTime == TimeSpan.FromMinutes(1))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\1min.wav");
                    player.Play();
                }

                if (gameTime == TimeSpan.Zero)
                {
                    stopGameTime();
                    setBreakTime(resetBreakTime);
                    startBreakTime();
                }
                gameTime = gameTime.Add(TimeSpan.FromMilliseconds(-100));
            }, Application.Current.Dispatcher);

            timerGameTime.Stop();

            timerBreakTime = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Normal, delegate
            {
                updateBreakTimeLabel();

                if (breakTime == TimeSpan.FromMinutes(1))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\1min.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromMinutes(2))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\2min.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(30))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\30sec.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(20))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\20sec.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(10))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\10sec.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(3))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\bepp.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(2))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\bepp.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(1))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\bepp.wav");
                    player.Play();
                }
                else if (breakTime == TimeSpan.Zero)
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"..\sounds\beeeeep.wav");
                    player.Play();

                    stopBreakTime();
                    startGameTime();
                }
                breakTime = breakTime.Add(TimeSpan.FromMilliseconds(-100));
            }, Application.Current.Dispatcher);

            timerBreakTime.Stop();
        }

        private void initLogos()
        {
            LogoLowerLeft.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoLowerLeft.png", UriKind.Absolute));
            LogoLowerRight.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoLowerRight.png", UriKind.Absolute));
            LogoUpperLeft.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoUpperLeft.png", UriKind.Absolute));
            LogoUpperRight.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoUpperRight.png", UriKind.Absolute));
        }

        private void Button_Click300(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(3));
            setGameTime(resetGameTime);
        }

        private void Button_Click500(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(5));
            setGameTime(resetGameTime);
        }

        private void Button_Click700(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(7));
            setGameTime(resetGameTime);
        }

        private void Button_Click1000(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(10));
            setGameTime(resetGameTime);
        }

        private void Button_Click2000(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(20));
            setGameTime(resetGameTime);
        }

        private void Button_Click2500(object sender, RoutedEventArgs e)
        {
            setResetGameTime(TimeSpan.FromMinutes(25));
            setGameTime(resetGameTime);
        }

        private void setGameTime(TimeSpan timeSpan)
        {
            gameTime = timeSpan;
            updateGameTimeLabel();
        }

        private void setBreakTime(TimeSpan timeSpan)
        {
            breakTime = timeSpan;
            updateBreakTimeLabel();
        }

        private void updateGameTimeLabel()
        {
            labelGameTime.Content = String.Format("{0,2:00}:{1,2:00}", gameTime.Minutes, gameTime.Seconds);
        }

        private void updateBreakTimeLabel()
        {
            labelBreakTime.Content = String.Format("{0,2:00}:{1,2:00}", breakTime.Minutes, breakTime.Seconds);
        }

        private void Button_ClickStartGameTime(object sender, RoutedEventArgs e)
        {
            if (timerGameTime.IsEnabled)
            {
                stopGameTime();
            }
            else
            {
                startGameTime();
            }
        }

        private void stopGameTime()
        {
            timerGameTime.Stop();
            buttonStartGameTime.Content = "Start";

            setBreakTime(resetBreakTime);
            startBreakTime();
        }

        private void startGameTime()
        {
            stopBreakTime();

            timerGameTime.Start();

            buttonStartGameTime.Content = "Stop";
        }

        private void Button_ClickTeam1IncPoint(object sender, RoutedEventArgs e)
        {
            pointsTeam1++;
            updateScore();
        }

        private void Button_ClickTeam1DecPoint(object sender, RoutedEventArgs e)
        {
            if (pointsTeam1 > 0)
            {
                pointsTeam1--;
            }
            updateScore();
        }

        private void Button_ClickTeam1Zero(object sender, RoutedEventArgs e)
        {
            pointsTeam1 = 0;
            updateScore();
        }

        private void updateScore()
        {
            team1Points.Content = String.Format("{0,2:00}", pointsTeam1);
            team2Points.Content = String.Format("{0,2:00}", pointsTeam2);
        }

        private void Button_ClickTeam2IncPoint(object sender, RoutedEventArgs e)
        {
            pointsTeam2++;
            updateScore();
        }

        private void Button_ClickTeam2DecPoint(object sender, RoutedEventArgs e)
        {
            if (pointsTeam2 > 0)
            {
                pointsTeam2--;
            }
            updateScore();
        }

        private void Button_ClickTeam2Zero(object sender, RoutedEventArgs e)
        {
            pointsTeam2 = 0;
            updateScore();
        }

        private void Button_ClickStartBreakTime(object sender, RoutedEventArgs e)
        {
            if (timerBreakTime.IsEnabled)
            {
                stopBreakTime();
            }
            else
            {
                startBreakTime();
            }
        }

        private void stopBreakTime()
        {
            timerBreakTime.Stop();
            buttonStartBreakTime.Content = "Start";
        }

        private void startBreakTime()
        {
            timerBreakTime.Start();
            buttonStartBreakTime.Content = "Stop";
        }

        private void Button_ClickBreakTime200(object sender, RoutedEventArgs e)
        {
            setResetBreakTime(TimeSpan.FromSeconds(120));
            setBreakTime(resetBreakTime);
        }

        private void Button_ClickBreakTime130(object sender, RoutedEventArgs e)
        {
            setResetBreakTime(TimeSpan.FromSeconds(90));
            setBreakTime(resetBreakTime);

        }

        private void Button_ClickBreakTime100(object sender, RoutedEventArgs e)
        {
            setResetBreakTime(TimeSpan.FromSeconds(60));
            setBreakTime(resetBreakTime);

        }

        private void Button_ClickBreakTime030(object sender, RoutedEventArgs e)
        {
            setResetBreakTime(TimeSpan.FromSeconds(30));
            setBreakTime(resetBreakTime);

        }
        private void Button_ClickBreakTime010(object sender, RoutedEventArgs e)
        {
            setResetBreakTime(TimeSpan.FromSeconds(10));
            setBreakTime(resetBreakTime);

        }

        private void Button_ClickResetBreakTime(object sender, RoutedEventArgs e)
        {
            setBreakTime(resetBreakTime);
        }

        private void Button_ClickResetGameTime(object sender, RoutedEventArgs e)
        {
            setGameTime(resetGameTime);
        }

        private void Button_ClickIncMin(object sender, RoutedEventArgs e)
        {
            gameTime = gameTime.Add(TimeSpan.FromMinutes(1));
            updateGameTimeLabel();
        }

        private void Button_ClickIncSec(object sender, RoutedEventArgs e)
        {
            gameTime = gameTime.Add(TimeSpan.FromSeconds(1));
            updateGameTimeLabel();
        }

        private void Button_ClickDecMin(object sender, RoutedEventArgs e)
        {
            gameTime = gameTime.Subtract(TimeSpan.FromMinutes(1));
            if (gameTime < TimeSpan.Zero)
            {
                gameTime = TimeSpan.Zero;
            }
            updateGameTimeLabel();
        }

        private void Button_ClickDecSec(object sender, RoutedEventArgs e)
        {
            gameTime = gameTime.Subtract(TimeSpan.FromSeconds(1));
            if (gameTime < TimeSpan.Zero)
            {
                gameTime = TimeSpan.Zero;
            }
            updateGameTimeLabel();
        }
    }
}
