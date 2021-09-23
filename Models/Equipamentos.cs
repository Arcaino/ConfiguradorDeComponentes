namespace ConfiguradorDeComponents.Models
{
    public class Equipamentos : TipoEquipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int NumeroDeSerie { get; set; }
        public string DataCadastro { get; set; }
    }

    public class TipoEquipamento
    {
        public int IdDoTipoDeEquipamento { get; set; }
        public string NomeDoTipoDeEquipamento { get; set; }
    }
}