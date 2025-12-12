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
                var resp = await connection.QueryAsync<EnvioModel>("dbo.GetEnvios",parametros,commandType: CommandType.StoredProcedure);
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

        public async Task<Response<RespuestaBD>> Guardarenvios(EnvioModel env)
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
                    "dbo.CreaActualizaEnvios", parametros,commandType: CommandType.StoredProcedure);
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


    }
}
