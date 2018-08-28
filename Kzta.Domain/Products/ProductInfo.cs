using System;
using System.ComponentModel;
using Databas.Models;

namespace Kzta.Domain.Products
{
    /// <summary>
    /// Модель для продукта(добейте меня, пишу в ласт день в 10 вечера)
    /// </summary>
    public class ProductInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid ProductGuid { get; set; }

        /// <summary>
        /// Внешний ключ на файл(картинку)
        /// </summary>
        public Guid? FileGuid { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [Description("Название")]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [Description("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// S/N
        /// </summary>
        [Description("Серийный номер")]
        public string SerialNumber { get; set; }

        public ProductInfo()
        {
            
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Product product))
                return false;
            if (product.Name == Name
                && product.ProductGuid == ProductGuid
                && product.Description == Description
                && product.SerialNumber == SerialNumber
                && product.FileGuid == FileGuid)
                return true;
            return false;
        }
    }
}
