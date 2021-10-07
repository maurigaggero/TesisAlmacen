﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAlmacen.Data.Entities
{
    public class Articulos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public long Codigo_Art { get; set; }
        public decimal Precio_Unit { get; set; }
        public decimal Precio_Mayor { get; set; }
        public int Stock_Act { get; set; }
        public DateTime Ultima_Modif { get; set; }
        public DateTime? FechaBaja { get; set; }

    }
}
