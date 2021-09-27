using System.ComponentModel.DataAnnotations;

namespace ConfiguradorDeComponents.Models
{
    public class Equipamentos : TipoEquipamento
    {
        public int Id { get; set; }
        [StringLength(50)] 
        [Required(ErrorMessage = "O preenchimento do campo é necessário")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O preenchimento do campo é necessário")]
        [Range(1, 1000, ErrorMessage = "O número de série deve estar entre 1 e 1000")]
        public int NumeroDeSerie { get; set; }
        public string DataCadastro { get; set; }
        [StringLength(200)] 
        [Required(ErrorMessage = "O preenchimento do campo é necessário")]
        public string Descricao { get; set; }
    }

    public class TipoEquipamento
    {
        [Range(1, 1000, ErrorMessage = "Escolha uma opção válida")]
        public int IdDoTipoDeEquipamento { get; set; }
        public string NomeDoTipoDeEquipamento { get; set; }
    }
}