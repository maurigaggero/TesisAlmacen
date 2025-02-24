﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAlmacen.Model.Models
{
    public class DetalleVentaModel
    {
        public int Id { get; set; }
        public int Articulo_Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio.")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(0, 1000, ErrorMessage = "Solo números")]
        public int Cantidad { get; set; }

        public decimal SubTotal { get; set; }
        public int Venta_Id { get; set; }

        public ArticuloModel Articulo { get; set; }
    }
}
