using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AutoMapper;
using Databas.Models;
using Kzta.Annotations;
using Kzta.Commands;
using Kzta.Domain.Details;
using Kzta.Domain.Files;
using Kzta.Domain.Files.Models;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;
using Kzta.Views.ValidatorViews;
using Microsoft.Win32;
using Unity;
using File = System.IO.File;

namespace Kzta.ViewModels.Details
{
    public class DetailViewModel : INotifyPropertyChanged
    {
        //Serivces
        private readonly FileService _fileService;
        private readonly DetailService _detailService;
        private readonly PropertyTypeService _propertyTypeService;
        private readonly IUnityContainer _containers;

        //Property Type
        private ObservableCollection<PropertyType> _propertyTypes;

        public ObservableCollection<PropertyType> PropertyTypes
        {
            get => _propertyTypes;
            set
            {
                _propertyTypes = value;
                OnPropertyChanged();
            }
        }

        public PropertyType SelectedPropertyType { get; set; }

        //Картинка
        private BitmapSource _displayedImagePath;

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

        public DetailInfo Detail { get; set; }

        public bool IsCreateWindow { get; set; }

        public DetailViewModel(FileService fileService, DetailService detailService, IUnityContainer containers,
            PropertyTypeService propertyTypeService)
        {
            _fileService = fileService;
            _detailService = detailService;
            this._containers = containers;
            _propertyTypeService = propertyTypeService;
            Detail = new DetailInfo();
            InitializeData().Wait();
        }

        private async Task InitializeData()
        {
            IsCreateWindow = true;
            PropertyTypes = new ObservableCollection<PropertyType>();
            UploadImageCommand = new Command(async () => { await UploadImage(); }, () => true);
            SaveDataCommand = new Command(async () => await SaveDate(), () => true);
            PropertyTypes = new ObservableCollection<PropertyType>(await _propertyTypeService.Get().ToListAsync());
            SelectedPropertyType = PropertyTypes.FirstOrDefault();
        }

        private async Task UploadImage()
        {
            var res = UploadFileFromFileDialog().Result;
            if (res == Guid.Empty)
                return;
            await _detailService.SetImage(Detail.DetailtGuid, res);
            Detail.ImageGuid = res;
            var path = _fileService.GetPath(res).Result;
            SetImagePath(path);
        }

        private async Task SaveDate()
        {
            if (Validate())
                return;
            Detail.PropertyTypeGuid = SelectedPropertyType.PropertyTypeGuid;
            if (IsCreateWindow)
                await _detailService.Create(Detail);
            else
                await _detailService.Update(Detail.DetailtGuid, Detail);
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
        public void InitializeDetail(Detail detail)
        {
            Detail = Mapper.Map<DetailInfo>(detail);
            IsCreateWindow = false;
            SelectedPropertyType = PropertyTypes.FirstOrDefault(x => x.PropertyTypeGuid == detail.PropertyTypeGuid);
            SetImagePath(detail.Image?.Path);
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


        private bool Validate()
        {
            var validator = Validator.Create();
            validator = validator.IsNull(() => Detail)
                .IsNullOrEmpty(() => Detail.Name);
            if (SelectedPropertyType == null)
                validator.AddError("Выберите тип дитали.");
            return validator.RaiseError();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}