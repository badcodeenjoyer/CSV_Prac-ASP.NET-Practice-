using CSVApp.Models;
using CsvHelper.Configuration;
using System.Globalization;

namespace CSVApp
{
    public class WithoutIdMap : ClassMap<CSVModel>
    {
        public WithoutIdMap()
        {

            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
        }
    }
}
