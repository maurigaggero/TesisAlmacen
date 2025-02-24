﻿using MiAlmacen.Data.Repositories;
using MiAlmacen.Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiAlmacen.API.Controllers
{
    [Route("api/salida-dinero")]
    [ApiController]
    public class SalidaDineroController : ControllerBase
    {
        private readonly SalidaDineroRepository _repository;
        public SalidaDineroController(SalidaDineroRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{idcaja}")]
        public async Task<IActionResult> GetAll(int idcaja)
        {
            var salidas = _repository.GetAll(idcaja);
            return Ok(salidas);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SalidasDineroModel model)
        {
            return Ok(_repository.Post(model));
        }
    }
}
