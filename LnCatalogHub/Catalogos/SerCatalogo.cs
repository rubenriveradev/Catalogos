using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LnCatalogHub.Catalogos
{
    public class SerCatalogo
    {
        private readonly string cadenaConexion;
        public SerCatalogo(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("CadenaCON");
        }

        public async Task<Response<List<CatalogosModel>>> ConsultarCatalogo(string key)
        {
            List<CatalogosModel> lsRetorno = new List<CatalogosModel>();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { key };

            try
            {
                var resp = await connection.QueryAsync<CatalogosModel>("dbo.Get_MasterList", parametros, commandType: CommandType.StoredProcedure);
                if (resp == null)
                {
                    return new Response<List<CatalogosModel>>(true, "sin resultados", null);
                }
                else
                {
                    lsRetorno = resp.ToList();
                    return new Response<List<CatalogosModel>>(false, "", lsRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<CatalogosModel>>(true, ex.Message, null);
            }
        }




    }
}
