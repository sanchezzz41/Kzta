using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Databas.Models
{
    /// <summary>
    /// Связь для бд (продукт-деталь)
    /// </summary>
    public class DetailInProduct
    {
        /// <summary>
        /// ВК на деталь
        /// </summary>
        [ForeignKey(nameof(Detail))]
        public Guid DetailGuid { get; set; }

        /// <summary>
        /// ВК на продукт
        /// </summary>
        [ForeignKey(nameof(Product))]
        public Guid ProductGuid { get; set; }

        /// <summary>
        /// Количество в продукте
        /// </summary>
        public int Count { get; set; }

        public virtual Detail Detail { get; set; }

        public virtual Product Product { get; set; }

        public DetailInProduct()
        {
            
        }

        /// <inheritdoc />
        public DetailInProduct(Guid detailGuid, Guid productGuid, int count)
        {
            DetailGuid = detailGuid;
            ProductGuid = productGuid;
            Count = count;
        }
    }
}
