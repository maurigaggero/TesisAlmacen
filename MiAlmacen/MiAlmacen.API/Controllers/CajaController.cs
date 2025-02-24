﻿using MiAlmacen.Data.Repositories;
using MiAlmacen.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiAlmacen.API.Controllers
{
    [Route("api/caja")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly CajaRepository _repository;
        public CajaController(CajaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var cajas = _repository.GetAll();
            return Ok(cajas);
        }

        [HttpGet("last/{id}")]
        public async Task<IActionResult> GetLast(int id)
        {
            var caja = _repository.GetLast(id);
            return Ok(caja);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var caja = _repository.GetOne(id);
            return Ok(caja);
        }

        [HttpGet("ingresos/{idcaja}")]
        public async Task<IActionResult> GetIngreso(int idcaja)
        {
            return Ok(_repository.IngresosXfp(idcaja));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CajaModel model)
        {
            return Ok(_repository.Post(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CajaModel model)
        {
            return Ok(_repository.Put(id, model));
        }

    }
}
