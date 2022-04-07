using System;
using System.Collections.Generic;
using System.Linq;
using eStock.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eStock.CompanyData
{
    public class CompanyCRUD : ICompanyData
    {
        private IMongoDatabase db;

        public CompanyCRUD()
        {
            var client = new MongoClient();
            db = client.GetDatabase("eStock");
        }
        public List<T> getAllCompany<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T getCompanyById<T>(string table, string code)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Code", code);
            return collection.Find(filter).FirstOrDefault();
        }

        public void registerCompany<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public void deleteCompany<T>(string table,string code)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Code", code);
            collection.DeleteOne(filter);
        }

        public void addStock<T>(string table, string code,  CompanyModel company)
        {

            var collection = db.GetCollection<CompanyModel>(table);
            var result = collection.ReplaceOne(
                new BsonDocument("Code", code),
                company,
                new ReplaceOptions { IsUpsert = true });

        }

        public List<Stock> getStock(CompanyModel company, string startdate, string enddate)
        {
            DateTime startdt = Convert.ToDateTime(startdate);
            DateTime enddt = Convert.ToDateTime(enddate);
            return company.stockList.FindAll(x => x.dateTimeStock.Date >= startdt.Date && x.dateTimeStock.Date <= enddt.Date).ToList();
        }
    }
}
