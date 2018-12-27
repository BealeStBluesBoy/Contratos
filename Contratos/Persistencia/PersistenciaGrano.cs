using Contratos.Dominio;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Contratos.Persistencia
{
    public class PersistenciaGrano : Persistencia
    {
        public PersistenciaGrano()
        {
            Table = "Grano";
            SeekId = "tipo";
        }

        public bool Insert(string tipoGrano)
        {
            if (GetID(tipoGrano) == -1 && OpenConnection()) /// Si existe el Grano no tiene sentido insertarlo
            {
                var Query = string.Format("INSERT INTO Grano (tipo) VALUES ('{0}');", tipoGrano);
                MySqlCommand Cmd = new MySqlCommand(Query, Connection);
                Cmd.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Grano Select(string tipo) /// Se hace este select solo por las Foreign Key asociadas a grano
        {
            Grano ret = null;
            if (OpenConnection())
            {
                var Query = string.Format("SELECT * FROM Grano WHERE tipo = '{0}';", tipo);
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = new Grano(tipo);
                }
            }
            CloseConnection();
            return ret;
        }

        public List<Grano> SelectAll() /// Un Select normal no tendria sentido ya que con solo saber el tipo se puede crear el Grano, ademas al traer Contrato se trae tambien su Grano
        {
            List<Grano> ret = new List<Grano>();
            if (OpenConnection())
            {
                var Query = "SELECT * FROM Grano;";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var grano = new Grano(
                        reader.GetString("tipo")
                    );
                    ret.Add(grano);
                }
            }
            CloseConnection();
            return ret;
        }

        /// Un Delete no es nesesario, los tipos sirven para futuro
    }
}
