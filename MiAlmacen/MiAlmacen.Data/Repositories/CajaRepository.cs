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
    public class CajaRepository : DBConex
    {
        string orden;
        private static Caja IniciarObjeto(CajaModel model)
        {
            Caja ca = new();
            ca.Fecha = model.Fecha;
            ca.Empleado_Id = model.Empleado_Id;
            ca.Apertura = model.Apertura;
            ca.Cierre = model.Cierre;
            ca.FechaCierre = model.FechaCierre;
            ca.Actual = model.Actual;
            Usuarios emp = new();
            emp.Id = model.Empleado.Id;
            emp.Nombre = model.Empleado.Nombre;
            emp.Email = model.Empleado.Email;
            emp.Usuario = model.Empleado.Usuario;
            emp.Contraseña = model.Empleado.Contraseña;
            ca.Empleado = emp;
            return ca;
        }

        public List<Caja> GetAll()
        {
            orden = $"SELECT * FROM Caja ORDER BY Nombre ASC";
            List<Caja> cajas = new();

            SqlCommand sqlcmd = new(orden, conexion);
            try
            {
                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    Caja ca = new();
                    ca.Id = Convert.ToInt32(reader["Id"].ToString());
                    ca.Fecha = Convert.ToDateTime(reader["Fecha"].ToString());
                    ca.Empleado_Id = Convert.ToInt32(reader["Empleado_Id"].ToString());
                    ca.Apertura = Convert.ToDecimal(reader["Apertura"].ToString());
                    ca.Actual = Convert.ToDecimal(reader["Actual"].ToString());
                    ca.Cierre = Convert.ToDecimal(reader["Cierre"].ToString());
                    ca.FechaCierre = !string.IsNullOrEmpty(reader["FechaCierre"].ToString()) ? Convert.ToDateTime(reader["FechaCierre"].ToString()) : null;

                    Usuarios usuario = new();
                    UsuarioRepository usuarioRepository = new();
                    usuario = usuarioRepository.GetOne(Convert.ToInt32(reader["Empleado_Id"].ToString()));
                    ca.Empleado = usuario;

                    cajas.Add(ca);
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
            return cajas;
        }

        public Caja GetLast()
        {
            orden = @$"SELECT TOP 1 * FROM Caja
                        ORDER BY Fecha DESC";
            SqlCommand sqlcmd = new(orden, conexion);
            Caja caja = new();
            try
            {
                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    caja.Id = Convert.ToInt32(reader["Id"].ToString());
                    caja.Fecha = Convert.ToDateTime(reader["Fecha"].ToString());
                    caja.Empleado_Id = Convert.ToInt32(reader["Empleado_Id"].ToString());
                    caja.Apertura = Convert.ToDecimal(reader["Apertura"].ToString());
                    caja.Actual = Convert.ToDecimal(reader["Actual"].ToString());
                    caja.Cierre = Convert.ToDecimal(reader["Cierre"].ToString());
                    caja.FechaCierre = !string.IsNullOrEmpty(reader["FechaCierre"].ToString()) ? Convert.ToDateTime(reader["FechaCierre"].ToString()) : null;

                    Usuarios usuario = new();
                    UsuarioRepository usuarioRepository = new();
                    usuario = usuarioRepository.GetOne(Convert.ToInt32(reader["Empleado_Id"].ToString()));
                    caja.Empleado = usuario;
                }

                CerrarConex();
            }
            catch (Exception e)
            {
                throw new Exception("Error al tratar de ejecutar la operación " + e.Message);
            }
            finally
            {
                CerrarConex();
                sqlcmd.Dispose();
            }
            return caja;
        }

        public Caja GetOne(int id)
        {
            orden = $"SELECT * FROM Caja WHERE Id ={id}";
            SqlCommand sqlcmd = new(orden, conexion);
            Caja caja = new();
            try
            {
                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    caja.Id = Convert.ToInt32(reader["Id"].ToString());
                    caja.Fecha = Convert.ToDateTime(reader["Fecha"].ToString());
                    caja.Empleado_Id = Convert.ToInt32(reader["Empleado_Id"].ToString());
                    caja.Apertura = Convert.ToDecimal(reader["Apertura"].ToString());
                    caja.Actual = Convert.ToDecimal(reader["Actual"].ToString());
                    caja.Cierre = Convert.ToDecimal(reader["Cierre"].ToString());
                    caja.FechaCierre = !string.IsNullOrEmpty(reader["FechaCierre"].ToString()) ? Convert.ToDateTime(reader["FechaCierre"].ToString()) : null;

                    Usuarios usuario = new();
                    UsuarioRepository usuarioRepository = new();
                    usuario = usuarioRepository.GetOne(Convert.ToInt32(reader["Empleado_Id"].ToString()));
                    caja.Empleado = usuario;
                }

                CerrarConex();
            }
            catch (Exception e)
            {
                throw new Exception("Error al tratar de ejecutar la operación " + e.Message);
            }
            finally
            {
                CerrarConex();
                sqlcmd.Dispose();
            }
            return caja;
        }

        public Caja Post(CajaModel model)
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
                Caja caja = IniciarObjeto(model);

                try
                {
                    sqlcmd.Connection = conexion;
                    sqlcmd.Transaction = transaction;

                    orden = @"INSERT INTO Caja (Fecha, Empleado_Id, Apertura, Cierre, Actual)
                            VALUES (@Fecha, @Empleado_Id, @Apertura, @Cierre, @Actual) ";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@Empleado_Id", caja.Empleado_Id);
                    sqlcmd.Parameters.AddWithValue("@Apertura", caja.Apertura);
                    sqlcmd.Parameters.AddWithValue("@Actual", caja.Apertura); //El monto de apertura es el monto actual al momento de crear
                    sqlcmd.Parameters.AddWithValue("@Cierre", 0);

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

                return caja;
            }
        }
        public Caja Put(int id, CajaModel model)
        {
            Caja ca = GetOne(id);
            if (ca == null || model == null)
            {
                throw new Exception("Error al tratar de ejecutar la operación");
            }
            else
            {
                Caja caja = IniciarObjeto(model);
                SqlCommand sqlcmd = new(orden, conexion);
                try
                {
                    AbrirConex();
                    orden = $@"UPDATE Caja 
                               SET Cierre=@Cierre, FechaCierre=@FechaCierre WHERE Id=@Id";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@Id", id);
                    sqlcmd.Parameters.AddWithValue("@Cierre", caja.Cierre);
                    sqlcmd.Parameters.AddWithValue("@FechaCierre", caja.Cierre);

                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw new Exception("Error al tratar de ejecutar la operación " + e.Message);
                }
                finally
                {
                    CerrarConex();
                    sqlcmd.Dispose();
                }
                return caja;
            }
        }
    }
}
