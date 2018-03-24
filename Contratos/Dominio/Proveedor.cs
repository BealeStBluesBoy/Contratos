using System;
using System.Collections.Generic;

namespace Contratos
{
    public class Proveedor : Persona
    {
        public string IngresosBrutos { get; set; }
        public DateTime InicioActividades { get; set; }
        public List<Contrato> Contratos { get; set; }

        public Proveedor(int id, string cuitCuil, string razonSocial, string ingresosBrutos, DateTime iniActividades) : base(id, cuitCuil, razonSocial)
        {
            Id = id;
            IngresosBrutos = ingresosBrutos;
            InicioActividades = iniActividades;
            CuitCuil = cuitCuil;
            RazonSocial = razonSocial;
            Contratos = null;
        }

        public void AgregarContrato(Contrato c)
        {
            if (Contratos.Contains(c) == false)
            {
                Contratos.Add(c);
                c.AgregarProveedor(this);
            }
        }
    }
}
