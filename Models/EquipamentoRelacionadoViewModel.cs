using System.Collections.Generic;

namespace ConfiguradorDeComponents.Models
{
    public class EquipamentoRelacionadoViewModel
    {
        public Equipamentos Equipamento { get; set; }

        public IEnumerable<Equipamentos> EquipamentosLista { get; set; }
    }
}