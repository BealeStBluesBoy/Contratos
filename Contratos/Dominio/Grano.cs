using System.Collections.Generic;

namespace Contratos
{
    public class Grano
    {
        public string Tipo { get; set; }
        public List<Contrato> Contratos { get; set; }

        public Grano(string tipo)
        {
            Tipo = tipo;
            Contratos = new List<Contrato>();
        }

        public void AgregarContrato(Contrato con)
        {
            if (Contratos.Contains(con) == false)
            {
                Contratos.Add(con);
                con.AgregarGrano(this);
            }
        }
    }
}
