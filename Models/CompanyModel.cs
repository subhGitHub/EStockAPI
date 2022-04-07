using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStock.Models
{
    public class CompanyModel
    {
        [BsonId]
        public Guid id { get; set; }
        [BsonRequired]
        public string Code { get; set; }
        [BsonRequired]
        public string Name { get; set; }
        [BsonRequired]
        public string CEO { get; set; }
        [BsonRequired]
        public float Turnover { get; set; }
        [BsonRequired]
        public string Website { get; set; }
        [BsonRequired]
        public string Stock_Exchange { get; set; }
        public List<Stock> stockList { get; set; }
    }

    public class Stock
    {
        public float price { get; set; }
        public DateTime dateTimeStock { get; set; }
    }

 }
