using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
	public class CuentasController :Controller
	{
		private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
		private readonly IServiciosUuarios serviciosUuarios;

		public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
			                     IServiciosUuarios serviciosUuarios)
		{
			this.repositorioTiposCuentas = repositorioTiposCuentas;
			this.serviciosUuarios = serviciosUuarios;
		}

		[HttpGet]
		public  async Task<IActionResult> Crear()
		{
			var usurioId = serviciosUuarios.ObtenerUsuarioId();
			var tiposCuentas = await repositorioTiposCuentas.Obtener(usurioId);
			var modelo = new CuentaCreacionViewModel();

			modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
			return View(modelo);
		}
	}
}
