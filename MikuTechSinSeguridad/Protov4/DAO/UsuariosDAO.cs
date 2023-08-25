using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Protov4.DTO;
using Protov4.DAO;

namespace Protov4.DAO
{
    public class UsuariosDAO : DbConnection
    {

        public UsuariosDAO(IConfiguration configuration) : base(configuration)
        {
            // Constructor que llama al constructor de la clase base (DbConnection) pasando la configuración.
            // Esto asegura que el objeto de conexión se inicialice correctamente.
        }

        public ((int id_usuario, int id_rol_user), int id_cliente) ValidarUsuario(UsuariosDTO nuser)
        {
            int id_usuario = 0;
            int id_cliente = 0;
            int id_rol_user = 0;

            nuser.contrasena = ConvertirSha256(nuser.contrasena);
            using (var connection = GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT id_usuario, id_rol_user FROM Usuarios WHERE correo_elec = '" + nuser.correo_elec + "' AND contrasena = '" + nuser.contrasena + "'", connection);


                using (var reader = cmd.ExecuteReader())
                {
                    

                    if (reader.Read())
                    {
                        id_usuario = Convert.ToInt32(reader["id_usuario"]);
                        id_rol_user = Convert.ToInt32(reader["id_rol_user"]);
                       
                    }

                    return ((id_usuario, id_rol_user), id_cliente);
                }
            }

        }

        public bool Registrar(ClientesDTO nclient)
        {
            bool registrado = false;
            try
            {
                nclient.contrasena_nueva = ConvertirSha256(nclient.contrasena_nueva);

                using (var connection = GetSqlConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Login_RegistrarUsuario", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@correo_elec", SqlDbType.VarChar).Value = nclient.correo_nuevo;
                        cmd.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = nclient.contrasena_nueva;
                        cmd.Parameters.Add("@nombre_cliente", SqlDbType.VarChar).Value = nclient.nombre_cliente;
                        cmd.Parameters.Add("@apellido_cliente", SqlDbType.VarChar).Value = nclient.apellido_cliente;
                        cmd.Parameters.Add("@telefono_cliente", SqlDbType.VarChar).Value = nclient.telefono_cliente;
                        cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                    }
                }
            }
            catch (Exception)
            {
                // Puedes manejar el error aquí si es necesario.
            }

            return registrado;
        }

        public void RegistrarAuditoria(int idUsuario, DateTime fechaSesion, bool esInicioSesion)
        {
            try
            {
                using (var connection = GetSqlConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("Login_RegistrarAuditoria", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                        // Si esInicioSesion es verdadero, registramos la fecha de inicio de sesión
                        if (esInicioSesion)
                        {
                            cmd.Parameters.AddWithValue("@fecha_inicio_sesion", fechaSesion);
                            cmd.Parameters.AddWithValue("@fecha_cierre_sesion", DBNull.Value); // Valor nulo para cierre
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@fecha_inicio_sesion", DBNull.Value); // Valor nulo para inicio
                            cmd.Parameters.AddWithValue("@fecha_cierre_sesion", fechaSesion); // Registramos fecha de cierre
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Agregar manejo de errores aquí si es necesario
                Console.WriteLine("Error en RegistrarAuditoria: " + ex.Message);
            }
        }

        public List<MisPedidosDTO> ListarPedidos(int id_cliente)
        {
            try
            {
                var Lista = new List<MisPedidosDTO>();

                using (var connection = GetSqlConnection())
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("ObtenerDatosPedido", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new MisPedidosDTO
                            {
                                ciudad_envio = dr["ciudad_envio"].ToString(),
                                fecha_pedido = ((DateTime)dr["fecha_pedido"]).ToString("dd/MM/yyyy"),
                                nombre_pago = dr["nombre_pago"].ToString(),
                                pago_total = dr.IsDBNull(dr.GetOrdinal("pago_total")) ? null : (decimal?)dr.GetDecimal(dr.GetOrdinal("pago_total")),
                                nombre_estado = dr["nombre_estado"].ToString()
                            });
                        }
                    }

                }
                return Lista;
            }
            catch (Exception ex)
            {
                // Agregar manejo de errores aquí si es necesario
                Console.WriteLine("Error en RegistrarAuditoria: " + ex.Message);
                return new List<MisPedidosDTO>();
            }
        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

    }
}

