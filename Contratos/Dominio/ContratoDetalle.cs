namespace Contratos.Dominio
{
    public class ContratoDetalle
    {
        public float Valor { get; set; }
        public Condicion Condicion { get; set; }
        public Contrato Contrato { get; set; }

        public ContratoDetalle(float val)
        {
            Valor = val;
            Condicion = null;
            Contrato = null;
        }

        public void AgregarCondicion(Condicion cond) => Condicion = cond;

        public void AgregarContrato(Contrato c) => Contrato = c;
    }
}
