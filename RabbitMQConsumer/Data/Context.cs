using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options): base(options)
        { }
        public DbSet<Report> Reports { get; set; }
    }
}
