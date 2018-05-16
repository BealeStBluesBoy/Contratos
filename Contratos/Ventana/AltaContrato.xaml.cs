using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Contratos
{
    /// <summary>
    /// Lógica de interacción para AltaContrato.xaml
    /// </summary>
    public partial class AltaContrato : Window
    {
        public ObservableCollection<ContratoDetalle> Items { get; set; }

        public AltaContrato()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<ContratoDetalle>();
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            var granos = ControladorGrano.VerTodos();
            var lista = new List<string>();
            if (granos != null)
            {
                granos.ForEach(x => {
                    lista.Add(x.Tipo);
                });
                granoTipo.ItemsSource = lista;
            }
        }

        private void ChequearProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (cuitCuil.Text != "")
            {
                if (!ControladorProveedor.Existe(cuitCuil.Text))
                    MessageBox.Show("El Proveedor no existe", "Resultado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("El Proveedor es " + ControladorProveedor.VerProveedor(cuitCuil.Text).RazonSocial, "Resultado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void NuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            AltaProveedor altaProveedorVentana = new AltaProveedor()
            { Owner = this };
            altaProveedorVentana.ShowDialog();
        }

        private void NuevoGrano_Click(object sender, RoutedEventArgs e)
        {
            AltaGrano altaGranoVentana = new AltaGrano()
            { Owner = this };
            altaGranoVentana.ShowDialog();
            if (altaGranoVentana.Refrescar)
                PopulateComboBox();
        }

        private void NuevoDetalle_Click(object sender, RoutedEventArgs e)
        {
            AltaContratoDetalle altaDetalleVentana = new AltaContratoDetalle()
            { Owner = this };
            altaDetalleVentana.ShowDialog();
            if (altaDetalleVentana.Nombre != "" && altaDetalleVentana.Unidad != "" && altaDetalleVentana.Valor != 0)
            {
                var item = new ContratoDetalle(0, altaDetalleVentana.Valor)
                {
                    Condicion = new Condicion(0, altaDetalleVentana.Nombre, altaDetalleVentana.Unidad)
                };
                if (!Items.Any(x => x.Condicion.Nombre == altaDetalleVentana.Nombre))
                {
                    Items.Add(item);
                }
                else
                {
                    MessageBox.Show("La condición seleccionada ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelarContrato_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GuardarSalirContrato_Click(object sender, RoutedEventArgs e)
        {
            if (cuitCuil.Text != "" &&
            granoTipo.Text != "" &&
            numero.Text != "" &&
            valor.Text != "" &&
            precio.Text != "" &&
            fechaLabra.SelectedDate != null &&
            fechaLimite.SelectedDate != null &&
            Items.Count != 0)
            {
                if (ControladorProveedor.VerProveedor(cuitCuil.Text) != null)
                {
                    string tipoCantidad;
                    List<ContratoDetalle> detalles = Items.ToList();

                    if (Camiones.IsChecked == true)
                        tipoCantidad = "Camiones";
                    else
                        tipoCantidad = "Toneladas";

                    if (ControladorContrato.IngresarContrato(
                        cuitCuil.Text,
                        detalles,
                        granoTipo.Text,
                        Int32.Parse(valor.Text),
                        fechaLabra.SelectedDate.Value.Date,
                        fechaLimite.SelectedDate.Value.Date,
                        Int32.Parse(numero.Text),
                        float.Parse(precio.Text),
                        tipoCantidad))
                    {
                        MessageBox.Show("Operación exitosa", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else MessageBox.Show("Error en la operación. Compruebe los datos o contacte con el administrador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("El proveedor no existe. Asegurese de que el CUIL/CUIT ingresado es correcto. En caso de ser correcto debe ingresar sus datos a la base de datos presionando el boton 'Nuevo' en la parte del proveedor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else MessageBox.Show("No deje campos vacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EliminarDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count != 0 && grillaDetalles.SelectedItem != null)
            {
                ContratoDetalle item = (ContratoDetalle)grillaDetalles.SelectedItem;
                Items.Remove(item);
            }
        }

        private void DateFechaLabra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fechaLabra.SelectedDate != null) fechaLimite.IsEnabled = true;
        }

        private void DateFechaLimite_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime SelDate = (DateTime)e.AddedItems[0];
            DateTime SelDate2 = fechaLabra.SelectedDate.Value;
            if (SelDate <= SelDate2)
            {
                MessageBox.Show("La fecha limite no puede ser igual o menor que la inicial.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                fechaLimite.SelectedDate = SelDate2.AddDays(1);
            }
        }

        private void VerProveedores_Click(object sender, RoutedEventArgs e)
        {
            VerProveedores verProveedoresVentana = new VerProveedores()
            { Owner = this };
            verProveedoresVentana.CuitCuilSeleccionado = cuitCuil.Text;
            verProveedoresVentana.ShowDialog();
            cuitCuil.Text = verProveedoresVentana.CuitCuilSeleccionado;
        }
    }
}
