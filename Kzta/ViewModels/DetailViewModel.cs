using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Databas;
using Databas.Models;
using Kzta.Annotations;
using Kzta.Commands;
using Kzta.Domain.Details;
using Kzta.Domain.Files;
using Kzta.Domain.Files.Models;
using Kzta.Extensions;
using Microsoft.Win32;
using File = System.IO.File;

namespace Kzta.ViewModels
{
    /// <summary>
    /// ViewModel для деталей, тоже самое что в основной ВМ
    /// </summary>
    public class DetailViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly DatabaseContext _context;
        private readonly DetailService _detailService;
        private readonly FileService _fileService;

        private BitmapSource _displayedImagePath;
        private Detail _selectedDetail;

        //PropertyType
        public ObservableCollection<PropertyType> PropertyTypes { get; set; }
        public PropertyType SelectedPropertyType { get; set; }

        //Detail
        public ObservableCollection<Detail> Details { get; set; }

        public Detail SelectedDetail
        {
            get => _selectedDetail;
            set
            {
                _selectedDetail = value;
                if (_selectedDetail.ImageGuid != null)
                {
                    var path = _fileService.GetPath(_selectedDetail.ImageGuid.Value).Result;
                    SetImagePath(path);
                }
                else
                    SetImagePath(null);

                OnPropertyChanged();
            }
        }

        public DetailInfo DetailInfo { get; set; }

        //File
        public Command UploadFile { get; set; }

        public BitmapSource DisplayedImagePath
        {
            get => _displayedImagePath;
            set
            {
                _displayedImagePath = value;
                OnPropertyChanged();
            }
        }

        public DetailViewModel(DatabaseContext context, DetailService detailService, FileService fileService)
        {
            _context = context;
            _detailService = detailService;
            _fileService = fileService;
            Initialize().Wait();
        }

        public DetailViewModel(DetailService detailService, FileService fileService)
        {
            _detailService = detailService;
            _fileService = fileService;
        }

        public void InitializeActions(Action ok, Action close)
        {
            OkayCommand = new Command(() => { Create(); }, () => true);
            CancelCommand = new Command(close, () => true);
        }

        private async Task Initialize()
        {
            OkayCommand = new Command(async () => { await Create(); }, () => true);
            UploadFile = new Command(() =>
            {
                var res = UploadFileFromFileDialog().Result;
                if (res == Guid.Empty)
                    return;
                DetailInfo.ImageGuid = res;
                var path = _fileService.GetPath(res).Result;
                SetImagePath(path);
            }, () => true);

            PropertyTypes = new ObservableCollection<PropertyType>(await _context.PropertyTypes.ToListAsync());
            SelectedPropertyType = PropertyTypes.FirstOrDefault();
            Details = new ObservableCollection<Detail>(await _context.Details.ToListAsync());
            DetailInfo = new DetailInfo();
            SelectedDetail = new Detail();
        }

        private async Task Create()
        {
            if (Validate(DetailInfo))
                return;
            DetailInfo.PropertyTypeGuid = SelectedPropertyType.PropertyTypeGuid;
            var id = await _detailService.Create(DetailInfo);
            var result = await _detailService.GetSingle(id);
            Details.Add(result);
        }


        private bool Validate(DetailInfo detailInfo)
        {
            var validator = Validator.Create();
            validator = validator
                .IsNull(() => detailInfo)
                .IsNullOrEmpty(() => detailInfo.Name);
            if (SelectedPropertyType == null)
                validator.AddError("Тип дитали не выбран.");
            return validator.RaiseError();
        }

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
            fileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
