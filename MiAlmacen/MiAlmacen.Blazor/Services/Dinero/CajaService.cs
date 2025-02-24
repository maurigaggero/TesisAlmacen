﻿using MiAlmacen.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiAlmacen.Blazor.Services
{
    public class CajaService
    {
        private readonly HttpClient _httpClient;
        public CajaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CajaModel>> GetAll()
        {
            var respuesta = _httpClient.GetStringAsync($"api/caja/getall");
            return JsonConvert.DeserializeObject<IEnumerable<CajaModel>>(await respuesta);
        }


        public async Task<CajaModel> GetUn(int id)
        {
            var respuesta = _httpClient.GetStringAsync($"api/caja/{id}");
            return JsonConvert.DeserializeObject<CajaModel>(await respuesta);
        }

        public async Task<CajaModel> GetUltimo(int id)
        {
            var respuesta = _httpClient.GetStringAsync($"api/caja/last/{id}");
            return JsonConvert.DeserializeObject<CajaModel>(await respuesta);
        }

        public async Task<HttpResponseMessage> Alta(CajaModel caja)
        {
            string cajaSerealizada = JsonConvert.SerializeObject(caja);
            var respuesta = await _httpClient.PostAsync("api/caja/",
                            new StringContent(cajaSerealizada, UnicodeEncoding.UTF8, "application/json"));
            return respuesta;
        }

        public async Task<HttpResponseMessage> Editar(CajaModel caja)
        {
            string cajaSerealizada = JsonConvert.SerializeObject(caja);
            var respuesta = await _httpClient.PutAsync($"api/caja/{caja.Id}", 
                            new StringContent(cajaSerealizada, UnicodeEncoding.UTF8, "application/json"));
            return respuesta;
        }

        public async Task<IngresoModel> GetIngresos(int idcaja)
        {
            var respuesta = _httpClient.GetStringAsync($"api/caja/ingresos/{idcaja}");
            return JsonConvert.DeserializeObject<IngresoModel>(await respuesta);
        }
    }
}
