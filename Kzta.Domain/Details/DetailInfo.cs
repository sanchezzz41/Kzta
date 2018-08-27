using System;
using System.ComponentModel;

namespace Kzta.Domain.Details
{
    /// <summary>
    /// Модель для создания типа имущества
    /// </summary>
    public class DetailInfo
    {
        /// <summary>
        /// Внешний ключ на тип имущества
        /// </summary>
        public Guid PropertyTypeGuid { get; set; }

        /// <summary>
        /// Внешний ключ на файл изображения
        /// </summary>
        public Guid? ImageGuid { get; set; }

        /// <summary>
        ///  Название
        /// </summary>
        [Description("Название")]
        public string Name { get; set; }

        /// <summary>
        /// Инвентарный номер
        /// </summary>
        [Description("Инвентарный номер")]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Description("ОПисание")]
        public string Description { get; set; }

        /// <summary>
        /// Материал
        /// </summary>
        [Description("Материал")]
        public string Material { get; set; }

        /// <summary>
        /// Размеры
        /// </summary>
        [Description("Размеры")]
        public string Size { get; set; }
    }
}
