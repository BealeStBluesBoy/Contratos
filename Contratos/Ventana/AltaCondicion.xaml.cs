using System.Windows;

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
            if (nombreCondicion.Text != "" && unidadCondicion.Text != "")
            {
                if (ControladorCondicion.IngresarCondicion(nombreCondicion.Text, unidadCondicion.Text))
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
            if (nombreCondicion.Text != "" && unidadCondicion.Text != "")
            {
                if (ControladorCondicion.IngresarCondicion(nombreCondicion.Text, unidadCondicion.Text))
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
