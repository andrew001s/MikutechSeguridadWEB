using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Protov4.DTO
{
    public class MisPedidosDTO
    {
        public string? ciudad_envio { get; set; }
        public string? fecha_pedido { get; set; }
        public string? nombre_pago { get; set; }
        public decimal? pago_total { get; set; }
        public string? nombre_estado { get; set; }

    }
}
