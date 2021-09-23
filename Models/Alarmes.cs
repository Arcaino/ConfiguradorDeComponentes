namespace ConfiguradorDeComponents.Models
{
    public class Alarmes : ClassificacaoAlarmes
    {
        public int IdAlarme { get; set; }
        public string DescricaoAlarme { get; set; }
        public string DataDeCadastroAlarme { get; set; }
    }
    public class ClassificacaoAlarmes : Equipamentos
    {
        public int IdClassificacaoDoAlarme { get; set; }
        public string NomeClassificacaoAlarme { get; set; }
    }
}