﻿namespace UCommerce.MasterClass.Models
{
    public class OrderlineModel
    {
        public string Total { get; set; }

        public int Quantity { get; set; }

        public int OrderLineId { get; set; }

        public string Sku { get; set; }

        public string VariantSku { get; set; }

        public string ProductName { get; set; }
    }
}