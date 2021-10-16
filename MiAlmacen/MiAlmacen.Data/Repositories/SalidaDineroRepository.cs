﻿using MiAlmacen.Data.Conection;
using MiAlmacen.Data.Entities;
using MiAlmacen.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiAlmacen.Data.Repositories
{
    public class SalidaDineroRepository : DBConex
    {
        string orden;
        private static SalidasDinero IniciarObjeto(SalidasDineroModel model)
        {
            CajaRepository cajaRepository = new();

            SalidasDinero sd = new();
            sd.Id = model.Id;
            sd.Descripcion = model.Descripcion;
            sd.Importe = model.Importe;
            sd.Caja_Id = model.Caja_Id;
            sd.Caja = cajaRepository.GetOne(model.Caja_Id);

            return sd;
        }

        public List<SalidasDinero> GetAll()
        {
            orden = $"SELECT * FROM SalidasDinero ORDER BY Id ASC";
            List<SalidasDinero> salidas = new();

            SqlCommand sqlcmd = new(orden, conexion);
            try
            {
                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    SalidasDinero sa = new();
                    SalidasDinero sd = new();
                    sd.Id = Convert.ToInt32(reader["Id"].ToString());
                    sd.Descripcion = reader["Descripcion"].ToString();
                    sd.Importe = Convert.ToDecimal(reader["Importe"].ToString());
                    sd.Caja_Id = Convert.ToInt32(reader["Caja_Id"].ToString());

                    Caja caja = new();
                    CajaRepository cajaRepository = new();
                    caja = cajaRepository.GetOne(Convert.ToInt32(reader["Caja_Id"].ToString()));
                    sd.Caja = caja;

                    salidas.Add(sd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al tratar de ejecutar la operación " + ex.Message);
            }
            finally
            {
                CerrarConex();
                sqlcmd.Dispose();
            }
            return salidas;
        }

        public SalidasDinero Post(SalidasDineroModel model)
        {
            if (model == null)
            {
                throw new Exception("Error al tratar de ejecutar la operación");
            }
            else
            {
                AbrirConex();

                SqlTransaction transaction;
                transaction = conexion.BeginTransaction();
                SqlCommand sqlcmd = new(orden, conexion, transaction);
                SalidasDinero salida = IniciarObjeto(model);

                try
                {
                    sqlcmd.Connection = conexion;
                    sqlcmd.Transaction = transaction;

                    orden = @"INSERT INTO SalidasDinero (Descripcion, Importe, Caja_Id)
                            VALUES (@Descripcion, @Importe, @Caja_Id) ";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@Descripcion", salida.Descripcion);
                    sqlcmd.Parameters.AddWithValue("@Importe", salida.Importe);
                    sqlcmd.Parameters.AddWithValue("@Caja_Id", salida.Caja_Id);

                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.Parameters.Clear();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    throw new Exception("Error al tratar de ejecutar la operación " + e.Message);
                }
                finally
                {
                    CerrarConex();
                    sqlcmd.Dispose();
                }

                return salida;
            }
        }
    }
}
