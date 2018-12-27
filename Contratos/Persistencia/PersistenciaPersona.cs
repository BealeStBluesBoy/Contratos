using MySql.Data.MySqlClient;

namespace Contratos.Persistencia
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
                var Query = $"INSERT INTO Persona (cuitCuil, razonSocial) VALUES ('{cuitCuil}','{razonSocial}');";
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
