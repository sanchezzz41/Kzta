using System.Windows;
using Databas;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;
using Kzta.ViewModels;
using Kzta.ViewModels.Details;
using Unity;

namespace Kzta
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            container.AddServices();
            InitializeDb(container);

            var mainModel = container.Resolve<MainViewModel>();
            Resources.Add("MainViewModel", mainModel);

            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void InitializeDb(IUnityContainer containers)
        {
            var context = containers.Resolve<DatabaseContext>();
            context.Database.CreateIfNotExists();
        }
    }
}
