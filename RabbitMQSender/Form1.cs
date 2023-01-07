using Microsoft.EntityFrameworkCore;
using RabbitMQSender.Model;

namespace RabbitMQSender
{
    public partial class Form1 : Form
    {
        RabbitMQMessageSender _rabbitMQMessageSender;
        public Form1()
        {
            InitializeComponent();
            
        }


        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string erro = "";
            if (txtNome.Text.Trim() == "")
            {
                erro = "Você deve informar um nome.";
            }
            if (txtIdade.Text == "" || txtIdade.Text.Contains(" "))
            {
                erro = "Idade incorreta, revise o campo.";
            }

            if (erro != "")
            {
                MessageBox.Show(erro);
                return;
            }

            Guid myuuid = Guid.NewGuid();

            _rabbitMQMessageSender = new RabbitMQMessageSender();

            Report report = new Report()
            {
                Id = myuuid.ToString(),
                Nome = txtNome.Text,
                Idade = txtIdade.Text == "" ? 0 : int.Parse(txtIdade.Text),
                DataHora = DateTime.Now

            };
            _rabbitMQMessageSender.SendMessage(report, "LeadQueue");

            txtNome.Text = "";
            txtIdade.Text = "";
        }
    }
}