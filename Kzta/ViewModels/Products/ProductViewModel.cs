using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AutoMapper;
using Databas;
using Databas.Models;
using Kzta.Annotations;
using Kzta.Commands;
using Kzta.Domain.Details;
using Kzta.Domain.Files;
using Kzta.Domain.Files.Models;
using Kzta.Domain.Products;
using Kzta.Extensions;
using Kzta.ViewModels.Details;
using Kzta.ViewModels.Products;
using Kzta.Views;
using Kzta.Views.ValidatorViews;
using Microsoft.Win32;
using Unity;
using File = System.IO.File;

namespace Kzta.ViewModels
{
    /// <summary>
    /// VM для продуктов
    /// </summary>
    public class ProductViewModel : INotifyPropertyChanged
    {
        //Serivces
        private readonly DatabaseContext _context;
        private readonly FileService _fileService;
        private readonly ProductService _productService;
        private readonly IUnityContainer _containers;
        private readonly DetailService _detailService;

        //Details
        public ObservableCollection<DetailWithCount> Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged();
            }
        }

        public DetailWithCount SelectedDetail { get; set; }

        //Продукты
        public ProductInfo ProductInfo { get; set; }

        //Картинка
        private BitmapSource _displayedImagePath;
        private ObservableCollection<DetailWithCount> _details;

        public BitmapSource DisplayedImagePath
        {
            get => _displayedImagePath;
            set
            {
                _displayedImagePath = value;
                OnPropertyChanged();
            }
        }
        
        //Commands
        public Command UploadImageCommand { get; set; }
        public Command SaveDataCommand { get; set; }
        public Command AddDetailToProductCommand { get; set; }
        public Command ViewDetailCommand { get; set; }

        public bool IsCreateWindow { get; set; }

        public ProductViewModel(DatabaseContext context, FileService fileService, ProductService productService, IUnityContainer containers, DetailService detailService)
        {
            _context = context;
            _fileService = fileService;
            _productService = productService;
            this._containers = containers;
            _detailService = detailService;
            ProductInfo = new ProductInfo();
            InitializeData().Wait();
        }

        private async Task InitializeData()
        {
            IsCreateWindow = false;
            Details = new ObservableCollection<DetailWithCount>();
            UploadImageCommand = new Command( async () => { await UploadImage(); }, () => true);
            SaveDataCommand = new Command(async () => await SaveDate(), () => true);
            AddDetailToProductCommand = new Command(async () => { await AddDetail(); }, () => true);
            ViewDetailCommand = new Command(async () => await ViewDetail(), () => true);
        }

        private async Task UploadImage()
        {
            var res = UploadFileFromFileDialog().Result;
            if (res == Guid.Empty)
                return;
            await _productService.SetImage(ProductInfo.ProductGuid, res);
            ProductInfo.FileGuid = res;
            var path = _fileService.GetPath(res).Result;
            SetImagePath(path);
        }

        private async Task SaveDate()
        {
            if (Validate())
                return;
            if (IsCreateWindow)
                await _productService.Create(ProductInfo);
            else
                await _productService.Edit(ProductInfo.ProductGuid, ProductInfo);
            new SaveResultView()
                .ShowDialog();
            Close?.Invoke();
        }

        public Action Close { get; set; }

        public void InitializeClose(Action close)
        {
            Close = close;
        }

        /// <summary>
        /// Иницилизирует объект продукта
        /// </summary>
        /// <param name="product"></param>
        public void InitializeProduct(Product product)
        {
            ProductInfo = Mapper.Map<ProductInfo>(product);
            UpdateDetails().Wait();
            SetImagePath(product.File?.Path);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void SetImagePath(string path)
        {
            if (path == null)
            {
                DisplayedImagePath = new BitmapImage();
                return;
            }

            var uri = new Uri(path);
            DisplayedImagePath = new BitmapImage(uri);
        }

        private async Task<Guid> UploadFileFromFileDialog()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.png;*.jpeg;*jpg;)|*.png;*.jpeg;*.jpg;|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                var file = new FileInfo
                {
                    Path = fileDialog.FileName,
                    FileName = fileDialog.SafeFileName,
                    Content = File.OpenRead(fileDialog.FileName)
                };
                return await _fileService.UploadFile(file);
            }

            return Guid.Empty;
        }

        public async Task AddDetail()
        {
            var context = _containers.Resolve<DetailToProductViewModel>();
            var window = _containers.Resolve<DetailToProductView>();
            context.InitializeProduct(ProductInfo);
            window.DataContext = context;
            window.ShowDialog();
            await UpdateDetails();
        }

        public async Task UpdateDetails()
        {
            Details = new ObservableCollection<DetailWithCount>(await _productService
                .GetDetailWithProduct(ProductInfo.ProductGuid));
        }

        private async Task ViewDetail()
        {
            var context = _containers.Resolve<DetailViewModel>();
            var window = _containers.Resolve<DetailView>();
            context.InitializeClose(() => window.Close());
            var detail = await _detailService.GetSingle(SelectedDetail.DetailtGuid);
            context.InitializeDetail(detail);
            window.DataContext = context;
            window.ShowDialog();
            await UpdateDetails();
        }

        private bool Validate()
        {
            var validator = Validator.Create();
            return validator
                .IsNull(() => ProductInfo)
                .IsNullOrEmpty(() => ProductInfo.Name)
                .RaiseError();
        }
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
