using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databas.Models
{
    /// <summary>
    /// Техника
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid DetailtGuid { get; set; }

        /// <summary>
        /// Внешний ключ на тип имущества
        /// </summary>
        [ForeignKey(nameof(PropertyType))]
        public Guid PropertyTypeGuid { get; set; }
        
        /// <summary>
        /// Внешний ключ на файл изображения
        /// </summary>
        [ForeignKey(nameof(Image))]
        public Guid? ImageGuid { get; set; }

        /// <summary>
        ///  Название
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Инвентарный номер
        /// </summary>
        [MaxLength(100)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Материал
        /// </summary>
        [MaxLength(100)]
        public string Material { get; set; }

        /// <summary>
        /// Размеры
        /// </summary>
        [MaxLength(100)]
        public string Size { get; set; }

        /// <summary>
        /// Тип имущества
        /// </summary>
        public virtual PropertyType PropertyType{ get; set; }
        
        /// <summary>
        /// Файл изображения
        /// </summary>
        public virtual File Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<DetailInProduct> DetailInProducts { get; set; }


        /// <summary>
        /// Конструктор техники
        /// </summary>
        public Detail()
        {
            DetailtGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Конструктор техники
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inventoryNumber"></param>
        /// <param name="description"></param>
        /// <param name="propertyTypeGuid"></param>
        /// <param name="imageGuid"></param>
        public Detail(string name, string inventoryNumber, string description,
            Guid propertyTypeGuid, Guid? imageGuid)
        {
            DetailtGuid = Guid.NewGuid();
            Name = name;
            InventoryNumber = inventoryNumber;
            Description = description;
            PropertyTypeGuid = propertyTypeGuid;
            ImageGuid = imageGuid;
        }
    }
}


