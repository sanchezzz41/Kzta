using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kzta.Commands;
using Kzta.Domain.PropertyTypes;
using Kzta.Extensions;

namespace Kzta.ViewModels.PropertyTypes
{
    public class PropertyTypeViewModel
    {
        private readonly PropertyTypeService _propertyTypeService;
        
        public PropertyTypeViewModel(PropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
            PropertyTypeInfo = new PropertyTypeInfo();
            CreateCommand = new Command(async () => await Create(), () => true);
        }

        public Command CreateCommand { get; set; }
        private Action _close;
        public PropertyTypeInfo PropertyTypeInfo { get; set; }

        public void InitializeClose(Action close)
        {
            _close = close;
        }

        private async Task Create()
        {
            if (Validator.Create()
                .IsNull(() => PropertyTypeInfo)
                .IsNullOrEmpty(() => PropertyTypeInfo.Name)
                .RaiseError())
                return;
            //kill me
            await _propertyTypeService.Create(PropertyTypeInfo);
            _close?.Invoke();
        }

    }
}
