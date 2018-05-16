using System.Collections.Generic;

namespace Contratos
{
    public static class ControladorGrano
    {
        public static bool IngresarGrano(string tipo)
        {
            PersistenciaGrano db = new PersistenciaGrano();
            return (db.Insert(tipo));
        }

        public static List<Grano> VerTodos() /// Util para un desplegable solamente
        {
            PersistenciaGrano db = new PersistenciaGrano();
            return db.SelectAll();
        }
    }
}
