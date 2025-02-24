﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAlmacen.Data.Entities
{
    public class Ventas
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Cliente_Id { get; set; }
        public int Empleado_Id { get; set; }
        public decimal Total { get; set; }
        public decimal Saldo { get; set; }
        public DateTime? Fecha_Baja { get; set; }

        public List<DetalleVentas> Detalle { get; set; } = new();
        public Clientes Cliente { get; set; } = new ();
        public Usuarios Empleado { get; set; } = new ();
        public List<FormaPagoVentas> FormasPago { get; set; } = new();
    }
}
