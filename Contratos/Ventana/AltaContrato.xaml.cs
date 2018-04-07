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
        public bool Refrescar = false;
        public ObservableCollection<GridItem> Items { get; set; }

        public AltaContrato()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<GridItem>();
            PopulateComboBox();
        }

        public class GridItem
        {
            public string Condicion { get; set; }
            public float Valor { get; set; }
            public string Unidad { get; set; }

            public GridItem(string nom, string unid, float val)
            {
                Condicion = nom;
                Unidad = unid;
                Valor = val;
            }
        }

        private void PopulateComboBox()
        {
            ControladorGrano ctrl = new ControladorGrano();
            var granos = ctrl.VerTodos();
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
            ControladorProveedor ctrl = new ControladorProveedor();
            if (cuitCuil.Text != "")
            {
                if (ctrl.Existe(cuitCuil.Text))
                    MessageBox.Show("El Proveedor no existe", "Resultado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("El Proveedor es " + ctrl.VerProveedor(cuitCuil.Text).RazonSocial, "Resultado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                var item = new GridItem(altaDetalleVentana.Nombre, altaDetalleVentana.Unidad, altaDetalleVentana.Valor);
                bool existe = false;
                Items.ToList().ForEach(x =>
                {
                    if (x.Condicion == altaDetalleVentana.Nombre)
                        existe = true;
                });
                if (!existe)
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
            ControladorProveedor ctrlPro = new ControladorProveedor();
            if (cuitCuil.Text != "" &&
            granoTipo.Text != "" &&
            numero.Text != "" &&
            valor.Text != "" &&
            precio.Text != "" &&
            fechaLabra.SelectedDate != null &&
            fechaLimite.SelectedDate != null &&
            Items.Count != 0)
            {
                if (ctrlPro.VerProveedor(cuitCuil.Text) != null)
                {
                    string tipoCantidad;
                    List<ContratoDetalle> detalles = new List<ContratoDetalle>();
                    Items.ToList().ForEach(x =>
                    {
                        var condicion = new Condicion(0, x.Condicion, x.Unidad);
                        var det = new ContratoDetalle(0, x.Valor);
                        det.AgregarCondicion(condicion);
                        detalles.Add(det);
                    });

                    if (Camiones.IsChecked == true)
                        tipoCantidad = "Camiones";
                    else
                        tipoCantidad = "Toneladas";

                    ControladorContrato ctrlCon = new ControladorContrato();
                    if (ctrlCon.IngresarContrato(
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
                        Refrescar = true;
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
                int cantElementos = Items.Count;
                Items.ToList().ForEach(x =>
                {
                    while (cantElementos == Items.Count)
                    {
                        GridItem item = (GridItem)grillaDetalles.SelectedItem;
                        if (x.Condicion == item.Condicion)
                            Items.RemoveAt(grillaDetalles.Items.IndexOf(grillaDetalles.SelectedItem));
                        break;
                    }
                });
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
