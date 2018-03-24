using System;
using System.Collections.Generic;

namespace Contratos
{
    public class ControladorProveedor
    {
        public bool IngresarProveedor(string cuitCuil, string razonSocial, string ingresosBrutos, DateTime inicioActividades)
        {
            PersistenciaProveedor db = new PersistenciaProveedor();
            if (db.Insert(cuitCuil, razonSocial) && db.Insert(cuitCuil, ingresosBrutos, inicioActividades))
            {
                return true;
            }
            return false;
        }

        public Proveedor VerProveedor(string cuitCuil)
        {
            PersistenciaProveedor db = new PersistenciaProveedor();
            return db.Select(cuitCuil);
        }

        public void EliminarProveedor(string cuitCuil) /// De dudosa utilidad, los Proveedores debieran mantenerse
        {
            PersistenciaProveedor db = new PersistenciaProveedor();
            db.Delete(cuitCuil);
        }

        public List<Proveedor> VerTodos()
        {
            PersistenciaProveedor db = new PersistenciaProveedor();
            return db.SelectAll();
        }

        public bool Existe(string cuitCuil)
        {
            PersistenciaProveedor db = new PersistenciaProveedor();
            return db.GetID(cuitCuil) == -1;
        }
    }
}
