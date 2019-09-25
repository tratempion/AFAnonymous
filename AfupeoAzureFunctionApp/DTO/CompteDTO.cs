using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class CompteDTO
    {
        public Guid id { get; set; }
        public Guid banque { get; set; }

        public string numero { get; set; }
        public CompteDTO( string numero)
        {
            this.numero = numero;
        }
        public override string ToString()
        {
            return numero;
        }
    }
}
