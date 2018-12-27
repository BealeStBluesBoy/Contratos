namespace Contratos.Dominio
{
    public class Condicion
    {
        public string Nombre { get; set; }
        public string Unidad { get; set; }

        public Condicion(string nom, string unid)
        {
            Nombre = nom;
            Unidad = unid;
        }
    }
}