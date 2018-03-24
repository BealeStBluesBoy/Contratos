namespace Contratos
{
    public class Condicion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Unidad { get; set; }

        public Condicion(int id, string nom, string unid)
        {
            Id = id;
            Nombre = nom;
            Unidad = unid;
        }
    }
}