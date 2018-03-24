using System;
using System.Collections.Generic;

namespace Contratos
{
    public class Contrato
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public bool Cerrado { get; set; }
        public DateTime FechaLabra { get; set; }
        public DateTime FechaLimite { get; set; }
        public int Numero { get; set; }
        public float Precio { get; set; }
        public string TipoContrato { get; set; }
        public List<ContratoDetalle> Detalles { get; set; }
        public Grano Grano { get; set; }
        public Proveedor Proveedor { get; set; }

        public Contrato(int cant, bool cerrado, DateTime fechaLabra, DateTime fechaLimite, int num, float precio, string tipoContrato)
        {
            Cantidad = cant;
            Cerrado = cerrado;
            FechaLabra = fechaLabra;
            FechaLimite = fechaLimite;
            Numero = num;
            Precio = precio;
            TipoContrato = tipoContrato;
            Detalles = null;
            Grano = null;
            Proveedor = null;
        }

        public void CerrarContrato()
        {
            Cerrado = true;
        }

        public void AgregarDetalle(ContratoDetalle det)
        {
            if (Detalles.Contains(det) == false)
            {
                Detalles.Add(det);
                det.AgregarContrato(this);
            }
        }

        public void AgregarGrano(Grano g)
        {
            Grano = g;
        }

        public void AgregarProveedor(Proveedor p)
        {
            Proveedor = p;
        }
    }
}
