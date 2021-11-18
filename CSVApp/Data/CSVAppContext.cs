using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSVApp.Models;

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
