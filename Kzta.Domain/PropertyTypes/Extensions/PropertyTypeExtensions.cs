using Astral.Extensions.Data;
using Astral.PD.Workflow.Domain.Entities;

namespace Astral.PD.Workflow.Domain.PropertyTypes.Extensions
{
    /// <summary>
    /// Расширения для типа имущества
    /// </summary>
    public static class PropertyTypeExtensions
    {
        /// <summary>
        /// Сортирует тип имущества
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static CollectionReference<PropertyType> Sort(this CollectionReference<PropertyType> collection,
            PropertyTypeSortField sortField, SortOrder sortOrder)
        {
            switch (sortField)
            {
                case PropertyTypeSortField.Name:
                    collection = collection.OrderBy(x => x.Name, sortOrder);
                    break;
                case PropertyTypeSortField.Description:
                    collection = collection.OrderBy(x => x.Description, sortOrder);
                    break;
                default:
                    collection = collection.OrderBy(x => x.Name, sortOrder);
                    break;
            }
            return collection;
        }
    }
}
