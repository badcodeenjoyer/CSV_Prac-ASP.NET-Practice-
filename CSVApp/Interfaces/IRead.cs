using CSVApp.Models;

namespace CSVApp.Interfaces
{
    public interface IRead
    {
        public List<CSVModel> Read_CSV(string fileName);
    }
}
