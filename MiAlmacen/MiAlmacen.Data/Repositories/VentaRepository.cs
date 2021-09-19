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
    public class VentaRepository : DBConex
    {
        string orden;
        //private static Ventas IniciarObjeto(VentaModel model)
        //{
        //    Ventas venta = new();
        //    venta.Id = model.Id;
        //    venta.Fecha = model.Fecha;
        //    venta.FormaPago = model.FormaPago;
        //    venta.Cliente_Id = model.Cliente_Id;
        //    venta.Empleado_Id = model.Empleado_Id;
        //    venta.Total = model.Total;
        //    venta.Saldo = model.Saldo;
        //    venta.Fecha_Baja = model.Fecha_Baja;

        //    foreach (var item in model.DetalleVenta)
        //    {
        //        DetalleVentas detventa = new();
        //        detventa.Venta_Id = item.Venta_Id;
        //        detventa.Articulo_Id = item.Articulo_Id;
        //        detventa.Precio = item.Precio;
        //        detventa.Cantidad = item.Cantidad;

        //        venta.Detalle.Add(detventa);
        //    }

        //    return venta;
        //}

        public List<Ventas> GetAll()
        {
            SqlCommand sqlcmd = new(orden, conexion);

            List<Ventas> ventas = new();

            try
            {
                orden = @"SELECT * FROM Ventas ORDER BY Fecha DESC";

                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    Ventas venta = new();
                    venta.Id = Convert.ToInt32(reader["Id"].ToString());
                    venta.Fecha = Convert.ToDateTime(reader["Fecha"].ToString());
                    venta.FormaPago = reader["FormaPago"].ToString();
                    venta.Cliente_Id = Convert.ToInt32(reader["Cliente_Id"].ToString());
                    venta.Empleado_Id = Convert.ToInt32(reader["Empleado_Id"].ToString());
                    venta.Total = Convert.ToSingle(reader["Total"].ToString());
                    venta.Saldo = Convert.ToSingle(reader["Saldo"].ToString());
                    venta.Fecha_Baja = string.IsNullOrEmpty(reader["FechaBaja"].ToString()) ? null : Convert.ToDateTime(reader["FechaBaja"]);
                    ventas.Add(venta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CerrarConex();
                sqlcmd.Dispose();
            }
            return ventas;
        }

        public Ventas GetOne(int id)
        {
            orden = $"SELECT * FROM Ventas WHERE Id ={id}";
            SqlCommand sqlcmd = new(orden, conexion);
            Ventas venta = new();
            try
            {
                AbrirConex();
                sqlcmd.CommandText = orden;
                SqlDataReader reader = sqlcmd.ExecuteReader();

                while (reader.Read())
                {
                    venta.Id = Convert.ToInt32(reader["Id"].ToString());
                    venta.Fecha = Convert.ToDateTime(reader["Fecha"].ToString());
                    venta.FormaPago = reader["FormaPago"].ToString();
                    venta.Cliente_Id = Convert.ToInt32(reader["Cliente_Id"].ToString());
                    venta.Empleado_Id = Convert.ToInt32(reader["Empleado_Id"].ToString());
                    venta.Total = Convert.ToSingle(reader["Total"].ToString());
                    venta.Saldo = Convert.ToSingle(reader["Saldo"].ToString());
                    venta.Fecha_Baja = string.IsNullOrEmpty(reader["FechaBaja"].ToString()) ? null : Convert.ToDateTime(reader["FechaBaja"]);
                }

                DetalleVentaRepository detalleVentasRepository = new DetalleVentaRepository();
                venta.Detalle.AddRange(detalleVentasRepository.GetAll(venta));
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
            return venta;
        }

        public Ventas Post(Ventas venta)
        {
            if (venta == null)
            {
                throw new Exception("Error al tratar de ejecutar la operación");
            }
            else
            {
                AbrirConex();

                SqlTransaction transaction;
                transaction = conexion.BeginTransaction();
                SqlCommand sqlcmd = new(orden, conexion, transaction);

                try
                {
                    sqlcmd.Connection = conexion;
                    sqlcmd.Transaction = transaction;

                    orden = @"INSERT INTO Ventas (FormaPago, Fecha, Cliente_Id, Empleado_Id, Total, Saldo)
                            VALUES (@FormaPago, @Fecha, @Cliente_Id, @Empleado_Id, @Total, @Saldo) ";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@FormaPago", venta.FormaPago);
                    sqlcmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                    sqlcmd.Parameters.AddWithValue("@Cliente_Id", venta.Cliente_Id);
                    sqlcmd.Parameters.AddWithValue("@Empleado_Id", venta.Empleado_Id);
                    sqlcmd.Parameters.AddWithValue("@Total", venta.Total);
                    sqlcmd.Parameters.AddWithValue("@Saldo", venta.Saldo);

                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.Parameters.Clear();
                    
                    foreach (var detalle in venta.Detalle)
                    {
                        orden = @"INSERT INTO DetalleVentas (Articulo_Id, Precio, Cantidad, Venta_Id)
                            VALUES (@Articulo_Id, @Precio, @Cantidad, (SELECT IDENT_CURRENT ('Ventas')))";

                        sqlcmd.CommandText = orden;
                        sqlcmd.Parameters.AddWithValue("@Articulo_Id", detalle.Articulo_Id);
                        sqlcmd.Parameters.AddWithValue("@Precio", detalle.Precio);
                        sqlcmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);

                        sqlcmd.ExecuteNonQuery();
                        sqlcmd.Parameters.Clear();

                        orden = $"SELECT Stock_Act FROM Articulos WHERE Id = {detalle.Articulo_Id};";
                        sqlcmd.CommandText = orden;
                        sqlcmd.ExecuteNonQuery();

                        SqlDataReader reader = sqlcmd.ExecuteReader();

                        int stock = 0;
                        if (reader.Read())
                        {
                            stock = Convert.ToInt32(reader["Stock_Act"].ToString());
                            stock -= detalle.Cantidad;
                            reader.Close();
                        }

                        orden = $@"UPDATE Articulos 
                                  SET Stock_Act = @Stock_Act 
                                  WHERE Id = @Id";

                        sqlcmd.CommandText = orden;
                        sqlcmd.Parameters.AddWithValue("@Id", detalle.Articulo_Id);
                        sqlcmd.Parameters.AddWithValue("@Stock_Act", stock);

                        sqlcmd.ExecuteNonQuery();
                        sqlcmd.Parameters.Clear();
                    }

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
                return venta;
            }
        }

        public float PutSaldo(int id, float pago)
        {
            var venta = GetOne(id);
            if (venta != null)
            {
                AbrirConex();

                SqlTransaction transaction;
                transaction = conexion.BeginTransaction();
                SqlCommand sqlcmd = new(orden, conexion, transaction);

                float nuevoSaldo = venta.Saldo - pago;
                try
                {
                    orden = $"UPDATE Ventas SET Saldo=@Saldo WHERE Id=@Id";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@Id", id);
                    sqlcmd.Parameters.AddWithValue("@Saldo", nuevoSaldo);

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
                return nuevoSaldo;
            }
            else
                return 0;
        }

        public int Delete(int id)
        {
            var venta = GetOne(id);
            if (venta != null)
            {
                AbrirConex();

                SqlTransaction transaction;
                transaction = conexion.BeginTransaction();
                SqlCommand sqlcmd = new(orden, conexion, transaction);

                try
                {
                    orden = $"UPDATE Ventas SET FechaBaja=@FechaBaja WHERE Id=@Id";

                    sqlcmd.CommandText = orden;
                    sqlcmd.Parameters.AddWithValue("@Id", id);
                    sqlcmd.Parameters.AddWithValue("@FechaBaja", DateTime.Now);

                    sqlcmd.ExecuteNonQuery();
                    sqlcmd.Parameters.Clear();

                    foreach (var item in venta.Detalle)
                    {
                        orden = $"SELECT Stock_Act FROM Articulos WHERE Id = {item.Articulo_Id}";
                        sqlcmd.CommandText = orden;
                        sqlcmd.ExecuteNonQuery();

                        SqlDataReader reader = sqlcmd.ExecuteReader();

                        int stock = 0;
                        if (reader.Read())
                        {
                            stock = Convert.ToInt32(reader["Stock_Act"].ToString());
                            stock += item.Cantidad;
                            reader.Close();
                        }

                        orden = $@"UPDATE Articulos 
                                  SET Stock_Act = @Stock_Act 
                                  WHERE Id = @Id";

                        sqlcmd.CommandText = orden;
                        sqlcmd.Parameters.AddWithValue("@Id", item.Articulo_Id);
                        sqlcmd.Parameters.AddWithValue("@Stock_Act", stock);

                        sqlcmd.ExecuteNonQuery();
                        sqlcmd.Parameters.Clear();
                    }

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
            }
            else
            {
                id = 0;
            }
            return id;
        }
    }
}