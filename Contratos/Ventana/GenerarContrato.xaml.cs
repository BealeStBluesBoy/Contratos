﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Contratos
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
                ControladorCondicion ctrl = new ControladorCondicion();
                List<Condicion> condAct = ctrl.VerTodos();
                IList<Condicion> indCond = condAct;

                List<ContratoDetalle> ret = new List<ContratoDetalle>();

                int tope = rnd.Next(1, 5);
                for (int i = 0; i <= tope; i++)
                {
                    var det = new ContratoDetalle(0, rnd.Next(1, 10))
                    {
                        Condicion = indCond[i]
                    };
                    ret.Add(det);
                }
                return ret;
            }

            if (Cantidad.Text != "" && Int32.TryParse(Cantidad.Text, out int result) && Int32.Parse(Cantidad.Text) > 0 && Int32.Parse(Cantidad.Text) <= 20)
            {
                int cantGen = Int32.Parse(Cantidad.Text);

                ControladorProveedor ctrlProv = new ControladorProveedor();
                List<Proveedor> provAct = ctrlProv.VerTodos();
                IList<Proveedor> indProv = provAct;

                ControladorGrano ctrlGrano = new ControladorGrano();
                List<Grano> granoAct = ctrlGrano.VerTodos();
                IList<Grano> indGrano = granoAct;

                ControladorContrato ctrlCont = new ControladorContrato();
                List<Contrato> contAct = ctrlCont.VerTodos();
                List<int> numOcupados = new List<int>();
                contAct.ForEach(x => { numOcupados.Add(x.Numero); });
                IList<int> indCont = numOcupados;
                int cantActual = contAct.Count;

                for (int i = 1; i <= cantGen; i++)
                {
                    int contNum;
                    do
                    {
                        contNum = rnd.Next(1, 99999999);
                    } while (numOcupados.Contains(contNum));

                    if (cantActual != 0)
                    {
                        var indiceCont = rnd.Next(1, cantActual);
                    }
                    var indiceProv = rnd.Next(1, provAct.Count);
                    var indiceGrano = rnd.Next(1, granoAct.Count);

                    if (ctrlCont.IngresarContrato(
                        indProv[indiceProv].CuitCuil,
                        RandomDetalles(),
                        indGrano[indiceGrano].Tipo,
                        rnd.Next(1, 25),
                        DateTime.Today,
                        DateTime.Today.AddDays(rnd.Next(10, 60)),
                        contNum,
                        rnd.Next(100, 200),
                        RandomTipo()
                        ))
                    {
                        cantActual++;
                        indCont.Add(contNum);
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