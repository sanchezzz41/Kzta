using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databas.Models
{
    /// <summary>
    /// Продукт(изделие)
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ProductGuid { get; set; }

        /// <summary>
        /// Внешний ключ на файл(картинку)
        /// </summary>
        [ForeignKey(nameof(File))]
        public Guid? FileGuid { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// S/N
        /// </summary>
        [MaxLength(1000)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Картинка(да да, сори за название)
        /// </summary>
        public virtual File File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<DetailInProduct> DetailInProducts { get; set; }

        public Product()
        {
            ProductGuid = Guid.NewGuid();
        }

        /// <inheritdoc />
        public Product(Guid productGuid, Guid? fileGuid, string name, string description)
        {
            ProductGuid = productGuid;
            FileGuid = fileGuid;
            Name = name;
            Description = description;
        }
    }
}