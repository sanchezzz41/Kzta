using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Databas;
using Databas.Models;
using Kzta.Annotations;
using Kzta.Commands;
using Kzta.Domain.Details;
using Kzta.Domain.Products;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;
using Kzta.ViewModels.Details;
using Kzta.ViewModels.PropertyTypes;
using Kzta.Views;
using Kzta.VIews;
using Unity;

namespace Kzta.ViewModels
{
    /// <summary>
    /// Основной ViewModel
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        //Collections
        /// <summary>
        /// Тип детали
        /// </summary>
        public ObservableCollection<PropertyType> PropertyTypes { get; set; }

        /// <summary>
        /// Детали
        /// </summary>
        public ObservableCollection<Detail> Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Изделия
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        //Services

        private readonly IUnityContainer _containers;
        private  ProductService _productService;
        private  DetailService _detailService;

        //SelectedItems
        public PropertyType SelectPropertyType { get; set; }
        public Product SelectedProduct { get; set; }
        public Detail SelectedDetail { get; set; }

        //Db
        private  DatabaseContext _context;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Detail> _details;

        //Commands
        public Command CreateProductCommand { get; set; }
        public Command DeleteProductCommand { get; set; }
        public Command ViewProductCommand { get; set; }

        public Command CreatePropertyTypeCommand { get; set; }
        //Commands/Details
        public Command CreateDetailCommand { get; set; }
        public Command ViewDetailCommand { get; set; }
        public Command DeleteDetailCommand { get; set; }

        //Fiels
        public PropertyTypeInfo PropertyType { get; set; }


        public MainViewModel(ProductService productService, DetailService detailService)
        {
            _productService = productService;
            _detailService = detailService;
        }

        public MainViewModel( DatabaseContext context, IUnityContainer containrs, ProductService productService, DetailService detailService)
        {
            _context = context;
            _containers = containrs;
            _productService = productService;
            _detailService = detailService;
            InitializeData().Wait();
        }

        private async Task InitializeData()
        {
            PropertyType = new PropertyTypeInfo();
            PropertyTypes = new ObservableCollection<PropertyType>(await _context.PropertyTypes.ToListAsync());
            Details = new ObservableCollection<Detail>(await _context.Details.ToListAsync());
            Products = new ObservableCollection<Product>(await _productService.Get());
            CreateProductCommand = new Command(async () => await CreateProduct(), () => true);
            DeleteProductCommand = new Command(async () => await DeleteProduct(), () => true);
            ViewProductCommand = new Command(async () => { await ViewProduct(); }, () => true);
            CreatePropertyTypeCommand = new Command(async () => await CreatePropertyType(), () => true);

            CreateDetailCommand = new Command(async () => await CreateDetail(), () => true);
            ViewDetailCommand = new Command(async () => await ViewDetail(), () => true);
            DeleteDetailCommand = new Command(async () => await DeleteDetail(), () => true);
        }

        public async Task CreateProduct()
        {
            var context = _containers.Resolve<ProductViewModel>();
            var window = _containers.Resolve<CreateProductView>();
            context.InitializeClose(() => window.Close());
            context.IsCreateWindow = true;
            window.DataContext = context;
            window.ShowDialog();
            Products = new ObservableCollection<Product>(await _productService.Get());
        }

        public async Task DeleteProduct()
        {
            await _productService.Delete(SelectedProduct.ProductGuid);
            Products.Remove(SelectedProduct);
        }

        public async Task ViewProduct()
        {
            var context = _containers.Resolve<ProductViewModel>();
            var window = _containers.Resolve<ProductView>();
            context.InitializeProduct(SelectedProduct);
            context.InitializeClose(() => window.Close());
            window.DataContext = context;
            window.ShowDialog();
            Products = new ObservableCollection<Product>(await _productService.Get());
            Details = new ObservableCollection<Detail>(await _detailService.Get().ToListAsync());
        }

        private async Task CreatePropertyType()
        {
            var context = _containers.Resolve<PropertyTypeViewModel>();
            var window = _containers.Resolve<PropertyTypeView>();
            context.InitializeClose(() => window.Close());
            window.DataContext = context;
            window.ShowDialog();
        }

        private async Task CreateDetail()
        {
            var context = _containers.Resolve<DetailViewModel>();
            var window = _containers.Resolve<DetailView>();
            context.InitializeClose(() => window.Close());
            window.DataContext = context;
            window.ShowDialog();
            Details = new ObservableCollection<Detail>(await _detailService.Get().ToListAsync());
        }

        private async Task ViewDetail()
        {
            var context = _containers.Resolve<DetailViewModel>();
            var window = _containers.Resolve<DetailView>();
            context.InitializeClose(() => window.Close());
            context.InitializeDetail(SelectedDetail);
            window.DataContext = context;
            window.ShowDialog();
            Details = new ObservableCollection<Detail>(await _detailService.Get().ToListAsync());
        }

        public async Task DeleteDetail()
        {
            await _detailService.Delete(SelectedDetail.DetailtGuid);
            Details.Remove(SelectedDetail);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}