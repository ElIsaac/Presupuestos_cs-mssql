using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public interface IRepositorioTiposCuentas{
        Task Crear(TipoCuenta tipoCuenta);
    }
    public class RepositorioTiposCuentas:IRepositorioTiposCuentas{
        private readonly string connectionString;
        RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(TipoCuenta tipoCuenta){
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingle<int>($@"insert into TiposCuentas (Nombre, UsuarioId, Orden) values (@Nombre, @UsuarioId, 0);
                                                    Select SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id=id;
        }
    }
}