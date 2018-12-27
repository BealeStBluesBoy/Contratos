using Contratos.Dominio;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Contratos.Persistencia
{
    public class PersistenciaContratoDetalle : Persistencia
    {
        /// Nota importante: Al ser una tabla que tiene dos FKs, ambas tablas a las que apuntan deben estar creadas antes de insertar
        /// La eliminacion puede ser directa, pero solo es necesaria al eliminar al Contrato que pertenecen
        /// Por motivos de diseño de DB esta es la ULTIMA tabla que se debe agregar, ya que las tablas Condicion y Contrato deben estar ya creadas

        public bool Insert(int numeroContrato, string nombreCondicion, float valor)
        {
            PersistenciaCondicion dbCondicion = new PersistenciaCondicion();
            int idCondicion = dbCondicion.GetID(nombreCondicion);

            PersistenciaContrato dbContrato = new PersistenciaContrato();
            int idContrato = dbContrato.GetID(numeroContrato);

            if (idCondicion != -1 && idContrato != -1 && OpenConnection()) /// Hay que verificar que no existe y además de que los id sean diferentes
            {
                var Query = "INSERT INTO ContratoDetalle (contrato_id, condicion_id, valor) " +
                    $"VALUES ('{idContrato}','{idCondicion}','{valor}');";
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

        public bool Delete(int numero) /// Elimina ContratoDetalles
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            int idContrato = dbContrato.GetID(numero);

            if (idContrato != -1 && OpenConnection())
            {
                var Query = $"DELETE FROM ContratoDetalle WHERE Contrato_id = '{idContrato}';";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ContratoDetalle> Select(int numeroContrato)
        {
            PersistenciaContrato dbContrato = new PersistenciaContrato();
            int idContrato = dbContrato.GetID(numeroContrato);

            List<ContratoDetalle> ret = new List<ContratoDetalle>();

            if (OpenConnection())
            {
                var Query = "SELECT valor, nombre, unidad " +
                    "FROM ContratoDetalle Det " +
                    "INNER JOIN Condicion Con ON Det.Condicion_id = Con.id " +
                    $"WHERE Contrato_id = '{idContrato}';";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var detalle = new ContratoDetalle(
                        reader.GetFloat("valor")
                        );
                    var condicion = new Condicion(
                        reader.GetString("nombre"),
                        reader.GetString("unidad")
                        );
                    detalle.AgregarCondicion(condicion);
                    ret.Add(detalle);
                };
                CloseConnection();
            }
            return ret;
        }
    }
}
