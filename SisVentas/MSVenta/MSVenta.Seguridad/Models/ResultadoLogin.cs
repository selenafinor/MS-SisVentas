using MSVenta.Seguridad.Models;

namespace MSVenta.Seguridad.DTOs
{
    public enum MotivoLogin
    {
        Exitoso,
        UsuarioNoExiste,
        PasswordIncorrecta,
        CuentaInactiva,
        CuentaBloqueada
    }

    public class ResultadoLogin
    {
        public MotivoLogin Motivo { get; set; }
        public Usuario Usuario { get; set; }
        public System.DateTime? BloqueadoHasta { get; set; }
    }
}