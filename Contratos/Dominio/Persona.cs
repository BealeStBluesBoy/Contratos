namespace Contratos.Dominio
{
    public abstract class Persona
    {
        public string CuitCuil { get; set; }
        public string RazonSocial { get; set; }

        public Persona(string cuitCuil, string razonSocial)
        {
            CuitCuil = cuitCuil;
            RazonSocial = razonSocial;
        }
    }
}
