using Contratos.Dominio;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Contratos.Persistencia
{
    public class PersistenciaProveedor : PersistenciaPersona
    {
        public bool Insert(string cuitCuil, string ingresosBrutos, DateTime inicioActividades)
        {
            int idPersona = base.GetID(cuitCuil);
            if (idPersona != -1 && GetID(cuitCuil) == -1 && OpenConnection()) /// Chequea que exista la Persona en la DB
            {
                var Query = "INSERT INTO Proveedor (Persona_id, ingresosBrutos, inicioActividades) " +
                    $"VALUES ('{idPersona}','{ingresosBrutos}','{inicioActividades.ToString("yyyy-MM-dd HH:mm:ss.fff")}');";
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

        public Proveedor Select(string cuitCuil)
        {
            Proveedor ret = null;
            int idProveedor = GetID(cuitCuil);
            if (idProveedor != -1 && OpenConnection())
            {
                var Query = string.Format("SELECT " +
                    "Pro.id," +
                    "Pro.ingresosBrutos," +
                    "Pro.InicioActividades," +
                    "Per.cuitCuil," +
                    "Per.razonSocial " +
                    "FROM (SELECT * FROM Proveedor WHERE id = {0}) AS Pro " +
                    "INNER JOIN Persona Per ON Per.id = Pro.Persona_id;", idProveedor);
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = new Proveedor(
                        reader.GetString("cuitCuil"),
                        reader.GetString("razonSocial"),
                        reader.GetString("ingresosBrutos"),
                        reader.GetDateTime("inicioActividades"));
                }
                CloseConnection();
            }
            return ret;
        }

        public int GetID(string cuitCuil)
        {
            int ret = -1;
            if (OpenConnection())
            {
                var Query = "SELECT " +
                    "Proveedor.id " +
                    "FROM Proveedor " +
                    $"INNER JOIN (SELECT Persona.id FROM Persona WHERE cuitCuil = '{cuitCuil}') AS Derived " +
                    "ON Derived.id = Proveedor.Persona_id;";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                if (cmd.ExecuteScalar() != null)
                {
                    ret = (int)cmd.ExecuteScalar();
                }
                CloseConnection();
            }
            return ret;
        }

        public bool Delete(string cuitCuil) /// De dudosa utilidad, los Proveedores debieran mantenerse
        {
            int idPersona = base.GetID(cuitCuil);
            int idProveedor = GetID(cuitCuil);
            if (idProveedor != -1 && OpenConnection())
            {
                var Query = $"DELETE FROM Proveedor WHERE Persona_id='{idPersona}';";
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

        public List<Proveedor> SelectAll() /// Vamo a hacerlo sencillo
        {
            List<Proveedor> ret = null;
            List<string> cuilsProvs = new List<string>();

            if (OpenConnection())
            {
                var Query = "SELECT DISTINCT Per.cuitCuil FROM Proveedor Pro " +
                    "INNER JOIN Persona Per ON Pro.Persona_id = Per.id;";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                if (cmd.LastInsertedId != -1)
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cuilsProvs.Add(reader.GetString("cuitCuil"));
                    }
                }
                CloseConnection();

                ret = new List<Proveedor>();
                cuilsProvs.ForEach(x => {
                    ret.Add(Select(x));
                });
            }

            return ret;
        }
    }
}
