using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Q3Client
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public static readonly DependencyProperty HubProperty =
            DependencyProperty.Register("Hub", typeof (Hub), typeof (Header));

        private UserConfig userConfig;

        public Header()
        {
            InitializeComponent();

            this.SettingsButton.ToolTip = "Q3 " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            userConfig = DataCache.Load<UserConfig>();

#if DEBUG
            Logo.Content = "D\u00b3";
#endif
        }

        public async void NewQueueClicked(object sender, RoutedEventArgs e)
        {
            var window = new NewQueue(GroupsCache);
            window.Owner = ParentQueueList;
            window.ShowDialog();

            if (!string.IsNullOrWhiteSpace(window.NewQueueName))
            {
                var hub = Hub;
                Dispatcher.InvokeAsync(() => hub.CreateQueue(window.NewQueueName, (string)window.GroupSelector.SelectedValue));
            }
            Win32.IsApplicationActive();
        }

        public Hub Hub {
            get { return (Hub) GetValue(HubProperty); }
            set { SetValue(HubProperty, value);}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ParentQueueList.WindowStateExtended = QueueList.eWindowStateExtended.Closed;
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ParentQueueList.WindowStateExtended = QueueList.eWindowStateExtended.Minimized;
        }

        private QueueList ParentQueueList
        {
            get { return (QueueList) Window.GetWindow(this); }
        }

        public GroupsCache GroupsCache { get; set; }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).DragMove();
            ParentQueueList.AdjustHeight();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsButton.ContextMenu.PlacementTarget = SettingsButton;
            SettingsButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            SettingsButton.ContextMenu.IsOpen = true;
        }

        private void ShowHidden_Click(object sender, RoutedEventArgs e)
        {
            ParentQueueList.RestoreHidden();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            ParentQueueList.Close();
        }

        public bool RunOnWindowsStart => StartupRegistration.IsRegisteredForStartup;

        public bool PersistNewQueueNotifications => userConfig.PersistentNewQueueNotifications;

        public bool MuteNewQueueNotifications => userConfig.MuteNewQueueNotifications;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var shouldBeRegistered = !StartupRegistration.IsRegisteredForStartup;
            StartupRegistration.IsRegisteredForStartup = shouldBeRegistered;
            ((MenuItem)sender).IsChecked = shouldBeRegistered;
        }

        private void PersistNotificationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            userConfig.PersistentNewQueueNotifications = ((MenuItem)sender).IsChecked = !userConfig.PersistentNewQueueNotifications;
            DataCache.Save(userConfig);
        }

        private void MuteNewQueueNotificationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            userConfig.MuteNewQueueNotifications = ((MenuItem)sender).IsChecked = !userConfig.MuteNewQueueNotifications;
            DataCache.Save(userConfig);
        }
    }
}
