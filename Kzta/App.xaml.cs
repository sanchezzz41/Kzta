using System.Windows;
using Databas;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;
using Kzta.ViewModels;
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
            var detailModel = container.Resolve<DetailViewModel>();
            Resources.Add("MainViewModel", mainModel);
            Resources.Add("DetailViewModel", detailModel);

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
