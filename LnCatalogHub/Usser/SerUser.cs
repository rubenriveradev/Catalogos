using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LnCatalogHub.Login
{
    public class SerUser
    {
        private readonly string cadenaConexion;
        public SerUser(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("CadenaCON");
        }
        public async Task<Response<UserModel>> ValidarUsuarioAsync(string usuario, string clave)
        {
            UserModel usu = new UserModel();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { Usuario = usuario };

            try
            {
                var resp = await connection.QueryFirstOrDefaultAsync<UserModel>(
                    "dbo.spValidarUsuario",
                    parametros,
                    commandType: CommandType.StoredProcedure
                    );
                if (resp == null)
                {
                    return new Response<UserModel>(true, "Usuario no existe", null);
                }
                else
                {
                    string mensajeError = ValidarUsuario(resp, clave);

                    if (!string.IsNullOrWhiteSpace(mensajeError))
                    {
                        return new Response<UserModel>(true, mensajeError, null);
                    }

                    return new Response<UserModel>(false, "", resp);
                }
            }
            catch (Exception ex)
            {
                return new Response<UserModel>(true, ex.Message, null);
            }
        }


        private string ValidarUsuario(UserModel usu, string passIngreso)
        {

            if (usu.Password != passIngreso)
                return "Contraseña incorrecta";

            if (!usu.IsActive)
                return "Usuario inactivo";

            usu.Password = string.Empty; // Limpiar la contraseña antes de devolver el usuario
            return string.Empty;
        }


        public async Task<Response<List<UserModel>>> ConsultarUsuarios()
        {
            UserModel usu = new UserModel();
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new { };

            try
            {
                var resp = await connection.QueryAsync<UserModel>(
                    "dbo.spConsultarUsuarios",
                    parametros,
                    commandType: CommandType.StoredProcedure
                    );



                if (resp == null)
                {
                    return new Response<List<UserModel>>(true, "No existen usuarios", null);
                }
                else
                {
                    return new Response<List<UserModel>>(false, "", resp.ToList());
                }
            }
            catch (Exception ex)
            {
                return new Response<List<UserModel>>(true, ex.Message, null);
            }
        }


        public async Task<Response<string>> GuardarUsuario(UserModel usuario)
        {
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new
            {
                IdUsuario = usuario.UserId,
                IdTipoIdentificacion = usuario.IdentificationTypeId,
                Identificacion = usuario.Identification,
                Nombres = usuario.FirstName,
                Apellidos = usuario.LastName,
                UserName = usuario.UserName,
                IdRol = usuario.RoleId,
                Activo = usuario.IsActive,
                usuLog = usuario.UserId                
            };
            try
            {
                var resp = await connection.QueryFirstOrDefaultAsync<RespuestaBD>("dbo.spCreaEditaUsuario",
                    parametros, commandType: CommandType.StoredProcedure);

                if (resp == null)
                {
                    return new Response<string>(true, "Error al guardar el usuario", null);
                }

                if (resp.Codigo <= 0)
                {
                    return new Response<string>(true, resp.Descripcion, null);
                }

                return new Response<string>(false, null, resp.Descripcion);

            }
            catch (Exception ex)
            {
                return new Response<string>(true, ex.Message, null);
            }
        }
        //
        public async Task<Response<string>> CambiarContrasena(CambioPasswordDTO pas)
        {
            using var connection = new SqlConnection(cadenaConexion);
            var parametros = new
            {
                pas.IdUsuario,
                pas.Password,
                pas.Id_Usuario_Modifica
            };
            try
            {
                var resp = await connection.QueryFirstOrDefaultAsync<RespuestaBD>("spCambiarContrasena",
                    parametros, commandType: CommandType.StoredProcedure);

                if (resp == null)
                {
                    return new Response<string>(true, "Error al guardar el usuario", null);
                }

                if (resp.Codigo <= 0)
                {
                    return new Response<string>(true, resp.Descripcion, null);
                }

                return new Response<string>(false, null, resp.Descripcion);

            }
            catch (Exception ex)
            {
                return new Response<string>(true, ex.Message, null);
            }
        }




    }
}
