using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);

        Task<Cuenta> ObtenerPorId(int id, int usuarioId);

        Task ActualizarCuenta(CuentasCreacionViewModel cuenta);
        Task Borrar(Cuenta cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance) 
        VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
        SELECT SCOPE_IDENTITY();", cuenta);

        cuenta.Id=id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId){
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"
            select c.id,c.nombre,c.balance,tc.nombre as tipoCuenta, c.tipocuentaid
            from cuentas c
            inner join  tiposCuentas tc
            on tc.id=c.tipoCuentaId
            where tc.usuarioId=1
            order by tc.orden
            ", new {usuarioId});
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId){
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"
            SELECT Cuentas.id, Cuentas.Nombre, Balance, Descripcion, tc.id
            FROM Cuentas
            INNER JOIN TiposCuentas tc
            ON tc.id = Cuentas.TipoCuentaId
            WHERE tc.UsuarioId = @UsuarioId AND Cuentas.id = @Id
            ", new {id, usuarioId});
        }

        public async Task ActualizarCuenta(CuentasCreacionViewModel cuenta){
            using var connection = new SqlConnection(connectionString);
            var id = await connection.ExecuteAsync(@"
            UPDATE Cuentas 
            SET Nombre = @Nombre, TipoCuentaId = @TipoCuentaId, Descripcion = @Descripcion, Balance = @Balance
            WHERE Id = @Id;", cuenta);

        }

        public async Task Borrar(Cuenta cuenta){
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM Cuentas WHERE id = @Id", cuenta);

        }
    }
}
