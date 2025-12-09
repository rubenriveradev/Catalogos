using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Response<T>
    {
        public bool IsError { get; set; }
        public string MensajeError { get; set; }
        public T Info { get; set; }

        public Response() { }
        public Response(bool _iserror, string _mensajeError, T _info)
        {
            this.IsError = _iserror;
            this.MensajeError = _mensajeError;
            this.Info = _info;
        }
    }
}
