using System;
using System.Collections.Generic;

namespace Facturas.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int ClienteId { get; set; }

    public string NumeroFacturaElectronica { get; set; } = null!;

    public DateOnly FechaEmision { get; set; }

    public decimal MontoTotal { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
