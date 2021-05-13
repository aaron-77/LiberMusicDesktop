using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using Vlc.DotNet.Wpf;

namespace LiberMusicDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DirectoryInfo vlcLibDirectory;
        private VlcControl control;

        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            // Default installation path of VideoLAN.LibVLC.Windows
            vlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.control?.Dispose();
            base.OnClosing(e);
        }

        private void OnPlayButtonClick(object sender, RoutedEventArgs e)
        {
            this.control?.Dispose();
            this.control = new VlcControl();
            this.ControlContainer.Content = this.control;
            this.control.SourceProvider.CreatePlayer(this.vlcLibDirectory);
           
            // This can also be called before EndInit
            this.control.SourceProvider.MediaPlayer.Log += (_, args) =>
            {
                string message = $"libVlc : {args.Level} {args.Message} @ {args.Module}";
                System.Diagnostics.Debug.WriteLine(message);
            };

            control.SourceProvider.MediaPlayer.Play(new Uri("http://192.168.100.51:4001/streaming/artistas/dreamtheater/whendreamanddayunite/dash/index.mpd"));
            this.botonPausaResumen.Content = "pausar";
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            //this.control?.Dispose();
            //this.control = null;
            if (this.control.SourceProvider.MediaPlayer.IsPlaying())
            {
                this.control.SourceProvider.MediaPlayer.SetPause(true);
                this.botonPausaResumen.Content = "reanudar";
            }
            else {
                this.control.SourceProvider.MediaPlayer.SetPause(false);
                this.botonPausaResumen.Content = "pausar";

            }
            
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            if(this.control == null)
            {
                return;
            }

            this.control.SourceProvider.MediaPlayer.Rate = 2;
        }

        private void GetLength_Click(object sender, RoutedEventArgs e)
        {
            if (this.control == null)
            {
                return;
            }

            GetLength.Content = this.control.SourceProvider.MediaPlayer.Length + " ms";
        }

        private void GetCurrentTime_Click(object sender, RoutedEventArgs e)
        {
            if (this.control == null)
            {
                return;
            }

            GetCurrentTime.Content = this.control.SourceProvider.MediaPlayer.Time + " ms";
        }

        private void SetCurrentTime_Click(object sender, RoutedEventArgs e)
        {
            if (this.control == null)
            {
                return;
            }

            this.control.SourceProvider.MediaPlayer.Time = 5000;
            SetCurrentTime.Content = this.control.SourceProvider.MediaPlayer.Time + " ms";
        }
    }
}
