using System.IO;
using AutoMapper;
using AutoMapper.Configuration;
using Databas;
using Databas.Models;
using Kzta.Domain.Details;
using Kzta.Domain.Files;
using Kzta.Domain.PropertyTypes;
using Unity;
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
            containers.RegisterType<DatabaseContext>();
            containers.RegisterType<PropertyTypeService>();
            containers.RegisterType<DetailService>();
            containers.RegisterType<FileService>();
            var rootPath = Directory.GetCurrentDirectory();
            var opt = new FileOptions {BaseRoot = rootPath +"~/../../Images"};
            containers.RegisterInstance(typeof(FileOptions), opt);
            Mapper.Initialize(new ConfigMapper());
            return containers;
        }
    }

    public class ConfigMapper : MapperConfigurationExpression
    {
        public ConfigMapper()
        {
            CreateMap<DetailInfo, Detail>();
        }
    }
}
