using System;
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
    /// Lógica de interacción para AltaCondicion.xaml
    /// </summary>
    public partial class AltaCondicion : Window
    {
        public AltaCondicion()
        {
            InitializeComponent();
        }

        private void GuardarCondicion_Click(object sender, RoutedEventArgs e)
        {
            ControladorCondicion ctrl = new ControladorCondicion();
            if ((nombreCondicion.Text != "") && (unidadCondicion.Text != ""))
            {
                if (ctrl.IngresarCondicion(nombreCondicion.Text, unidadCondicion.Text))
                    MessageBox.Show("Operación exitosa", "Éxito");
                else
                    MessageBox.Show("Error en la operación. Compruebe los datos o contacte con el administrador", "Error");
            }
            else
                MessageBox.Show("No deje campos vacios", "Error");
        }

        private void CancelarCondicion_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GuardarSalirCondicion_Click(object sender, RoutedEventArgs e)
        {
            ControladorCondicion ctrl = new ControladorCondicion();
            if ((nombreCondicion.Text != "") && (unidadCondicion.Text != ""))
            {
                if (ctrl.IngresarCondicion(nombreCondicion.Text, unidadCondicion.Text))
                {
                    MessageBox.Show("Operación exitosa", "Éxito");
                    Close();
                }
                else
                    MessageBox.Show("Error en la operación. Compruebe los datos o contacte con el administrador", "Error");
            }
            else
                MessageBox.Show("No deje campos vacios", "Error");
        }
    }
}
