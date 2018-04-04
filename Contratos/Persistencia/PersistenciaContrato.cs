using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Contratos
{
    public class PersistenciaContrato : Persistencia
    {
        /// El Select y SelectAll debe traer ENTIDADES COMPLETAS, Contrato + sus respectivos objetos
        /// 
        /// Una explicacion para el orden de inserciones es que hay tablas que pueden existir por si mismas: Condicion, Persona, Grano
        /// pero otras no pueden ser creadas si sus partes no existen previamente: Contrato, ContratoDetalle, Proveedor
        /// 
        /// Contrato depende de la existencia de sus:
        /// - Grano
        /// - Proveedor
        /// - Condicion (transitivamente por ContratoDetalle)
        /// 
        /// ContratoDetalle depende de la existencia de sus:
        /// - Contrato
        /// - Condicion
        /// 
        /// Proveedor depende de la existencia de sus:
        /// - Persona
        /// 

        public PersistenciaContrato()
        {
            Table = "Contrato";
            SeekId = "numero";
        }

        public bool Insert(int cantidad, DateTime fechaLabra, DateTime fechaLimite, int numero, float precio, string cuitcuil, string tipoGrano, string tipoContrato)
        {
            if (GetID(numero) == -1) /// Hay que combrobar que no exista el Contrato
            {
                PersistenciaGrano dbGrano = new PersistenciaGrano();
                int idGrano = dbGrano.GetID(tipoGrano);

                PersistenciaProveedor dbProveedor = new PersistenciaProveedor();
                int idProveedor = dbProveedor.GetID(cuitcuil);

                if (idGrano != -1 && idProveedor != -1 && OpenConnection()) /// y que existan las partes
                {
                    var Query = string.Format(
                        "INSERT INTO Contrato (Proveedor_id, Grano_id, tipoContrato, cantidad, cerrado, fechaLabra, fechaLimite, numero, precio)" +
                        "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                        idProveedor,
                        idGrano,
                        tipoContrato,
                        cantidad,
                        0,
                        fechaLabra.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        fechaLimite.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        numero,
                        precio);
                    MySqlCommand Cmd = new MySqlCommand(Query, Connection);
                    Cmd.ExecuteNonQuery();
                    CloseConnection();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public Contrato Select(int numero)
        {
            string cuitCuil = "";
            string tipoGrano = "";

            Contrato ret = null;
            if (GetID(numero) != -1 && OpenConnection()) /// Chequea que exista el Contrato en la DB, si existe logicamente existen sus tablas asociadas (si no se modifico la DB externamente)
            {
                var Query = string.Format("SELECT " +
                    "Der.cantidad, " +
                    "Der.cerrado, " +
                    "Der.fechaLabra, " +
                    "Der.fechaLimite, " +
                    "Der.numero, " +
                    "Der.precio, " +
                    "Der.tipoContrato, " +
                    "Per.cuitCuil, " +
                    "Gra.tipo " +
                    "FROM (SELECT * FROM Contrato WHERE numero = {0}) AS Der " +
                    "INNER JOIN Proveedor Pro ON Der.Proveedor_id = Pro.id " +
                    "INNER JOIN Persona Per ON Per.id = Pro.Persona_id " +
                    "INNER JOIN Grano Gra ON Der.Grano_id = Gra.id;", numero);
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = new Contrato(
                        reader.GetInt32("cantidad"),
                        reader.GetBoolean("cerrado"),
                        reader.GetDateTime("fechaLabra"),
                        reader.GetDateTime("fechaLimite"),
                        reader.GetInt32("numero"),
                        reader.GetFloat("precio"),
                        reader.GetString("tipoContrato")
                    );
                    cuitCuil = reader.GetString("cuitCuil");
                    tipoGrano = reader.GetString("tipo");
                    reader.Close();
                }
                CloseConnection();

                PersistenciaGrano dbGrano = new PersistenciaGrano();
                var grano = dbGrano.Select(tipoGrano);

                PersistenciaContratoDetalle dbDetalles = new PersistenciaContratoDetalle();
                var detalles = dbDetalles.Select(numero);

                PersistenciaProveedor dbProveedor = new PersistenciaProveedor();
                var proveedor = dbProveedor.Select(cuitCuil);

                ret.Grano = grano;
                ret.Detalles = detalles;
                ret.Proveedor = proveedor;
            }
            return ret;
        }

        public bool Delete(int numero) /// Elimina Contrato y sus ContratoDetalle
        {
            if (GetID(numero) != -1)
            {
                PersistenciaContratoDetalle dbDetalles = new PersistenciaContratoDetalle();
                dbDetalles.Delete(numero);
                if (OpenConnection())
                {
                    var Query = string.Format("DELETE FROM Contrato WHERE numero='{0}';", numero);
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
            else
            {
                return false;
            }
        }

        public bool Update(int numero, int cantidad, DateTime fechaLimite, float precio, string tipoGrano, string tipoContrato)
        {
            PersistenciaGrano dbGrano = new PersistenciaGrano();
            int idGrano = dbGrano.GetID(tipoGrano);

            int idContrato = GetID(numero);

            if (idContrato != -1 && OpenConnection())
            {
                var Query = string.Format("UPDATE Contrato SET " +
                    "cantidad = '{0}', " +
                    "fechaLimite = '{1}', " +
                    "precio = '{2}', " +
                    "tipoContrato = '{3}', " +
                    "Grano_id = '{4}' " +
                    "WHERE id = {5};", cantidad, fechaLimite.ToString("yyyy-MM-dd HH:mm:ss.fff"), precio, tipoContrato, idGrano, idContrato);
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

        public List<Contrato> SelectAll() /// Vamo a hacerlo sencillo
        {
            List<Contrato> ret = new List<Contrato>();
            List<int> numeros = new List<int>();

            if (OpenConnection())
            {
                var Query = "SELECT numero FROM Contrato ORDER BY numero;";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    numeros.Add(reader.GetInt32("numero"));
                }
                CloseConnection();
            }
            else
            {
                return ret;
            }
            
            numeros.ForEach( x => {
                ret.Add(Select(x));
            });

            return ret;
        }

        public bool Close(int numero)
        {
            int idContrato = GetID(numero);

            if (idContrato != -1 && OpenConnection())
            {
                var Query = string.Format("UPDATE Contrato SET cerrado = 1 WHERE id = {0};", idContrato);
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
    }
}
