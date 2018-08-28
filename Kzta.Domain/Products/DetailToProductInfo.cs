using System;
using System.ComponentModel;

namespace Kzta.Domain.Products
{
    public class DetailToProductInfo
    {
        /// <summary>
        /// ВК на деталь
        /// </summary>
        [Description("Деталь")]
        public Guid DetailtGuid { get; set; }

        /// <summary>
        /// ВК на продукт
        /// </summary>
        [Description("Продукт")]
        public Guid ProductGuid { get; set; }

        /// <summary>
        /// Количество в продукте
        /// </summary>
        [Description("Количество")]
        public int Count { get; set; }

        public DetailToProductInfo()
        {
            
        }
    }
}
