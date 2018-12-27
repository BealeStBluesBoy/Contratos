using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Contratos.Controlador;

namespace Contratos.GUI
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
            if (razonSocial.Text != "" && cuitCuil.Text != "" && iiBB.Text != "" && inicioActividades.Text != "")
            {
                if (ControladorProveedor.IngresarProveedor(
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
