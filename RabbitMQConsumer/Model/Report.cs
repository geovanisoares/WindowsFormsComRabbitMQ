using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Model
{
    public class Report
    {
        [Key]
        public string Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataHora { get; set; }
    }
}
