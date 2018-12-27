using System;
using System.Collections.Generic;

namespace Contratos.Dominio
{
    public class Proveedor : Persona
    {
        public string IngresosBrutos { get; set; }
        public DateTime InicioActividades { get; set; }
        public List<Contrato> Contratos { get; set; }

        public Proveedor(string cuitCuil, string razonSocial, string ingresosBrutos, DateTime iniActividades) : base(cuitCuil, razonSocial)
        {
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
