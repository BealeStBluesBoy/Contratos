using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contratos
{
    public class ControladorContrato
    {
        /// Ver notas en la Persistencia
        
        public bool IngresarContrato(string cuitCuilProveedor, List<ContratoDetalle> detalles, string tipoGrano, int cantidad, DateTime fechaLabra, DateTime fechaLimite, int numero, float precio, string tipo)
        {
            bool ret = false;
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            if (dbContrato.Insert(cantidad, fechaLabra, fechaLimite, numero, precio, cuitCuilProveedor, tipoGrano, tipo))
            {
                PersistenciaContratoDetalle dbDetalles = new PersistenciaContratoDetalle();
                detalles.ForEach(x => {
                    ret = dbDetalles.Insert(numero, x.Condicion.Nombre, x.Valor); /// Despues inserto los ContratoDetalle para finalizar los linkeos
                });
            }
            return ret;
        }

        public Contrato VerContrato(int numero)
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return dbContrato.Select(numero);
        }

        public bool CerrarContrato(int numero)
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return dbContrato.Close(numero);
        }

        public async Task<List<Contrato>> VerTodos()
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return await dbContrato.SelectAll();
        }

        public bool EliminarContrato(int numero) /// Elimina solamente Contrato y sus ContratoDetalle
        {
            PersistenciaContrato db = new PersistenciaContrato();
            return db.Delete(numero);
        }

        public bool ActualizarContrato(int numero, int cantidad, DateTime fechaLimite, float precio, string tipoGrano, string tipoContrato, List<ContratoDetalle> detalles)
        {
            PersistenciaContratoDetalle dbDetalles = new PersistenciaContratoDetalle();
            dbDetalles.Delete(numero);
            detalles.ForEach(x => {
                dbDetalles.Insert(numero, x.Condicion.Nombre, x.Valor);
            });
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return dbContrato.Update(numero, cantidad, fechaLimite, precio, tipoGrano, tipoContrato);
        }
    }
}