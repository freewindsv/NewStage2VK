using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NewStage2VK
{
    /// <summary>
    /// Логика взаимодействия для CaptchaWindow.xaml
    /// </summary>
    public partial class CaptchaWindow : Window
    {
        private const string SKIP_BUTTON_NAME = "Пропустить";
        private DispatcherTimer timer;
        private int remainInterval;
 
        public CaptchaWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            remainInterval--;
            btnSkip.Content = $"{SKIP_BUTTON_NAME} [{remainInterval}]";
            if (remainInterval <= 0)
            {
                HideWithCancel();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HideWithCancel();
            e.Cancel = true;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            HideWithSubmit();
        }

        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            HideWithCancel();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWithCancel();
            }
            else if (e.Key == Key.Enter)
            {
                HideWithSubmit();
            }
        }

        public void SetCaptcha(byte[] img, int showingInterval)
        {
            txtCaptchaKey.Text = string.Empty;
            imgCaptcha.Source = LoadImage(img);

            btnSkip.Content = SKIP_BUTTON_NAME;
            if (showingInterval > 0)
            {
                remainInterval = showingInterval;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Start();
            }
        }

        public string GetCaptchaKey()
        {
            return txtCaptchaKey.Text;
        }

        private void HideWithCancel()
        {
            txtCaptchaKey.Text = string.Empty;
            timer.Stop();
            Hide();
        }

        private void HideWithSubmit()
        {
            timer.Stop();
            Hide();
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) 
            {
                return null;
            }
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
