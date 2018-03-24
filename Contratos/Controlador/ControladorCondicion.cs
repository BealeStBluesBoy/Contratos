using System.Collections.Generic;
using System.Windows;

namespace Contratos
{
    public class ControladorCondicion
    {
        public bool IngresarCondicion(string nombre, string unidad)
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return (db.Insert(nombre, unidad));
        }

        public Condicion VerCondicion(string nombre)
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return db.Select(nombre);
        }

        public List<Condicion> VerTodos()
        {
            PersistenciaCondicion db = new PersistenciaCondicion();
            return db.SelectAll();
        }
    }
}
