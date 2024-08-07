using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProductEntity : TableEntity
    {
        public ProductEntity(string category, string productName)
        {
            PartitionKey = category;
            RowKey = productName;
        }

        public ProductEntity() { }

        public string Description { get; set; }
        public double Price { get; set; }
    }
}