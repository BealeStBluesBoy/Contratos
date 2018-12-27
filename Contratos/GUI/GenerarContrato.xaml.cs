using Contratos.Controlador;
using Contratos.Dominio;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Contratos.GUI
{
    /// <summary>
    /// Lógica de interacción para GenerarContrato.xaml
    /// </summary>
    public partial class GenerarContrato : Window
    {
        public bool Refrescar = false;

        public GenerarContrato()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            string RandomTipo()
            {
                string[] arr = new string[] { "Camiones", "Toneladas" };
                return arr[rnd.Next(0, 2)];
            }

            List<ContratoDetalle> RandomDetalles()
            {
                IList<Condicion> condAct = ControladorCondicion.VerTodos();

                List<ContratoDetalle> ret = new List<ContratoDetalle>();

                int tope = rnd.Next(1, condAct.Count);
                for (int i = 0; i <= tope; i++)
                {
                    var det = new ContratoDetalle(rnd.Next(1, 10))
                    {
                        Condicion = condAct[i]
                    };
                    ret.Add(det);
                }
                return ret;
            }

            if (Cantidad.Text != "" && Int32.TryParse(Cantidad.Text, out int result) && Int32.Parse(Cantidad.Text) > 0)
            {
                int cantGen = Int32.Parse(Cantidad.Text);

                IList<Proveedor> provAct = ControladorProveedor.VerTodos();
                IList<Grano> granoAct = ControladorGrano.VerTodos();

                List<Contrato> contAct = ControladorContrato.VerTodos();
                IList<int> numOcupados = new List<int>();
                contAct.ForEach(x => { numOcupados.Add(x.Numero); });
                int cantActual = contAct.Count;

                for (int i = 1; i <= cantGen; i++)
                {
                    int contNum;
                    do
                    {
                        contNum = rnd.Next(1, 9999);
                    } while (numOcupados.Contains(contNum));

                    var indiceProv = rnd.Next(0, provAct.Count);
                    var indiceGrano = rnd.Next(0, granoAct.Count);

                    if (ControladorContrato.IngresarContrato(
                        provAct[indiceProv].CuitCuil,
                        RandomDetalles(),
                        granoAct[indiceGrano].Tipo,
                        rnd.Next(1, 25),
                        DateTime.Today,
                        DateTime.Today.AddDays(rnd.Next(10, 60)),
                        contNum,
                        rnd.Next(100, 200),
                        RandomTipo()
                        ))
                    {
                        cantActual++;
                        numOcupados.Add(contNum);
                    }
                    else
                    {
                        MessageBox.Show("No inserto");
                    }
                }
                Refrescar = true;
                Close();
            }
            else
            {
                MessageBox.Show("Introduzca una cantidad coherente");
            }
        }
    }
}
