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
    public partial class VerEditarContrato : Window
    {
        private bool enableEdit = false;
        private DateTime fechaLabraAux;
        private int numeroaux;
        private ObservableCollection<GridItem> items = new ObservableCollection<GridItem>();

        public VerEditarContrato(Contrato contrato)
        {
            InitializeComponent();
            PopulateComboBox();
            PopulateGrid();
            CargarDatos(contrato);
        }

        private void CargarDatos(Contrato contrato)
        {
            fechaLabra.Content += contrato.FechaLabra.ToString("dd MMM yyyy");
            cuitCuil.Content += contrato.Proveedor.CuitCuil.ToString();
            razonSocial.Content += contrato.Proveedor.RazonSocial;
            inicioActividades.Content += contrato.Proveedor.InicioActividades.ToString("dd MMM yyyy");
            ingresosBrutos.Content += contrato.Proveedor.IngresosBrutos.ToString();
            numero.Content += contrato.Numero.ToString();
            valor.Text = contrato.Cantidad.ToString();
            precio.Text = contrato.Precio.ToString();
            fechaLimite.SelectedDate = contrato.FechaLimite;
            if (contrato.TipoContrato == "Camiones") Camiones.IsChecked = true;
            else Toneladas.IsChecked = true;
            if (contrato.Cerrado)
            {
                Estado.Content += "Cerrado";
                CerrarContrato.Visibility = Visibility.Collapsed;
                EditarContrato.Visibility = Visibility.Collapsed;
                EditarContrato.IsEnabled = false;
            }
            else Estado.Content += "Abierto";
            granoTipo.Text = contrato.Grano.Tipo;
            contrato.Detalles.ForEach(x => {
                items.Add(new GridItem(x.Condicion.Nombre, x.Condicion.Unidad, x.Valor));
            });
            PopulateGrid();
            fechaLabraAux = contrato.FechaLabra;
            numeroaux = contrato.Numero;
        }

        private class GridItem
        {
            public string Nombre { get; set; }
            public string Unidad { get; set; }
            public float Valor { get; set; }

            public GridItem(string nom, string unid, float val)
            {
                Nombre = nom;
                Unidad = unid;
                Valor = val;
            }
        }

        public void PopulateGrid()
        {
            if (items.Count != 0)
            {
                grillaDetalles.ItemsSource = items;
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

        private void NuevoGrano_Click(object sender, RoutedEventArgs e)
        {
            AltaGrano altaGranoVentana = new AltaGrano()
            { Owner = this };
            altaGranoVentana.ShowDialog();
            PopulateComboBox();
        }

        private void NuevoDetalle_Click(object sender, RoutedEventArgs e)
        {
            AltaContratoDetalle altaContratoDetalleVentana = new AltaContratoDetalle()
            { Owner = this };
            altaContratoDetalleVentana.ShowDialog();
            if ((altaContratoDetalleVentana.Nombre != "") && (altaContratoDetalleVentana.Unidad != "") && (altaContratoDetalleVentana.Valor != 0))
            {
                var item = new GridItem(altaContratoDetalleVentana.Nombre, altaContratoDetalleVentana.Unidad, altaContratoDetalleVentana.Valor);
                bool existe = false;
                items.ToList().ForEach(x =>
                {
                    if (x.Nombre == altaContratoDetalleVentana.Nombre) existe = true;
                });
                if (!existe)
                {
                    items.Add(item);
                }
                else
                {
                    MessageBox.Show("La condición seleccionada ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            PopulateGrid();
        }

        private void CancelarContrato_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HabilitarEdicion()
        {
            enableEdit = true;
            CerrarContrato.Visibility = Visibility.Collapsed;
            NuevoDetalle.IsEnabled = true;
            EliminarDetalles.IsEnabled = true;
            granoTipo.IsEnabled = true;
            NuevoGrano.IsEnabled = true;
            valor.IsEnabled = true;
            Camiones.IsEnabled = true;
            Toneladas.IsEnabled = true;
            precio.IsEnabled = true;
            fechaLimite.IsEnabled = true;
            CancelarContrato.Content = "Cancelar";
            EditarContrato.Content = "Guardar y salir";
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if ((enableEdit == false) && (Estado.Content.ToString() == "Estado: Abierto"))
            {
                HabilitarEdicion();
            }
            else if (enableEdit == true)
            {
                if ((granoTipo.Text != "") &&
                (valor.Text != "") &&
                (precio.Text != "") &&
                (fechaLimite.SelectedDate != null) &&
                (items.Count != 0))
                {
                    string tipoContrato;
                    if (Camiones.IsChecked == true) tipoContrato = "Camiones";
                    else tipoContrato = "Toneladas";
                    List<ContratoDetalle> detalles = new List<ContratoDetalle>();
                    items.ToList().ForEach(x =>
                    {
                        var condicion = new Condicion(0, x.Nombre, x.Unidad);
                        var det = new ContratoDetalle(0, x.Valor);
                        det.AgregarCondicion(condicion);
                        detalles.Add(det);
                    });
                    
                    if(ControladorContrato.ActualizarContrato(numeroaux, 
                        Int32.Parse(valor.Text),
                        (DateTime)fechaLimite.SelectedDate, 
                        float.Parse(precio.Text, System.Globalization.CultureInfo.InvariantCulture.NumberFormat), 
                        granoTipo.Text, 
                        tipoContrato, 
                        detalles))
                    {
                        MessageBox.Show("Operación exitosa", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else MessageBox.Show("Error en la operación. Compruebe los datos o contacte con el administrador.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("No deje campos vacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else MessageBox.Show("No se puede modificar contratos cerrados.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EliminarDetalles_Click(object sender, RoutedEventArgs e)
        {
            if ((items.Count != 0) && (grillaDetalles.SelectedItem != null))
            {
                int cantElementos = items.Count;
                items.ToList().ForEach(x =>
                {
                    while (cantElementos == items.Count)
                    {
                        if (x.Nombre == ((TextBlock)grillaDetalles.SelectedCells[0].Column.GetCellContent(grillaDetalles.SelectedCells[0].Item)).Text)
                            items.RemoveAt(grillaDetalles.Items.IndexOf(grillaDetalles.SelectedItem));
                        break;
                    }
                });
            }
        }

       private void DateFechaLimite_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime SelDate = (DateTime)e.AddedItems[0];
            if (SelDate <= fechaLabraAux)
            {
                MessageBox.Show("La fecha limite no puede ser igual o menor que la inicial.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                fechaLimite.SelectedDate = fechaLabraAux.AddDays(1);
            }
        }

        private void EliminarContrato_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Está seguro de querer eliminar este contrato?", "Eliminar contrato", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                if (ControladorContrato.EliminarContrato(numeroaux))
                {
                    MessageBox.Show("Contrato eliminado satisfactoriamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else MessageBox.Show("Error en la operación, no se ha podido eliminar el contrato. Contacte con el administrador si el problema persiste.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CerrarContrato_Click(object sender, RoutedEventArgs e)
        {
            if (enableEdit)
            {
                MessageBox.Show("Edite el contrato antes de cerrarlo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("¿Está seguro de querer cerrar este contrato?", "Cerrar contrato", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    if (ControladorContrato.CerrarContrato(numeroaux))
                    {
                        CerrarContrato.Visibility = Visibility.Collapsed;
                        EditarContrato.Visibility = Visibility.Collapsed;
                        Estado.Content = "Estado: Cerrado";
                        EditarContrato.IsEnabled = false;
                        MessageBox.Show("Contrato cerrado satisfactoriamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Error en la operación, no se ha podido cerrar el contrato. Contacte con el administrador si el problema persiste.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
