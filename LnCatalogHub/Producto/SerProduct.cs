using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LnCatalogHub.Producto
{
    public class SerProduct
    {
        private readonly string cadenaConexion;
        public SerProduct(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("CadenaCON");
        }

        public async Task<Response<List<Products>>> ConsultarProductos(int IdProducto)
        {
            List<Products> lsRetorno = new List<Products>();


            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { IdProduct = IdProducto };

            try
            {
                var resp = await connection.QueryAsync<Products>(
                    "dbo.GetProducts",
                    parametros,
                    commandType: CommandType.StoredProcedure
                    );
                if (resp == null)
                {
                    return new Response<List<Products>>(true, "sin resultados", null);
                }
                else
                {
                    lsRetorno = resp.ToList();
                    return new Response<List<Products>>(false, "", lsRetorno);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<Products>>(true, ex.Message, null);
            }
        }


        public async Task<Response<RespuestaBD>> GuardarProductos(Products prod)
        {
            RespuestaBD objRetorno = new RespuestaBD();


            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new {
                prod.IdProduct,
                prod.ItemID,
                prod.ProductName,
                prod.UPC,
                prod.Store,
                prod.Mattel,
                prod.CostPrice,
                prod.SalePrice,
                prod.WFS,
                prod.StorageFee_WFS,
                prod.PercentageCommission,
                prod.Commission,
                prod.Weight,
                prod.Length,
                prod.Width,
                prod.Height,
                prod.IsActive,
                prod.ImageData,
                prod.UserId
            };

            try
            {
                var resp = await connection.QueryAsync<RespuestaBD>(
                    "dbo.CrearModificarProducto",
                    parametros,
                    commandType: CommandType.StoredProcedure
                    );
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
