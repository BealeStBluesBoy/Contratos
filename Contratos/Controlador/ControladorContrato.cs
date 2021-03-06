﻿using Contratos.Dominio;
using Contratos.Persistencia;
using System;
using System.Collections.Generic;

namespace Contratos.Controlador
{
    public static class ControladorContrato
    {
        /// Ver notas en la Persistencia

        public static event EventHandler<Contrato> ContratoCreado;
        public static event EventHandler<Contrato> ContratoEliminado;
        public static event EventHandler<Contrato> ContratoActualizado;

        public static bool IngresarContrato(string cuitCuilProveedor, List<ContratoDetalle> detalles, string tipoGrano, int cantidad, DateTime fechaLabra, DateTime fechaLimite, int numero, float precio, string tipo)
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
            ContratoCreado?.Invoke(null, VerContrato(numero));
            return ret;
        }

        public static Contrato VerContrato(int numero)
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return dbContrato.Select(numero);
        }

        public static bool CerrarContrato(int numero)
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            bool ret = dbContrato.Close(numero);
            if (ret)
                ContratoActualizado?.Invoke(null, VerContrato(numero));
            return ret;
        }

        public static List<Contrato> VerTodos()
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            return dbContrato.SelectAll();
        }

        public static bool EliminarContrato(int numero) /// Elimina solamente Contrato y sus ContratoDetalle
        {
            PersistenciaContrato db = new PersistenciaContrato();
            ContratoEliminado?.Invoke(null, VerContrato(numero));
            return db.Delete(numero);
        }

        public static bool ActualizarContrato(int numero, int cantidad, DateTime fechaLimite, float precio, string tipoGrano, string tipoContrato, List<ContratoDetalle> detalles)
        {
            PersistenciaContratoDetalle dbDetalles = new PersistenciaContratoDetalle();
            dbDetalles.Delete(numero);
            detalles.ForEach(x => {
                dbDetalles.Insert(numero, x.Condicion.Nombre, x.Valor);
            });
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            bool ret = dbContrato.Update(numero, cantidad, fechaLimite, precio, tipoGrano, tipoContrato);
            if (ret)
                ContratoActualizado?.Invoke(null, VerContrato(numero));
            return ret;
        }
    }
}