using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Data;
using RabbitMQConsumer.Model;
using System.Timers;

namespace RabbitMQConsumer
{
    public partial class Form1 : Form
    {
        DbContextOptions<Context> _options;
        Context _context;
        RabbitMQMessageConsumer _rabbitMQMessageConsumer = new RabbitMQMessageConsumer();
        private static System.Timers.Timer _timer;
        CancellationToken _token = new CancellationToken();
        public Form1()
        {

            InitializeComponent();

            _options = new DbContextOptionsBuilder<Context>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            _context = new Context(_options);
            _timer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            /*using (var context = new Context(_options))
            {
                _context = context;
                Guid guid = Guid.NewGuid();
                var report = new Report
                {
                    Id = guid.ToString(),
                    Nome = "Geovani",
                    Idade = 30,
                    DataHora = DateTime.Now
                };

                context.Reports.Add(report);
                context.SaveChanges();



        }*/


        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                if (_context.Reports.ToList().Count != dgvLeadsConsumidos.Rows.Count)
                {
                    dgvLeadsConsumidos.DataSource = _context.Reports.ToList();
                }
                
            }));
                
                //context.SaveChanges();

            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _rabbitMQMessageConsumer.StartAsync(_token);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _token.ThrowIfCancellationRequested();
        }
    }
}