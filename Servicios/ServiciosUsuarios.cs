namespace ManejoPresupuesto.Servicios
{
	public interface IServiciosUuarios
	{
		int ObtenerUsuarioId();
	}
	public class ServiciosUsuarios : IServiciosUuarios
	{
		public int ObtenerUsuarioId()
		{
			return 1;
		}


	}
}
