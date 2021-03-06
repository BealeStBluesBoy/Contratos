﻿using Contratos.Dominio;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Contratos.Persistencia
{
    public class PersistenciaCondicion : Persistencia
    {
        public PersistenciaCondicion()
        {
            Table = "Condicion";
            SeekId = "nombre";
        }

        public bool Insert(string nombre, string unidad)
        {
            if (GetID(nombre) == -1 && OpenConnection()) /// Si existe la Condicion no tiene sentido insertarla
            {
                var Query = $"INSERT INTO {Table} (nombre, unidad) VALUES ('{nombre}','{unidad}');";
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

        public Condicion Select(string nombre)
        {
            Condicion ret = null;
            if (GetID(nombre) != -1 && OpenConnection()) /// Chequea que exista la Condicion en la DB
            {
                var Query = $"SELECT * FROM {Table} WHERE {SeekId} = '{nombre}';";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ret = new Condicion(
                        reader.GetString("nombre"),
                        reader.GetString("unidad")
                    );
                    reader.Close();
                }
            }
            CloseConnection();
            return ret;
        }

        public List<Condicion> SelectAll()
        {
            List<Condicion> ret = new List<Condicion>();
            if (OpenConnection())
            {
                var Query = $"SELECT * FROM {Table};";
                MySqlCommand cmd = new MySqlCommand(Query, Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var condicion = new Condicion(
                        reader.GetString("nombre"),
                        reader.GetString("unidad")
                    );
                    ret.Add(condicion);
                }
                reader.Close();
            }
            CloseConnection();
            return ret;
        }

        /// Un Delete no es nesesario, las Condicion sirven para futuro
    }
}