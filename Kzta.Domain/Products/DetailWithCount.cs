using System;

namespace Kzta.Domain.Products
{
    public class DetailWithCount
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid DetailtGuid { get; set; }

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
        public string Name { get; set; }

        /// <summary>
        /// Инвентарный номер
        /// </summary>
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Материал
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// Размеры
        /// </summary>
        public string Size { get; set; }

        public int Count { get; set; }
    }
}
