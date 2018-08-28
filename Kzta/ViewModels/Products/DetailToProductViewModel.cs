using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Databas.Models;
using Kzta.Commands;
using Kzta.Domain.Details;
using Kzta.Domain.Products;
using Kzta.Extensions;

namespace Kzta.ViewModels.Products
{
    public class DetailToProductViewModel
    {
        private readonly DetailService _detailService;
        private readonly ProductService _productService;

        public ObservableCollection<Detail> Details { get; set; }
        public Detail SelectedDetail { get; set; }

        [Description("Кол-во")]
        public string Count { get; set; }

        private Product _product;

        public Command AddDetailToProductCommand { get; set; }

        public DetailToProductViewModel(DetailService detailService, ProductService productService)
        {
            _detailService = detailService;
            _productService = productService;
            Details = new ObservableCollection<Detail>(_detailService.Get().ToList());
            SelectedDetail = Details.FirstOrDefault();
            AddDetailToProductCommand = new Command(async () => await AddDetails(), () => SelectedDetail != null);
        }

        public void InitializeProduct(ProductInfo product)
        {
            _product = Mapper.Map<Product>(product);
        }

        public async Task AddDetails()
        {
            var validator = Validator.Create();
            var count = Count.ParseToInt();
            if (count <= 0)
                validator.AddError("Введите кол-во (больше 0).")
                    .RaiseError();
            var model = new DetailToProductInfo
            {
                Count = count,
                ProductGuid = _product.ProductGuid,
                DetailtGuid = SelectedDetail.DetailtGuid
            };
            await _productService.AddDetailToProduct(model);
        }
    }
}
