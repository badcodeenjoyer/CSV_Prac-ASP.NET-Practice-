using CSVApp.Interfaces;
using CSVApp.Models;
using CsvHelper;
using System.Globalization;

namespace CSVApp.Services
{
    public class Read_CSVService : IRead
    {
        public List<CSVModel> Read_CSV(string fileName)
        {
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            var models = new List<CSVModel>();
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<WithoutIdMap>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {

                    var model = csv.GetRecord<CSVModel>();

                    models.Add(model);
                }
            }

            return models;

        }
    }
}
