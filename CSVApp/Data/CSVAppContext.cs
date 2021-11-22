
using Microsoft.EntityFrameworkCore;


namespace CSVApp.Data
{
    public class CSVAppContext : DbContext
    {
        public CSVAppContext (DbContextOptions<CSVAppContext> options)
            : base(options)
        {
        }

        public DbSet<CSVApp.Models.CSVModel> CSVModel { get; set; }
    }
}
