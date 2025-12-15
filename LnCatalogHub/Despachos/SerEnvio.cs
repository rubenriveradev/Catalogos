using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LnCatalogHub.Despachos
{
    public class SerEnvio
    {
        private readonly string cadenaConexion;
        public SerEnvio(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("CadenaCON");
        }

        public async Task<Response<List<EnvioModel>>> ConsultarEnvios(int idDespacho)
        {
            List<EnvioModel> lsRetorno = new List<EnvioModel>();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { IdDespacho = idDespacho };

            try
            {
                var resp = await connection.QueryAsync<EnvioModel>("dbo.Get_SalesOrder", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<List<EnvioModel>>(true, "sin resultados", null);
                }
                else
                {
                    lsRetorno = resp.ToList();
                    return new Response<List<EnvioModel>>(false, "", lsRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<EnvioModel>>(true, ex.Message, null);
            }
        }

        public async Task<Response<RespuestaBD>> GuardarEnvios(EnvioModel env)
        {
            RespuestaBD objRetorno = new RespuestaBD();

            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new
            {
                env.IdDespacho,
                env.FechaDespacho,
                env.IdEstado,
                env.UsserId
            };

            try
            {
                var resp = await connection.QueryAsync<RespuestaBD>(
                    "dbo.CreateModify_SalesOrder", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<RespuestaBD>(true, "sin resultados", null);
                }
                else
                {
                    objRetorno = resp.FirstOrDefault();
                    return new Response<RespuestaBD>(false, "", objRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<RespuestaBD>(true, ex.Message, null);
            }
        }

        public async Task<Response<RespuestaBD>> CreaActualizaEnvioDetalle(int IdDetalle, int IdDespacho, int IdProduct, int IdEstado, int IdUser)
        {
            RespuestaBD objRetorno = new RespuestaBD();

            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new
            {
                IdDetalle,
                IdDespacho,
                IdProduct,
                IdEstado,
                IdUser
            };

            try
            {
                var resp = await connection.QueryAsync<RespuestaBD>(
                    "dbo.CreateModify_SalesOrderDetail", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<RespuestaBD>(true, "sin resultados", null);
                }
                else
                {
                    objRetorno = resp.FirstOrDefault();
                    return new Response<RespuestaBD>(false, "", objRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<RespuestaBD>(true, ex.Message, null);
            }
        }


        public async Task<Response<List<EnvioDetailModel>>> ConsultarEnvioDetalle(int idEnvio)
        {
            List<EnvioDetailModel> lsRetorno = new List<EnvioDetailModel>();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { IdEnvio = idEnvio };

            try
            {
                var resp = await connection.QueryAsync<EnvioDetailModel>("dbo.Get_ProductsSalesOrder", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<List<EnvioDetailModel>>(true, "sin resultados", null);
                }
                else
                {
                    lsRetorno = resp.ToList();
                    return new Response<List<EnvioDetailModel>>(false, "", lsRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<EnvioDetailModel>>(true, ex.Message, null);
            }
        }

        public async Task<Response<RespuestaBD>> EliminarEnvioDetalle(int IdDetalle)
        {
            RespuestaBD objRetorno = new RespuestaBD();

            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { IdDetalle };

            try
            {
                var resp = await connection.QueryAsync<RespuestaBD>(
                    "dbo.Delete_SalesOrderDetail", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<RespuestaBD>(true, "sin resultados", null);
                }
                else
                {
                    objRetorno = resp.FirstOrDefault();
                    return new Response<RespuestaBD>(false, "", objRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<RespuestaBD>(true, ex.Message, null);
            }
        }

        public async Task<Response<List<EnvioDetailModel>>> ConsultarProductosHistorico(int idProducto)
        {
            List<EnvioDetailModel> lsRetorno = new List<EnvioDetailModel>();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { IdProduct = idProducto };

            try
            {
                var resp = await connection.QueryAsync<EnvioDetailModel>("dbo.Get_ProductsSalesOrderHistory", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<List<EnvioDetailModel>>(true, "sin resultados", null);
                }
                else
                {
                    lsRetorno = resp.ToList();
                    return new Response<List<EnvioDetailModel>>(false, "", lsRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<EnvioDetailModel>>(true, ex.Message, null);
            }
        }



    }
}
