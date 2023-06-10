using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioTiposCuentas
	{
		Task Actualizar(TipoCuenta tipoCuenta);
		Task Borrar(TipoCuenta tipoCuenta);
		Task Crear(TipoCuenta tipoCuenta);
		Task<bool> Existe(string nombre, int usuarioId);
		Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
		Task<TipoCuenta> ObtenerPorId(int Id, int UsuarioId);
	}
	public class RepositorioTiposCuentas : IRepositorioTiposCuentas
	{
		private readonly string connectionString;
		public RepositorioTiposCuentas(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");

		}

		public async Task Crear(TipoCuenta tipoCuenta) 
		{
			using var connection = new SqlConnection(connectionString);
			var id =  await connection.QuerySingleAsync<int>($@"Insert Into TiposCuentas(Nombre,UsuarioId,Orden)
													Values (@Nombre, @UsuarioId,0)
													Select SCOPE_IDENTITY();",tipoCuenta);
			tipoCuenta.Id = id;
		}

		public async Task<bool> Existe(string nombre, int usuarioId)
		{
			using var connetion = new SqlConnection(connectionString);
			var existe = await connetion.QueryFirstOrDefaultAsync<int>(
																		@"Select 1 
																		From TiposCuentas
																		Where Nombre = @Nombre AND UsuarioId = @UsuarioId;",
																		new { nombre, usuarioId });

			return existe == 1;
		
		}

		public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
		{
			using var connetion = new SqlConnection(connectionString);
			return await connetion.QueryAsync<TipoCuenta>(@"SELECT id,[Nombre],[UsuarioId],[Orden]
														  FROM [ManejoPresupuesto].[dbo].[TiposCuentas]
														  Where UsuarioId = @usuarioID ", new { usuarioId});

		}

		public async Task Actualizar(TipoCuenta tipoCuenta)
		{ 
			using var connection = new SqlConnection(connectionString);
			await connection.ExecuteAsync(@"UPDATE TiposCuentas
											SET NOMBRE = @Nombre
											WHERE Id = @Id", tipoCuenta);
		}

		public async Task<TipoCuenta> ObtenerPorId(int Id, int UsuarioId)
		{
			using var connection = new SqlConnection(connectionString);
			return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT [Nombre],[Orden]
																		  FROM [ManejoPresupuesto].[dbo].[TiposCuentas]
																		  Where Id = @Id and UsuarioId = @usuarioId", 
																		  new {Id,UsuarioId });
		}

		public async Task Borrar(TipoCuenta tipoCuenta)
		{ 
			using var connection = new  SqlConnection(connectionString);
			await connection.ExecuteAsync(@"Delete From [ManejoPresupuesto].[dbo].[TiposCuentas] Where Id=@id",
				tipoCuenta);
		}
	}
}
