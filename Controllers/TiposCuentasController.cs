﻿using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
	public class TiposCuentasController : Controller
	{
		private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
		private readonly IServiciosUuarios serviciosUuarios;

		public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
			IServiciosUuarios serviciosUuarios)
		{
			this.repositorioTiposCuentas = repositorioTiposCuentas;
			this.serviciosUuarios = serviciosUuarios;
		}

		public async Task<IActionResult> Index()
		{ 
			var usuarioId= serviciosUuarios.ObtenerUsuarioId();
			var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
			return View(tiposCuentas);


		}
		public IActionResult Crear()
		{
				return View();
		}

		[HttpPost]
		public async Task<IActionResult> Crear(TipoCuenta tipoCuenta) 
		{
			if (!ModelState.IsValid)
			{
				return View(tipoCuenta);
			}

			tipoCuenta.UsuarioId = serviciosUuarios.ObtenerUsuarioId();

			var yaExisteTiposCuentas = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

			if (yaExisteTiposCuentas)
			{
				ModelState.AddModelError(nameof(tipoCuenta.Nombre),
					$"El Nombre {tipoCuenta.Nombre} ya existe.");
				return View(tipoCuenta);
			}

			await repositorioTiposCuentas.Crear(tipoCuenta);
			
			return RedirectToAction("Index");

			
		}

		[HttpGet]
		public async Task<IActionResult> Editar(int Id)
		{
			var usuarioId = serviciosUuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(Id, usuarioId);

			if (tipoCuenta is null )
			{
				return RedirectToAction("NoEncontrado", "Home");
			}
			return View(tipoCuenta);
		}

		[HttpPost]

		public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
		{
			var usuarioId = serviciosUuarios.ObtenerUsuarioId();
			var tipocuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

			if (tipocuentaExiste is null )
			{
				return View("NoEncontrado", "Home");
			}

			await repositorioTiposCuentas.Actualizar(tipoCuenta);
			return RedirectToAction("Index");

		}

		[HttpGet]
		public async Task<IActionResult> Borrar(int Id)
		{
			var usuarioId = serviciosUuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(Id, usuarioId);

			if (tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}
			return View(tipoCuenta);
		}

		[HttpPost]
		public async Task<IActionResult> Borrar(TipoCuenta tipoCuenta)
		{
			var usuarioId = serviciosUuarios.ObtenerUsuarioId();
			var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
			if (tipoCuentaExiste is null)
			{ 
				return View("NoEncontrado","Index");
			}
			await repositorioTiposCuentas.Borrar(tipoCuenta);
			return RedirectToAction("Index");
		}


		[HttpGet]
		public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
		{
			var usuarioId = serviciosUuarios.ObtenerUsuarioId();
			var yaExisteTiposCuentas = await repositorioTiposCuentas.Existe(nombre,usuarioId);

			if (yaExisteTiposCuentas)
			{
				return Json($"El Nombre {nombre} ya existe");
			}

			return Json(true);
		}
	}
}
