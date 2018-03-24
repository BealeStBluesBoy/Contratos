namespace Contratos
{
    public class ContratoDetalle
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public Condicion Condicion { get; set; }
        public Contrato Contrato { get; set; }

        public ContratoDetalle(int id, float val)
        {
            Id = id;
            Valor = val;
            Condicion = null;
            Contrato = null;
        }

        public void AgregarCondicion(Condicion cond) => Condicion = cond;

        public void AgregarContrato(Contrato c) => Contrato = c;
    }
}
