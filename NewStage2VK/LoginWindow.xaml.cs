using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

using NewStage2VK.View;
using NewStage2VK.ViewModel;

namespace NewStage2VK
{
    public partial class LoginWindow : Window, ILoginView
    {
        public LoginWindow()
        {
            InitializeComponent();
            browser.LoadCompleted += browser_LoadCompleted;
        }

        void browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            OnPageLoad(new PageLoadEventArgs(browser.Source));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region ILoginView implementation

        public void OpenLoginPage(string url)
        {
            browser.Navigate(url);
            ShowDialog();
        }

        void ILoginView.Close()
        {
            base.Hide();
        }

        public event EventHandler<PageLoadEventArgs> PageLoad;

        protected virtual void OnPageLoad(PageLoadEventArgs args)
        {
            if (PageLoad != null)
            {
                PageLoad(this, args);
            }
        }

        #endregion
    }
}

