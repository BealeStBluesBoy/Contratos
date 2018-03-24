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
    /// Lógica de interacción para AltaContratoDetalle.xaml
    /// </summary>
    public partial class AltaContratoDetalle : Window
    {
        public string Nombre { get; set; }
        public string Unidad { get; set; }
        public float Valor { get; set; }

        public AltaContratoDetalle()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            ControladorCondicion ctrl = new ControladorCondicion();
            var condiciones = ctrl.VerTodos();
            var lista = new List<string>();
            if (condiciones != null)
            {
                condiciones.ForEach( x => {
                    lista.Add(x.Nombre);
                    });
                nombreCondicion.ItemsSource = lista;
            }
        }

        private void NuevaCondicion_Click(object sender, RoutedEventArgs e)
        {
            AltaCondicion altaCondicionVentana = new AltaCondicion()
            { Owner = this };
            altaCondicionVentana.ShowDialog();
            PopulateComboBox();
        }

        private void AgregarDetalle_Click(object sender, RoutedEventArgs e)
        {
            if (valorDetalle.Text != "" && nombreCondicion.Text != "")
            {
                Nombre = nombreCondicion.Text;
                ControladorCondicion ctrl = new ControladorCondicion();
                Unidad = ctrl.VerCondicion(nombreCondicion.Text).Unidad;
                Valor = float.Parse(valorDetalle.Text);
                Close();
            }
            else
            {
                MessageBox.Show("Complete los campos","Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelarDetalle_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
