using eStock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStock.CompanyData
{
    public interface ICompanyData
    {
        List<T> getAllCompany<T>(string table);
        T getCompanyById<T>(string table, string code);
        void registerCompany<T>(string table, T record);
        void deleteCompany<T>(string table, string code);
        void addStock<T>(string table,string code, CompanyModel company);
        List<Stock> getStock(CompanyModel company, string startdate, string enddate);
    }
}
