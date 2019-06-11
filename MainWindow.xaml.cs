using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ScoreCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Settings
        {
            public TimeSpan gameTime = TimeSpan.FromMinutes(10);
            public TimeSpan breakTime = TimeSpan.FromMinutes(2);
            public TimeSpan timeoutTime = TimeSpan.FromMinutes(1);
        }

        private DispatcherTimer timerGameTime;
        private TimeSpan gameTime;
        private TimeSpan resetGameTime;
        private DispatcherTimer timerBreakTime;
        private TimeSpan breakTime;
        private TimeSpan resetBreakTime;

        private uint pointsTeam1;
        private uint pointsTeam2;

        private System.Media.SoundPlayer sound1min_warning = new System.Media.SoundPlayer(@"..\sounds\1min_warning.wav");
        private System.Media.SoundPlayer sound30sec = new System.Media.SoundPlayer(@"..\sounds\30sec.wav");
        private System.Media.SoundPlayer sound20sec = new System.Media.SoundPlayer(@"..\sounds\20sec.wav");
        private System.Media.SoundPlayer sound10sec = new System.Media.SoundPlayer(@"..\sounds\10sec.wav");
        private System.Media.SoundPlayer sound10 = new System.Media.SoundPlayer(@"..\sounds\10.wav");
        private System.Media.SoundPlayer sound9 = new System.Media.SoundPlayer(@"..\sounds\9.wav");
        private System.Media.SoundPlayer sound8 = new System.Media.SoundPlayer(@"..\sounds\8.wav");
        private System.Media.SoundPlayer sound7 = new System.Media.SoundPlayer(@"..\sounds\7.wav");
        private System.Media.SoundPlayer sound6 = new System.Media.SoundPlayer(@"..\sounds\6.wav");
        private System.Media.SoundPlayer sound5 = new System.Media.SoundPlayer(@"..\sounds\5.wav");
        private System.Media.SoundPlayer sound4 = new System.Media.SoundPlayer(@"..\sounds\4.wav");
        private System.Media.SoundPlayer sound3 = new System.Media.SoundPlayer(@"..\sounds\3.wav");
        private System.Media.SoundPlayer sound2 = new System.Media.SoundPlayer(@"..\sounds\2.wav");
        private System.Media.SoundPlayer sound1 = new System.Media.SoundPlayer(@"..\sounds\1.wav");
        private System.Media.SoundPlayer soundBeeeep = new System.Media.SoundPlayer(@"..\sounds\beeeeep.wav");
        private System.Media.SoundPlayer soundBepp = new System.Media.SoundPlayer(@"..\sounds\bepp.wav");
        private System.Media.SoundPlayer sound1min = new System.Media.SoundPlayer(@"..\sounds\1min.wav");
        private System.Media.SoundPlayer sound2min = new System.Media.SoundPlayer(@"..\sounds\2min.wav");
        private System.Media.SoundPlayer soundTimeout = new System.Media.SoundPlayer(@"..\sounds\timeout.wav");
        private System.Media.SoundPlayer soundTowel = new System.Media.SoundPlayer(@"..\sounds\towel.wav");

        static public Settings settings = new Settings();

        public MainWindow()
        {
            InitializeComponent();

            initTimer();
            initLogos();

            readSettings();
            updateFromSettings();
        }

        private void readSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            int value = 0;
            if (int.TryParse(appSettings["GameTime"], out value))
            {
                settings.gameTime = TimeSpan.FromSeconds(value);
            }
            if (int.TryParse(appSettings["BreakTime"], out value))
            {
                settings.breakTime = TimeSpan.FromSeconds(value);
            }
            if (int.TryParse(appSettings["TimeoutTime"], out value))
            {
                settings.timeoutTime = TimeSpan.FromSeconds(value);
            }
        }

        private void updateFromSettings()
        {
            setResetGameTime(settings.gameTime);
            setResetBreakTime(settings.breakTime);

            if (!timerBreakTime.IsEnabled && !timerGameTime.IsEnabled)
            {
                setGameTime(resetGameTime);
                setBreakTime(resetBreakTime);
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void writeSettings()
        {
            AddUpdateAppSettings("GameTime", settings.gameTime.TotalSeconds.ToString());
            AddUpdateAppSettings("BreakTime", settings.breakTime.TotalSeconds.ToString());
            AddUpdateAppSettings("TimeoutTime", settings.timeoutTime.TotalSeconds.ToString());
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
                    sound1min_warning.Play();
                }
                else if (gameTime == TimeSpan.FromSeconds(30))
                {
                    sound30sec.Play();
                }
                else if (gameTime == TimeSpan.FromSeconds(3))
                {
                    soundBepp.Play();
                }
                else if (gameTime == TimeSpan.FromSeconds(2))
                {
                    soundBepp.Play();
                }
                else if (gameTime == TimeSpan.FromSeconds(1))
                {
                    soundBepp.Play();
                }

                if (gameTime == TimeSpan.Zero)
                {
                    soundBeeeep.Play();

                    stopGameTime();
                    setBreakTime(resetBreakTime);

                    buttonNextPoint.IsEnabled = false;
                    buttonStartBreakTime.IsEnabled = true;
                }
                gameTime = gameTime.Add(TimeSpan.FromMilliseconds(-100));
            }, Application.Current.Dispatcher);

            timerGameTime.Stop();

            timerBreakTime = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Normal, delegate
            {
                updateBreakTimeLabel();

                if (breakTime > TimeSpan.FromMinutes(1))
                {
                    buttonTeam1Timeout.IsEnabled = true;
                    buttonTeam2Timeout.IsEnabled = true;
                }
                else
                {
                    buttonTeam1Timeout.IsEnabled = false;
                    buttonTeam2Timeout.IsEnabled = false;
                }

                if (breakTime == TimeSpan.FromMinutes(1))
                {
                    sound1min.Play();
                }
                else if (breakTime == TimeSpan.FromMinutes(2))
                {
                    sound2min.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(30))
                {
                    sound30sec.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(20))
                {
                    sound20sec.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(10))
                {
                    sound10sec.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(3))
                {
                    soundBepp.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(2))
                {
                    soundBepp.Play();
                }
                else if (breakTime == TimeSpan.FromSeconds(1))
                {
                    soundBepp.Play();
                }
                else if (breakTime == TimeSpan.Zero)
                {
                    soundBeeeep.Play();

                    stopBreakTime();
                    startGameTime();
                }
                breakTime = breakTime.Add(TimeSpan.FromMilliseconds(-100));
            }, Application.Current.Dispatcher);

            timerBreakTime.Stop();
        }

        private void initLogos()
        {
            try
            {
                LogoLowerLeft.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoLowerLeft.png", UriKind.Absolute));
            }
            catch { }

            try
            {
                LogoLowerRight.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoLowerRight.png", UriKind.Absolute));
            }
            catch { }

            try
            {
                LogoUpperLeft.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoUpperLeft.png", UriKind.Absolute));
            }
            catch { }

            try
            {
                LogoUpperRight.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"..\logos\LogoUpperRight.png", UriKind.Absolute));
            }
            catch { }
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
                buttonNextPoint.IsEnabled = true;
            }
            else
            {
                startGameTime();
                buttonNextPoint.IsEnabled = false;
            }
        }

        private void stopGameTime()
        {
            timerGameTime.Stop();
            buttonStartGameTime.Content = "Start";
            buttonTeam1Towel.IsEnabled = false;
            buttonTeam2Towel.IsEnabled = false;
        }

        private void startGameTime()
        {
            stopBreakTime();

            timerGameTime.Start();

            buttonStartGameTime.Content = "Stop";
            buttonStartBreakTime.IsEnabled = false;
            buttonTeam1Towel.IsEnabled = true;
            buttonTeam2Towel.IsEnabled = true;
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
            buttonTeam1Timeout.IsEnabled = false;
            buttonTeam2Timeout.IsEnabled = false;
            buttonStartGameTime.IsEnabled = true;
        }

        private void startBreakTime()
        {
            timerBreakTime.Start();
            buttonStartBreakTime.Content = "Stop";
            buttonTeam1Timeout.IsEnabled = true;
            buttonTeam2Timeout.IsEnabled = true;
            buttonStartGameTime.IsEnabled = false;
        }

        private void Button_ClickBreakTime200(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(120));
        }

        private void Button_ClickBreakTime130(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(90));
        }

        private void Button_ClickBreakTime100(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(60));
        }

        private void Button_ClickBreakTime030(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(30));
        }
        private void Button_ClickBreakTime020(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(20));
        }

        private void Button_ClickBreakTime010(object sender, RoutedEventArgs e)
        {
            setBreakTime(TimeSpan.FromSeconds(10));
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

        private void Button_ClickTeam1TimeOut(object sender, RoutedEventArgs e)
        {
            if (timerBreakTime.IsEnabled &&
                breakTime >= TimeSpan.FromMinutes(1))
            {
                breakTime = breakTime.Add(settings.timeoutTime);
                soundTimeout.Play();
            }
        }

        private void Button_ClickTeam2TimeOut(object sender, RoutedEventArgs e)
        {
            if (timerBreakTime.IsEnabled &&
                breakTime >= TimeSpan.FromMinutes(1))
            {
                breakTime = breakTime.Add(settings.timeoutTime);
                soundTimeout.Play();
            }
        }

        private void Button_ClickTeam1Towel(object sender, RoutedEventArgs e)
        {
            stopGameTime();
            buttonNextPoint.IsEnabled = true;            
            soundTowel.Play();
        }

        private void Button_ClickTeam2Towel(object sender, RoutedEventArgs e)
        {
            stopGameTime();
            buttonNextPoint.IsEnabled = true;
            soundTowel.Play();
        }

        private void Button_ClickOpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.Closed += Dialog_Closed;
            dialog.ShowDialog();
        }

        private void Dialog_Closed(object sender, EventArgs e)
        {
            updateFromSettings();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            writeSettings();
        }

        private void Button_ClickNextPoint(object sender, RoutedEventArgs e)
        {
            setBreakTime(resetBreakTime);
            startBreakTime();

            buttonNextPoint.IsEnabled = false;
            buttonStartBreakTime.IsEnabled = true;
        }



    }
}
