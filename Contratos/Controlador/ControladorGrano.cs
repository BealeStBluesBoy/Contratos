using System.Collections.Generic;

namespace Contratos
{
    public class ControladorGrano
    {
        public bool IngresarGrano(string tipo)
        {
            PersistenciaGrano db = new PersistenciaGrano();
            return (db.Insert(tipo));
        }

        public List<Grano> VerTodos() /// Util para un desplegable solamente
        {
            PersistenciaGrano db = new PersistenciaGrano();
            return db.SelectAll();
        }
    }
}
