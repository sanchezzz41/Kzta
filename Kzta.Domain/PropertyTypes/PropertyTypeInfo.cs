using System.ComponentModel;

namespace Kzta.Domain.PropertyTypes
{
    /// <summary>
    /// Модель для создания типа имущества
    /// </summary>
    public class PropertyTypeInfo
    {
        /// <summary>
        /// Hазвание типа имущества
        /// </summary>
        [Description("Название")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Description("Описание")]
        public string Description { get; set; }
    }
}
