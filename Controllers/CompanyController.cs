using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStock.CompanyData;
using eStock.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace eStock.Controllers
{

    public class CompanyController : ControllerBase
    {
        //public static CompanyCRUD db = new CompanyCRUD("eStock");

        private ICompanyData _companyData;
        public CompanyController(ICompanyData companyData)
        {
            _companyData = companyData;
        }

        [HttpGet]
        [Route("api/v1.0/market/company/getall")]
        public IActionResult GetAll()
        {
            return Ok(_companyData.getAllCompany<CompanyModel>("CompaniesTbl"));
        }

        [HttpGet]
        [Route("/api/v1.0/market/company/info/{code}")]
        public IActionResult GetById(string code)
        {
            code = code.ToUpper();
            var company = _companyData.getCompanyById<CompanyModel>("CompaniesTbl", code);
            if (company != null)
            {
                return Ok(company);
            }
            else
            {
                return NotFound("Company Not found");
            }
        }

        [HttpPost]
        [Route("/api/v1.0/market/company/register")]
        public IActionResult registerComany([FromBody] CompanyModel company)
        {
            var companyFound = _companyData.getCompanyById<CompanyModel>("CompaniesTbl", company.Code);
            if (companyFound != null)
            {
                return Ok("Company code Already Exists");
            }
            else
            {
                company.id = new Guid();
                _companyData.registerCompany("CompaniesTbl", company);
                return Ok("Company Added Successfully");
            }
        }

        [HttpDelete]
        [Route("/api/v1.0/market/company/delete/{code}")]
        public IActionResult deleteCompany(string code)
        {
            code = code.ToUpper();
            var company = _companyData.getCompanyById<CompanyModel>("CompaniesTbl", code);
            if (company != null)
            {
                _companyData.deleteCompany<CompanyModel>("CompaniesTbl", code);
                return Ok("Company deleted Successfully");
            }
            else
            {
                return NotFound("Company Not found");
            }
        }

        [HttpPut]
        [Route("/api/v1.0/market/stock/add/{code}")]
        public IActionResult AddStock(string code, [FromBody] string price)
        {
            code = code.ToUpper();
            var company = _companyData.getCompanyById<CompanyModel>("CompaniesTbl", code);
            Stock stock = new Stock();

            double stockPrice = Convert.ToDouble(price);
            if (stockPrice > 0)
            {
                stock.price = (float)stockPrice;
                stock.dateTimeStock = DateTime.Now;
                if (company != null)
                {
                    if (company.stockList is null)
                    {
                        company.stockList = new List<Stock>();
                    }
                    company.stockList.Add(stock);
                    _companyData.addStock<CompanyModel>("CompaniesTbl", company.Code, company);
                    return Ok("Stock Added successfully");
                }
                else
                {
                    return NotFound("Company Not found");
                }
            }
            else
            {
                return Ok("Price Cannot be zero");
            }

        }

        [HttpGet]
        [Route("/api/v1.0/market/stock/get/{code}/{startdate}/{enddate}")]
        public IActionResult GetStock(string code,string startdate, string enddate)
        {
            code = code.ToUpper();
            var company = _companyData.getCompanyById<CompanyModel>("CompaniesTbl", code);
            if (company != null)
            {
                CompanyModel comp = new CompanyModel
                {
                    id = company.id,
                    Code = company.Code,
                    Name = company.Name,
                    CEO = company.CEO,
                    Turnover = company.Turnover,
                    Website = company.Website,
                    Stock_Exchange = company.Stock_Exchange,
                    stockList = new List<Stock>()
                };

                var stockLst = _companyData.getStock(company, startdate, enddate);
                if (stockLst != null)
                {
                    comp.stockList = stockLst;
                }

                return Ok(comp);
            }
            else
            {
                return NotFound("Company Not found");
            }
        }
    }
}
