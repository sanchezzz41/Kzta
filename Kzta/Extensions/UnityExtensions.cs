using System.IO;
using AutoMapper;
using AutoMapper.Configuration;
using Databas;
using Databas.Models;
using Kzta.Domain.Details;
using Kzta.Domain.Files;
using Kzta.Domain.Products;
using Kzta.Domain.PropertyTypes;
using Kzta.ViewModels;
using Kzta.ViewModels.Details;
using Kzta.ViewModels.Products;
using Kzta.ViewModels.PropertyTypes;
using Kzta.Views;
using Unity;
using Unity.Lifetime;
using FileOptions = Kzta.Domain.Files.FileOptions;

namespace Kzta.Extensions
{
    /// <summary>
    /// Расширение для Di
    /// </summary>
    public static class UnityExtensions
    {
        public static IUnityContainer AddServices(this IUnityContainer containers)
        {
            containers.RegisterType<DatabaseContext>(new ContainerControlledTransientManager());

            containers.RegisterType<DetailToProductViewModel>();
            containers.RegisterType<DetailService>();
            containers.RegisterType<DetailViewModel>();

            containers.RegisterType<FileService>();

            containers.RegisterType<ProductView>();
            containers.RegisterType<ProductService>();
            containers.RegisterType<CreateProductView>();

            containers.RegisterType<PropertyTypeService>();
            containers.RegisterType<PropertyTypeViewModel>();

            var rootPath = Directory.GetCurrentDirectory();
            var opt = new FileOptions {BaseRoot = rootPath +"/Images"};
            //var opt = new FileOptions {BaseRoot = rootPath + "~/../../Images" };
            containers.RegisterInstance(typeof(FileOptions), opt);
            Mapper.Initialize(new ConfigMapper());
            return containers;
        }
    }

    public class ConfigMapper : MapperConfigurationExpression
    {
        public ConfigMapper()
        {
            CreateMap<PropertyTypeInfo, PropertyType>();

            CreateMap<DetailInfo, Detail>();
            CreateMap<Detail, DetailInfo>();
            CreateMap<DetailWithCount, Detail>();

            CreateMap<ProductInfo, Product>();
            CreateMap<Product, ProductInfo>();

            CreateMap<DetailToProductInfo, DetailInProduct>();
            CreateMap<DetailInProduct, DetailWithCount>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Detail.Description))
                .ForMember(dest => dest.DetailtGuid, opt => opt.MapFrom(src => src.Detail.DetailtGuid))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Detail.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Detail.Size))
                .ForMember(dest => dest.InventoryNumber, opt => opt.MapFrom(src => src.Detail.InventoryNumber))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Detail.Material))
                .ForMember(dest => dest.ImageGuid, opt => opt.MapFrom(src => src.Detail.ImageGuid))
                .ForMember(dest => dest.PropertyTypeGuid, opt => opt.MapFrom(src => src.Detail.PropertyTypeGuid))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));
        }
    }
}
