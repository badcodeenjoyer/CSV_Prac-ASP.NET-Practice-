using CSVApp.Models;
using CsvHelper.Configuration;
using System.Globalization;

namespace CSVApp
{
    public class FooMap : ClassMap<CSVModel>
    {
        public FooMap()
        {

            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
        }
    }
}
