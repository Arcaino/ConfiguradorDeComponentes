using System.ComponentModel.DataAnnotations;

namespace ConfiguradorDeComponents.Models
{
    public class Alarmes : ClassificacaoAlarmes
    {
        public int IdAlarme { get; set; }
        [StringLength(200)] 
        [Required(ErrorMessage = "O preenchimento do campo é necessário")]
        public string DescricaoAlarme { get; set; }
        public string DataDeCadastroAlarme { get; set; }
        [Required(ErrorMessage = "O preenchimento do campo é necessário")]
        public bool Status { get; set; }
        public string DataEntrada { get; set; }
        public string DataSaida { get; set; }
        public int VezesAtuadas { get; set; }
    }
    public class ClassificacaoAlarmes : EquipamentosRelacionados
    {
        [Range(1, 1000, ErrorMessage = "Escolha uma opção válida")]
        public int IdClassificacaoDoAlarme { get; set; }
        public string NomeClassificacaoAlarme { get; set; }
    }

    public class EquipamentosRelacionados : Equipamentos
    {
        [Range(1, 1000, ErrorMessage = "Escolha uma opção válida")]
        public int IdDoEquipamentoRelacionado { get; set; }
        public string NomeDoEquipamentoRelacionado { get; set; }
    }
}