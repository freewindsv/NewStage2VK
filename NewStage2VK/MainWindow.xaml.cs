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
using System.Windows.Navigation;
using System.Windows.Shapes;

using NewStage2VK.View;
using NewStage2VK.DomainModel;
using NewStage2VK.ViewModel;
using NewStage2VK.DataAccess.DataModel;
using System.Windows.Controls.Primitives;

namespace NewStage2VK
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView, IRemindView, IInviteView
    {
        private string rscUriPrefix;
        private List<Window> childWindows;
        private AboutWindow aboutWnd;
        private CaptchaWindow captchaWindow;
        private bool hasCurrentAction;
        private EventHandler eventMessageCancel;
        private EventHandler profileMessageCancel;
        private EventHandler<StartLoadBaseEventArgs> eventMessageStartLoad;
        private EventHandler<StartLoadBaseEventArgs> profileMessageStartLoad;
        private EventHandler<SendMessagesEventArgs<EventMessage>> eventMessageSend;
        private EventHandler<SendMessagesEventArgs<ProfileMessage>> profileMessageSend;
        private EventHandler<SaveBaseEventArgs> eventMessageSave;
        private EventHandler<SaveBaseEventArgs> profileMessageSave;

        public MainWindow()
        {
            InitializeComponent();
            childWindows = new List<Window>();
            rscUriPrefix = "pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/";
        }

        #region private handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Window wnd in childWindows)
            {
                wnd.Owner = this;
                if (aboutWnd == null && wnd is AboutWindow)
                {
                    aboutWnd = wnd as AboutWindow;
                }
                if (captchaWindow == null && wnd is CaptchaWindow)
                {
                    captchaWindow = wnd as CaptchaWindow;
                }
            }
            ClearLoggedUser();
            OnViewLoaded();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            aboutWnd.Show();
        }

        #endregion

        #region generic grid handlers

        private void GridRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            IProfileSendable dataItem = row.Item as IProfileSendable;
            dataItem.WasSent = !dataItem.WasSent;
            DataGrid grid = FindParent<DataGrid>(row);
            grid.Items.Refresh();
        }

        private void AvatarGridHeader_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridColumnHeader header = sender as DataGridColumnHeader;
            DataGrid grid = FindParent<DataGrid>(header);
            if (grid.Tag == null)
            {
                grid.Tag = false;
            }
            bool value = !(bool)grid.Tag;
            SetWasSentValue(grid, value);
            grid.Tag = value;
        }

        private void SetWasSentValue(DataGrid grid, bool value)
        {
            foreach (var item in grid.Items)
            {
                (item as IProfileSendable).WasSent = value;
            }
            grid.Items.Refresh();
        }

        private void SetIsSendValue(DataGrid grid, bool value)
        {
            foreach (var item in grid.Items)
            {
                (item as IProfileSendable).IsSend = value;
            }
            grid.Items.Refresh();
        }

        #endregion

        #region helper methods

        private ImageSource GetImageSourceFromResource(string resourceName)
        {
            return BitmapFrame.Create(new Uri(rscUriPrefix + resourceName, UriKind.Absolute));
        }

        private T FindParent<T>(DependencyObject obj) where T : DependencyObject
        {
            Type parentType = typeof(T);
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null && parent.GetType() != typeof(DataGrid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (T)parent;
        }

        private void IterateChilds<T>(DependencyObject obj, Action<T> cb) where T : DependencyObject
        {
            Type childType = typeof(T);
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject currObj = VisualTreeHelper.GetChild(obj, i);
                if (currObj is T)
                {
                    cb(currObj as T);
                }
                IterateChilds<T>(currObj, cb);
            }
        }

        private void SetCancelButtonState(Button btn)
        {
            btn.IsEnabled = true;
            btn.Tag = btn.Content;
            btn.Content = "Отменить";
        }

        private void SetNormalButtonState(Button btn)
        {
            btn.IsEnabled = true;
            btn.Content = btn.Tag;
        }

        #endregion

        #region validate methods

        private bool ValidateUsersCountWithError<T>(IList<T> list)
        {
            if (list.Count > 0)
            {
                return true;
            }
            ShowWarning("Не выбраны пользователи для рассылки");
            return false;
        }

        private bool ValidateTextBox(TextBox textBox)
        {
            BindingExpression be = textBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
            if (!be.HasError)
            {
                return true;
            }
            ShowWarning(be.ValidationError.ErrorContent as string);
            return false;
        }

        #endregion

        #region MainWindow own members

        public void AddChildWindow(Window wnd)
        {
            childWindows.Add(wnd);
        }

        #endregion

        #region IView implementation

        public void RunOnUI(Delegate deleg, params object[] args)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(deleg, args);
            }
        }

        #endregion

        #region IMainView implementation

        public void ClearLoggedUser()
        {
            lblName.Content = string.Empty;
            hlTextBlock.Visibility = System.Windows.Visibility.Hidden;
            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(imgAvatar, null);
        }

        public void ShowLoggedUser(string name, byte[] imageBytes)
        {
            lblName.Content = name;
            hlTextBlock.Visibility = System.Windows.Visibility.Visible;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = new System.IO.MemoryStream(imageBytes);
            bitmap.EndInit();
            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(imgAvatar, bitmap);
        }

        public void ShowLoadingAvatar()
        {
            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(imgAvatar, GetImageSourceFromResource("Images/loading.gif"));
        }

        public void ShowAccessFailMessage(string message)
        {
            MessageBox.Show(message, "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void IMainView.Close()
        {
            base.Close();
        }

        public event EventHandler ViewLoaded;

        protected virtual void OnViewLoaded()
        {
            if (ViewLoaded != null)
            {
                ViewLoaded(this, EventArgs.Empty);
            }
        }

        public event EventHandler UserExit;

        protected virtual void OnUserExit()
        {
            if (UserExit != null)
            {
                UserExit(this, EventArgs.Empty);
            }
        }

        void IMainView.DisableAllActions()
        {
            DisableRemindControls();
            DisableInviteControls();
        }

        void IMainView.EnableAllActions()
        {
            EnableRemindControls();
            EnableInviteControls();
        }

        #endregion

        #region IMainView private methods

        private void hlExit_Click(object sender, RoutedEventArgs e)
        {
            OnUserExit();
        }

        #endregion

        #region IStatusView implementation

        public void SetStatusText(string text)
        {
            lblStatus.Content = text;
        }

        public void InitProgress(int maxValue)
        {
            pbStatus.Minimum = 0;
            pbStatus.Maximum = maxValue;
        }

        public void SetProgressValue(int progress)
        {
            pbStatus.Value = progress;
        }

        public void ResetProgress()
        {
            SetProgressValue(0);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public string ShowCaptcha(byte[] img, int showingInterval)
        {
            captchaWindow.SetCaptcha(img, showingInterval);
            captchaWindow.ShowDialog();
            return captchaWindow.GetCaptchaKey();
        }

        #endregion

        #region ISenderView reminds implementations

        void ISenderView<EventMessage>.ClearGrid()
        {
            gridEventMessages.Items.Clear();
        }

        void ISenderView<EventMessage>.AddItemsToGrid(IList<EventMessage> eventMessages)
        {
            foreach (EventMessage msg in eventMessages)
            {
                gridEventMessages.Items.Add(msg);
            }
        }

        void ISenderView<EventMessage>.UpdateGridModel()
        {
            gridEventMessages.Items.Refresh();
        }

        void ISenderView<EventMessage>.SetMessage(string message)
        {
            txtEvtMessage.Text = message;
        }

        event EventHandler<StartLoadBaseEventArgs> ISenderView<EventMessage>.StartLoad
        {
            add
            {
                eventMessageStartLoad += value;
            }

            remove
            {
                eventMessageStartLoad -= value;
            }
        }

        protected virtual void OnLoadComments(RemindStartLoadEventArgs args)
        {
            if (eventMessageStartLoad != null)
            {
                eventMessageStartLoad(this, args);
            }
        }

        event EventHandler<SendMessagesEventArgs<EventMessage>> ISenderView<EventMessage>.Send
        {
            add
            {
                eventMessageSend += value;
            }

            remove
            {
                eventMessageSend -= value;
            }
        }

        protected virtual void OnSendEventMessage(SendMessagesEventArgs<EventMessage> args)
        {
            if (eventMessageSend != null)
            {
                eventMessageSend(this, args);
            }
        }

        event EventHandler<SaveBaseEventArgs> ISenderView<EventMessage>.Save
        {
            add
            {
                eventMessageSave += value;
            }

            remove
            {
                eventMessageSave -= value;
            }
        }

        protected virtual void OnSaveEventMessages(SaveEventMessagesEventArgs args)
        {
            if (eventMessageSave != null)
            {
                eventMessageSave(this, args);
            }
        }

        void ISenderView<EventMessage>.DisableAllActions()
        {
            DisableRemindControls();
        }

        void ISenderView<EventMessage>.EnableAllActions()
        {
            EnableRemindControls();
        }

        void ISenderView<EventMessage>.EnableCancelAction(ViewCancelActions cancelAction)
        {
            hasCurrentAction = true;

            switch (cancelAction)
            {
                case ViewCancelActions.Load:
                    SetCancelButtonState(btnLoadVisitors);
                    break;
                case ViewCancelActions.Save:
                    SetCancelButtonState(btnEvtMsgSave);
                    break;
                case ViewCancelActions.Send:
                    SetCancelButtonState(btnEvtMsgSend);
                    break;
            }
        }

        void ISenderView<EventMessage>.DisableCancelAction(ViewCancelActions cancelAction)
        {
            hasCurrentAction = false;

            switch (cancelAction)
            {
                case ViewCancelActions.Load:
                    SetNormalButtonState(btnLoadVisitors);
                    break;
                case ViewCancelActions.Save:
                    SetNormalButtonState(btnEvtMsgSave);
                    break;
                case ViewCancelActions.Send:
                    SetNormalButtonState(btnEvtMsgSend);
                    break;
            }
        }

        event EventHandler ISenderView<EventMessage>.CancelAction
        {
            add
            {
                eventMessageCancel += value;
            }

            remove
            {
                eventMessageCancel -= value;
            } 
        }

        protected virtual void OnCancelRemindAction()
        {
            if (eventMessageCancel != null)
            {
                eventMessageCancel(this, EventArgs.Empty);
            }
        }

        #endregion

        #region ISenderView invites implementations

        void ISenderView<ProfileMessage>.ClearGrid()
        {
            gridInvMessages.Items.Clear();
        }

        void ISenderView<ProfileMessage>.AddItemsToGrid(IList<ProfileMessage> profileMessages)
        {
            foreach (ProfileMessage msg in profileMessages)
            {
                gridInvMessages.Items.Add(msg);
            }
        }

        void ISenderView<ProfileMessage>.UpdateGridModel()
        {
            gridInvMessages.Items.Refresh();
        }

        event EventHandler<StartLoadBaseEventArgs> ISenderView<ProfileMessage>.StartLoad
        {
            add
            {
                profileMessageStartLoad += value;
            }

            remove
            {
                profileMessageStartLoad -= value;
            }
        }

        protected virtual void OnLoadUsers(InviteStartLoadEventArgs args)
        {
            if (profileMessageStartLoad != null)
            {
                profileMessageStartLoad(this, args);
            }
        }

        event EventHandler<SendMessagesEventArgs<ProfileMessage>> ISenderView<ProfileMessage>.Send
        {
            add
            {
                profileMessageSend += value;
            }

            remove
            {
                profileMessageSend -= value;
            }
        }

        protected virtual void OnSendInvites(SendMessagesEventArgs<ProfileMessage> args)
        {
            if (profileMessageSend != null)
            {
                profileMessageSend(this, args);
            }
        }

        event EventHandler<SaveBaseEventArgs> ISenderView<ProfileMessage>.Save
        {
            add
            {
                profileMessageSave += value;
            }

            remove
            {
                profileMessageSave -= value;
            }
        }

        protected virtual void OnSaveInvites(SaveInviteEventArgs args)
        {
            if (profileMessageSave != null)
            {
                profileMessageSave(this, args);
            }
        }

        void ISenderView<ProfileMessage>.SetMessage(string message)
        {
            txtInvMessage.Text = message;
        }

        void ISenderView<ProfileMessage>.DisableAllActions()
        {
            DisableInviteControls();
        }

        void ISenderView<ProfileMessage>.EnableAllActions()
        {
            EnableInviteControls();
        }

        void ISenderView<ProfileMessage>.EnableCancelAction(ViewCancelActions cancelAction)
        {
            hasCurrentAction = true;

            switch (cancelAction)
            {
                case ViewCancelActions.Load:
                    SetCancelButtonState(btnLoadUsers);
                    break;
                case ViewCancelActions.Save:
                    SetCancelButtonState(btnInvMsgSave);
                    break;
                case ViewCancelActions.Send:
                    SetCancelButtonState(btnInvMsgSend);
                    break;
            }
        }

        void ISenderView<ProfileMessage>.DisableCancelAction(ViewCancelActions cancelAction)
        {
            hasCurrentAction = false;

            switch (cancelAction)
            {
                case ViewCancelActions.Load:
                    SetNormalButtonState(btnLoadUsers);
                    break;
                case ViewCancelActions.Save:
                    SetNormalButtonState(btnInvMsgSave);
                    break;
                case ViewCancelActions.Send:
                    SetNormalButtonState(btnInvMsgSend);
                    break;
            }
        }

        event EventHandler ISenderView<ProfileMessage>.CancelAction
        {
            add
            {
                profileMessageCancel += value;
            }

            remove
            {
                profileMessageCancel -= value;
            }
        }

        protected virtual void OnCancelInviteAction()
        {
            if (profileMessageCancel != null)
            {
                profileMessageCancel(this, EventArgs.Empty);
            }
        }

        #endregion

        #region IRemindView private methods

        private void btnLoadVisitors_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelRemindAction();
                return;
            }

            if (ValidateTextBox(txtOwnerIdPostId))
            {
                string[] parts = txtOwnerIdPostId.Text.Split('_');
                OnLoadComments(new RemindStartLoadEventArgs(int.Parse(parts[0]), int.Parse(parts[1]), cbEvtUpdateAvatar.IsChecked.Value));
            }
        }

        private void btnEvtMsgSave_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelRemindAction();
                return;
            }

            OnSaveEventMessages(new SaveEventMessagesEventArgs(txtEvtMessage.Text, cbEvtUpdateAvatar.IsChecked.Value));
        }

        private void btnEvtMsgSend_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelRemindAction();
                return;
            }

            if (ValidateTextBox(txtEvtMessage) && ValidateTextBox(txtEvtMsgCount))
            {
                List<EventMessage> list = gridEventMessages.Items.Cast<EventMessage>().ToList();
                if (ValidateUsersCountWithError(list))
                {                    
                    int usersCount = string.IsNullOrEmpty(txtEvtMsgCount.Text) ? 0 : int.Parse(txtEvtMsgCount.Text);
                    OnSendEventMessage(new SendMessagesEventArgs<EventMessage>(list, txtEvtMessage.Text, cbEvtAutoSave.IsChecked.Value, cbEvtUpdateAvatar.IsChecked.Value, usersCount));
                }
            }
        }

        private void GridEvtMsgCbHeader_Checked(object sender, RoutedEventArgs e)
        {
            SetIsSendValue(gridEventMessages, true);
        }

        private void GridEvtMsgCbHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            SetIsSendValue(gridEventMessages, false);
        }

        private void DisableRemindControls()
        {
            IterateChilds<Button>(eventRemindControlsWrapper, x => x.IsEnabled = false);
            tabGroupInvites.IsEnabled = false;
            hlExit.IsEnabled = false; 
        }

        private void EnableRemindControls()
        {
            IterateChilds<Button>(eventRemindControlsWrapper, x => x.IsEnabled = true);
            tabGroupInvites.IsEnabled = true;
            hlExit.IsEnabled = true; 
        }

        #endregion

        #region IInviteView implementation

        public event EventHandler<PresenceFilterEventArgs> PresenceFilterChanged;

        protected virtual void OnPresenceFilterChanged(PresenceFilterEventArgs args)
        {
            if (PresenceFilterChanged != null)
            {
                PresenceFilterChanged(this, args);
            }
        }

        void IInviteView.SetFilterPrecenseType(PresenceType type)
        {
            SetSelectedPresenceType(type);
        }

        #endregion

        #region InviteView private methods

        private void btnLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelInviteAction();
                return;
            }

            OnLoadUsers(new InviteStartLoadEventArgs(cbInvUpdateAvatar.IsChecked.Value, GetSelectedPresenceType()));
        }

        private void comboUserPresence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPresenceFilterChanged(new PresenceFilterEventArgs(GetSelectedPresenceType()));
        }

        private void btnInvMsgSend_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelInviteAction();
                return;
            }

            if (ValidateTextBox(txtInvMessage) && ValidateTextBox(txtInvMsgCount))
            {
                List<ProfileMessage> list = gridInvMessages.Items.Cast<ProfileMessage>().ToList();
                if (ValidateUsersCountWithError(list))
                {
                    int usersCount = string.IsNullOrEmpty(txtInvMsgCount.Text) ? 0 : int.Parse(txtInvMsgCount.Text);
                    OnSendInvites(new SendMessagesEventArgs<ProfileMessage>(list, txtInvMessage.Text, cbEvtAutoSave.IsChecked.Value, cbEvtUpdateAvatar.IsChecked.Value, usersCount));
                }
            }
        }

        private void btnInvMsgSave_Click(object sender, RoutedEventArgs e)
        {
            if (hasCurrentAction)
            {
                OnCancelInviteAction();
                return;
            }

            OnSaveInvites(new SaveInviteEventArgs(txtInvMessage.Text, cbInvUpdateAvatar.IsChecked.Value, GetSelectedPresenceType()));
        }

        private void GridInvMsgCbHeader_Checked(object sender, RoutedEventArgs e)
        {
            SetIsSendValue(gridInvMessages, true);
        }

        private void GridInvMsgCbHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            SetIsSendValue(gridInvMessages, false);
        }

        private PresenceType GetSelectedPresenceType()
        {
            return (PresenceType)int.Parse((string)(comboUserPresence.SelectedItem as ComboBoxItem).Tag);
        }

        private void SetSelectedPresenceType(PresenceType type)
        {
            comboUserPresence.SelectedValue = comboUserPresence.Items.Cast<ComboBoxItem>().FirstOrDefault(x => int.Parse(x.Tag.ToString()) == (int)type);
        }

        private void DisableInviteControls()
        {
            IterateChilds<Button>(invitesControlsWrapper, x => x.IsEnabled = false);
            tabEventRemind.IsEnabled = false;
            comboUserPresence.IsEnabled = false;
            hlExit.IsEnabled = false;
        }

        private void EnableInviteControls()
        {
            IterateChilds<Button>(invitesControlsWrapper, x => x.IsEnabled = true);
            tabEventRemind.IsEnabled = true;
            comboUserPresence.IsEnabled = true;
            hlExit.IsEnabled = true;
        }

        #endregion
    }
}
