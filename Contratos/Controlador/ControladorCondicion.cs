using Contratos.Dominio;
using Contratos.Persistencia;
using System.Collections.Generic;

namespace Contratos.Controlador
{
    public static class ControladorCondicion
    {
        public static bool IngresarCondicion(string nombre, string unidad)
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return (db.Insert(nombre, unidad));
        }

        public static Condicion VerCondicion(string nombre)
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return db.Select(nombre);
        }

        public static List<Condicion> VerTodos()
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return db.SelectAll();
        }
    }
}
