using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CambioPassword
    {
        public int IdUsuario { get; set; }
        public string Password { get; set; }
        public string ConfirmPass { get; set; }
    }
    public class CambioPasswordDTO : CambioPassword
    {
        public int Id_Usuario_Modifica { get; set; }
    }
}
