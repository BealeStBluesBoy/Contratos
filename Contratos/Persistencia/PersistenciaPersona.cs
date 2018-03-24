using MySql.Data.MySqlClient;
using System.Windows;

namespace Contratos
{
    public abstract class PersistenciaPersona : Persistencia
    {
        public PersistenciaPersona()
        {
            Table = "Persona";
            SeekId = "cuitCuil";
        }

        public bool Insert(string cuitCuil, string razonSocial)
        {
            if ((GetID(cuitCuil) == -1) && OpenConnection()) /// Si existe la Persona no tiene sentido insertarla
            {
                var Query = string.Format("INSERT INTO Persona (cuitCuil, razonSocial) VALUES ('{0}','{1}');", cuitCuil, razonSocial);
                MySqlCommand Cmd = new MySqlCommand(Query, Connection);
                Cmd.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            else
            {
                CloseConnection();
                return false;
            }
        }
    }
}
