using System;
using Astral.PD.Workflow.Domain.Entities;

namespace Astral.PD.Workflow.Domain.PropertyTypes.Models
{
    /// <summary>
    /// Модель для отображения типа имущества
    /// </summary>
    public class PropertyTypeModel
    {
        /// <summary>
        /// Id типа имущества
        /// </summary>
        public Guid PropertyTypeGuid { get; set; }

        /// <summary>
        /// Hазвание типа имущества
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc />
        public PropertyTypeModel()
        {
            
        }

        /// <inheritdoc />
        public PropertyTypeModel(PropertyType propertyType)
        {
            PropertyTypeGuid = propertyType.PropertyTypeGuid;
            Name = propertyType.Name;
            Description = propertyType.Description;
        }
    }
}
