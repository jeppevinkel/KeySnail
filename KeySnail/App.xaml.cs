using System;
using System.Windows;
using KeySnail.Views;
using Prism.Ioc;
using Prism.Unity;

namespace KeySnail
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Services.IKeyboardService, Services.KeyboardServiceInstance>();
            containerRegistry.Register<Services.IKeyBindStore, Services.DbKeyBindStore>();
            containerRegistry.Register<Services.IInputSimulatorInstance, Services.InputSimulatorInstance>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}