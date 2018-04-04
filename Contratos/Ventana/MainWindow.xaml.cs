using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace Contratos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<GridItem> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<GridItem>();
            PopulateGrid();
        }

        public class GridItem
        {
            public string Numero { get; set; }
            public string RazonSocial { get; set; }
            public int Cantidad { get; set; }
            public string Tipo { get; set; }
            public float Precio { get; set; }
            public string FechaLabra { get; set; }
            public string FechaLimite { get; set; }
            public string Estado { get; set; }

            public GridItem(string num, string razSoc, int cant, string tipo, float prec, string fLabra, string fLimite, string cerrado)
            {
                Numero = num;
                RazonSocial = razSoc;
                Tipo = tipo;
                Cantidad = cant;
                Precio = prec;
                FechaLabra = fLabra;
                FechaLimite = fLimite;
                Estado = cerrado;
            }
        }

        public void PopulateGrid()
        {
            string Estado(bool estado)
            {
                if (estado)
                    return "Cerrado";
                else
                    return "Abierto";
            }

            ControladorContrato ctrl = new ControladorContrato();
            List<Contrato> contratos = ctrl.VerTodos();
            Items.Clear();
            if (contratos.Count != 0)
            {
                contratos.ForEach(x => {
                    Items.Add(new GridItem(x.Numero.ToString().PadLeft(8, '0'), x.Proveedor.RazonSocial, x.Cantidad, x.TipoContrato, x.Precio, x.FechaLabra.ToString("dd MMM yyyy"), x.FechaLimite.ToString("dd MMM yyyy"), Estado(x.Cerrado)));
                });
            }
        }

        private void MenuContrato_Click(object sender, RoutedEventArgs e)
        {
            AltaContrato altaContratoVentana = new AltaContrato
            { Owner = this };
            altaContratoVentana.ShowDialog();
            if (altaContratoVentana.Refrescar)
                PopulateGrid();
        }

        private void MenuCondicion_Click(object sender, RoutedEventArgs e)
        {
            AltaCondicion altaCondicionVentana = new AltaCondicion()
            { Owner = this };
            altaCondicionVentana.ShowDialog();
        }

        private void MenuGrano_Click(object sender, RoutedEventArgs e)
        {
            AltaGrano altaGranoVentana = new AltaGrano()
            { Owner = this };
            altaGranoVentana.ShowDialog();
        }

        private void MenuProveedor_Click(object sender, RoutedEventArgs e)
        {
            AltaProveedor altaProveedorVentana = new AltaProveedor()
            { Owner = this };
            altaProveedorVentana.ShowDialog();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(textboxBuscar.Text, out int result))
            {
                int cantCarac = textboxBuscar.Text.Length;
                if (Items.Count != 0)
                {
                    grillaContratos.ItemsSource = Items
                        .Where(x => x.Numero.Length >= cantCarac)
                        .Where(x => x.Numero.Substring(0, cantCarac) == textboxBuscar.Text);
                }
            }
            else
            {
                grillaContratos.ItemsSource = Items;
            }
        }

        private void MenuSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VerContrato(int numero)
        {
            VerEditarContrato verEditarContratoVentana = new VerEditarContrato(numero)
            { Owner = this };
            verEditarContratoVentana.ShowDialog();
            if (verEditarContratoVentana.Refrescar)
                PopulateGrid();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCellInfo cell = grillaContratos.SelectedCells[0];
            VerContrato(Int32.Parse(((TextBlock)cell.Column.GetCellContent(cell.Item)).Text));
        }

        private void MenuVerProveedores_Click(object sender, RoutedEventArgs e)
        {
            VerProveedores verProveedoresVentana = new VerProveedores()
            { Owner = this };
            verProveedoresVentana.ShowDialog();
        }

        private void MenuGen_Click(object sender, RoutedEventArgs e)
        {
            GenerarContrato generarContrato = new GenerarContrato()
            { Owner = this };
            generarContrato.ShowDialog();
            if (generarContrato.Refrescar)
                PopulateGrid();
        }

        private void Refrescar_Click(object sender, RoutedEventArgs e)
        {
            PopulateGrid();
        }
    }
}