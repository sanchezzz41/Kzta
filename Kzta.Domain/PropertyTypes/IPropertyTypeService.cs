using System;
using System.Threading.Tasks;
using Astral.Extensions.Data;
using Astral.PD.Workflow.Domain.Entities;
using Astral.PD.Workflow.Domain.PropertyTypes.Models;

namespace Astral.PD.Workflow.Domain.PropertyTypes
{
    /// <summary>
    /// Сервис для работы с типом имущества
    /// </summary>
    public interface IPropertyTypeService
    {
        /// <summary>
        /// Создаёт тип имущества
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Create(PropertyTypeInfo model);

        /// <summary>
        /// Обновляет тип имущества
        /// </summary>
        /// <param name="propertyTypeGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(Guid propertyTypeGuid, PropertyTypeInfo model);

        /// <summary>
        /// Удаляет тип имущества
        /// </summary>
        /// <param name="propertyTypeGuid"></param>
        /// <returns></returns>
        Task Delete(Guid propertyTypeGuid);

        /// <summary>
        /// Фильтрует тип имущества
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        CollectionReference<PropertyType> Search(string search);
    }
}
