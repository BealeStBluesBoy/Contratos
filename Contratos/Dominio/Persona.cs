namespace Contratos
{
    public abstract class Persona
    {
        public int Id { get; set; }
        public string CuitCuil { get; set; }
        public string RazonSocial { get; set; }

        public Persona(int id, string cuitCuil, string razonSocial)
        {
            Id = id;
            CuitCuil = cuitCuil;
            RazonSocial = razonSocial;
        }
    }
}
