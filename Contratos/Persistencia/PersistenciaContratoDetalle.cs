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
                var Query = string.Format("INSERT INTO ContratoDetalle (contrato_id, condicion_id, valor) VALUES ('{0}','{1}','{2}');", idContrato, idCondicion, valor);
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
                var Query = string.Format("DELETE FROM ContratoDetalle WHERE Contrato_id='{0}';", idContrato);
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
                var Query = string.Format("SELECT " +
                    "Det.id AS idDet, " +
                    "Con.id AS idCon, " +
                    "Det.valor, " +
                    "Con.nombre, " +
                    "Con.unidad " +
                    "FROM ContratoDetalle Det, Condicion Con " +
                    "WHERE Contrato_id='{0}' AND Det.Condicion_id = Con.id;", idContrato);
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
