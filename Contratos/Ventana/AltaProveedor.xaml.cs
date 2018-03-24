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
using System.Text.RegularExpressions;

namespace Contratos
{
    /// <summary>
    /// Lógica de interacción para AltaProveedor.xaml
    /// </summary>
    public partial class AltaProveedor : Window
    {
        public AltaProveedor()
        {
            InitializeComponent();
        }

        private void GuardarProveedor_Click(object sender, RoutedEventArgs e)
        {
            ControladorProveedor ctrl = new ControladorProveedor();
            if (razonSocial.Text != "" && cuitCuil.Text != "" && iiBB.Text != "" && inicioActividades.Text != "")
            {
                if (ctrl.IngresarProveedor(
                    cuitCuil.Text,
                    razonSocial.Text,
                    iiBB.Text,
                    inicioActividades.SelectedDate.Value.Date
                    ))
                {
                    MessageBox.Show("Operación exitosa", "Éxito", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Close();
                }
                else
                    MessageBox.Show("Error en la operación. Compruebe los datos o contacte con el administrador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Error en el ingreso de datos. Asegurese de ingresar datos en todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CancelarProveedor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
