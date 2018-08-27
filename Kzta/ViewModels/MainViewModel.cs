using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Databas;
using Databas.Models;
using Kzta.Commands;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;
using Kzta.Views;
using Unity;

namespace Kzta.ViewModels
{
    /// <summary>
    /// Основной ViewModel
    /// </summary>
    public class MainViewModel
    {
        //Collections
        public ObservableCollection<PropertyType> PropertyTypes { get; set; }
        public ObservableCollection<Detail> Details { get; set; }

        //Services
        private readonly PropertyTypeService _propertyTypeService;

        private readonly IUnityContainer _containers;

        //SelectedItems
        public PropertyType SelectPropertyType { get; set; }

        //Db
        private readonly DatabaseContext _context;

        //Commands
        public Command CreatePropertyTypeCommand { get; set; }

        //Fiels
        public PropertyTypeInfo PropertyType { get; set; }


        public MainViewModel()
        {
        }

        public MainViewModel(PropertyTypeService service, DatabaseContext context, IUnityContainer containrs)
        {
            _propertyTypeService = service;
            _context = context;
            _containers = containrs;
            InitializeData().Wait();
        }

        private async Task InitializeData()
        {
            PropertyType = new PropertyTypeInfo();
            PropertyTypes = new ObservableCollection<PropertyType>(await _context.PropertyTypes.ToListAsync());
            Details = new ObservableCollection<Detail>(await _context.Details.ToListAsync());
            CreatePropertyTypeCommand = new Command(async () =>
            {
                //TODO Вариант открытия окна
                var detailContext= _containers.Resolve<DetailViewModel>();
                var detailWindow = _containers.Resolve<DetailView>();
                detailWindow.DataContext = detailContext;
                detailWindow.ShowDialog();
                //TODO Вариант создания
                //if (Validate(PropertyType))
                //    return;

                //var id = await _propertyTypeService.Create(PropertyType);
                //var res = await _propertyTypeService.GetSingle(id);
                //PropertyTypes.Add(res);
            }, () => true);
        }


        private bool Validate(PropertyTypeInfo model)
        {
            var val = Validator.Create();
            val = val
                .IsNullOrEmpty(() => model.Name)
                .IsNullOrEmpty(() => model.Description);
            return val.RaiseError();
        }
    }
}