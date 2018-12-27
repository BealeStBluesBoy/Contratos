using Contratos.Dominio;
using Contratos.Persistencia;
using System.Collections.Generic;

namespace Contratos.Controlador
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
