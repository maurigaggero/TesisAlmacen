﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAlmacen.Data.Entities
{
    public class DetalleVentas
    {
        public int Id { get; set; }
        public int Articulo_Id { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int Venta_Id { get; set; }
        public decimal SubTotal { get; set; }

        public Articulos Articulo { get; set; } = new();  
    }
}
