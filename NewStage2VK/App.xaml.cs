using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;

using NewStage2VK.DomainModel;
using NewStage2VK.Presenter;
using System.Windows.Threading;
using NewStage2VK.DomainModel.Crypto;
using NewStage2VK.DataAccess;
using NewStage2VK.Config;

namespace NewStage2VK
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main(string[] args)
        {
            string currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (args.Length > 0)
            {
                ConsoleManager.Show();

                LocalDb localDb = new LocalDb(ConfigurationManager.ConnectionStrings["NewStage2DbConnection"].ConnectionString, currentDir);

                string arg = args[0].ToLower();
                if (arg == "initlocaldb")
                {
                    Console.WriteLine("LocalDb init begin");
                    localDb.InitLocalDB();
                    Console.WriteLine("LocalDb init done");
                }
                else if (arg == "detachlocaldb")
                {
                    Console.WriteLine("LocalDb detach begin");
                    localDb.DetachDatabase();
                    Console.WriteLine("LocalDb detach done");
                }
                else if (arg.EndsWith(".sql"))
                {
                    Console.WriteLine("Applying migration...");

                    if (localDb.ApplyMigration(arg))
                    {
                        Console.WriteLine("Migration success");
                    }
                    else
                    {
                        Console.WriteLine("Sql file not found");
                    }
                }
                else
                {
                    Console.WriteLine("Command not defined");
                }
                Console.WriteLine("Press [Enter] to exit...");
                Console.ReadLine();
                return;
            }

            ILogger logger = new FileLogger(Path.Combine(currentDir, ConfigurationManager.AppSettings["logFileName"]));

            App app = new App();

            #if !DEBUG
            app.DispatcherUnhandledException += (sender, e) =>
            {
                logger.WriteException(e.Exception);
                MessageBoxResult result = MessageBox.Show(e.Exception.Message + "\r\nЗакрыть приложение? \r\n(Нажмите \"Нет\", чтобы продолжить работу и попытаться сохранить изменения)", "Ошибка в приложении", MessageBoxButton.YesNo, MessageBoxImage.Error);
                e.Handled = true;
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            };
            #endif

            VkDomainModel model = new VkDomainModel(new ModelConfig()
            {                
                RequestDelay = int.Parse(ConfigurationManager.AppSettings["requestDelay"]),
                RequestTimeout = int.Parse(ConfigurationManager.AppSettings["requestTimeout"]),
                RequestResourceTimeout = int.Parse(ConfigurationManager.AppSettings["requestResourceTimeout"]),
                CaptchaErrorCode = int.Parse(ConfigurationManager.AppSettings["captchaErrorCode"]),
                ApiHost = ConfigurationManager.AppSettings["vkApiHost"],
                LoginUrl = ConfigurationManager.AppSettings["loginUrl"],
                DataAccess = new DataAccess.DataAccess(),
                CryptoProvider = new CryptoProvider(),
                MessageDebugVkUserId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["debugMessageVkUserId"]) ? 0 : 
                    int.Parse(ConfigurationManager.AppSettings["debugMessageVkUserId"])
            });            

            LoginWindow loginWnd = new LoginWindow();
            LoginPresenter loginPresenter = new LoginPresenter(loginWnd, model);

            AboutWindow aboutWnd = new AboutWindow();
            CaptchaWindow captchaWindow = new CaptchaWindow();

            MainWindow mainWnd = new MainWindow();
            mainWnd.AddChildWindow(loginWnd);
            mainWnd.AddChildWindow(aboutWnd);
            mainWnd.AddChildWindow(captchaWindow);
            MainPresenter mainPresenter = new MainPresenter(mainWnd, model, loginPresenter);
            app.MainWindow = mainWnd;

            Dictionary<string, string> substConfig = new Dictionary<string, string>();
            string substUser = ConfigurationManager.AppSettings["substUser"];
            if (!string.IsNullOrEmpty(substUser))
            {
                substConfig.Add("substUser", substUser);
            }
            MessagePresenterConfig msgConfig = new MessagePresenterConfig()
            {
                SendMessageDelay = int.Parse(ConfigurationManager.AppSettings["sendMessageDelay"]),
                Log = logger, 
                Substitutor = new MessageSubstitutor(substConfig),
                VkUserId = int.Parse(ConfigurationManager.AppSettings["vkUserId"]),
                VkGroupId = int.Parse(ConfigurationManager.AppSettings["vkGroupId"]),
                CaptchaDelay = string.IsNullOrEmpty(ConfigurationManager.AppSettings["captchaDelay"]) ? 0 : 
                    int.Parse(ConfigurationManager.AppSettings["captchaDelay"])
            };
            RemindPresenter remindPresenter = new RemindPresenter(mainWnd, model, msgConfig);
            InvitePresenter invitePresenter = new InvitePresenter(mainWnd, model, msgConfig);

            app.InitializeComponent();
            app.Run(mainWnd);
        }

        #if DEBUG

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConsoleManager.Show();
            Console.WriteLine("Application has started");
        }

        #endif
    }
}
