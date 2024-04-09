using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TenkiAme.Models;

namespace TenkiAme.Data
{
    public class TenkiAmeContext : DbContext
    {
        public TenkiAmeContext (DbContextOptions<TenkiAmeContext> options)
            : base(options)
        {
        }

        public DbSet<TenkiAme.Models.Home> Home { get; set; } = default!;
    }
}
