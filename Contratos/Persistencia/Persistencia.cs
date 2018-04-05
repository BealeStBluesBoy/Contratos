using MySql.Data.MySqlClient;
using System.Windows;

namespace Contratos
{
    public abstract class Persistencia
    {
        protected MySqlConnection Connection;
        protected string Table;
        protected string SeekId;

        public Persistencia()
        {
            string ConnectionString;
            ConnectionString = "server = javapower.duckdns.org; uid = external; pwd = 123abc; database = molino; sslmode = none";
            Connection = new MySqlConnection(ConnectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public int GetID(object uniqueValue)
        {
            int ret = -1;
            if (OpenConnection())
            {
                var Query = string.Format("SELECT id FROM {0} WHERE {1} = '{2}';", Table, SeekId, uniqueValue);
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                if (cmd.ExecuteScalar() != null)
                {
                    ret = (int)cmd.ExecuteScalar();
                }
                CloseConnection();
            }
            return ret;
        }
    }
}
