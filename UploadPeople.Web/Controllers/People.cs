using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using UploadPeople.Data;
using UploadPeople.Web.Models;

namespace UploadPeople.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class People : ControllerBase
    {
        private readonly string _connection;

        public People(IConfiguration config)
        {
            _connection = config.GetConnectionString("ConStr");   
        }

        [Route("generate")]
        [HttpGet]
        public List<Person> CreatePeople(GenerateVM gvm)
        {
            PeopleDealings deal = new PeopleDealings(_connection);
            return deal.GeneratePeople(gvm.Amount);
        }

        [Route("getall")]
        [HttpGet]
        public List<Person> GetPeople()
        {
            PeopleDealings deal = new PeopleDealings(_connection);
            return deal.GetPeople();
        }

        [Route("delete")]
        [HttpPost]
        public void GetRidOfAll()
        {
            PeopleDealings deal = new PeopleDealings(_connection);
            deal.DeleteDatabase();
        }
        
        [Route("download")]
        [HttpGet]
        public IActionResult GenerateCSV(int amount)
        {
            PeopleDealings deal = new PeopleDealings(_connection);
            List<Person> people = deal.GeneratePeople(amount);

            string stringedPeople = string.Join("\n", people.Select(p => p.ToString()));
            byte[] peopleBytes = Encoding.UTF8.GetBytes(stringedPeople);
            return File(peopleBytes, "text/csv", $"{amount}people.csv");

            //var writer = new StringWriter();
            //var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    Delimiter = ",",
            //    HasHeaderRecord = true,
            //    TrimOptions = TrimOptions.Trim,
            //    MissingFieldFound = null
            //});
            //csvWriter.WriteRecords(people);
            //This version didn't work
            //return File(writer.ToString(), $"{amount}people.csv");
        }

        [Route("upload")]
        [HttpPost]
        public void UploadCSV(UploadVM uvm)
        {
            int indexOfComma = uvm.Base64.IndexOf(",");
            string base64 = uvm.Base64.Substring(indexOfComma + 1);
            byte[] pplBytes = Convert.FromBase64String(base64);
            using MemoryStream memoryStream = new MemoryStream(pplBytes);
            using StreamReader reader = new StreamReader(memoryStream);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using CsvReader csvReader = new CsvReader(reader, csvConfig);
            var people = csvReader.GetRecords<Person>().ToList();

            PeopleDealings deal = new PeopleDealings(_connection);
            deal.InsertIntoDatabase(people);
        }
    }
}
