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
        public ObservableCollection<Contrato> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<Contrato>();
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            ControladorContrato ctrl = new ControladorContrato();
            Items.Clear();
            ctrl.VerTodos().ForEach(x => {
                Items.Add(x);
            });
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
            if (int.TryParse(textboxBuscar.Text, out int result))
            {
                int cantCarac = textboxBuscar.Text.Length;
                if (Items.Count != 0)
                {
                    grillaContratos.ItemsSource = Items
                        .Where(x => x.Numero.ToString().Length >= cantCarac)
                        .Where(x => x.Numero.ToString().Substring(0, cantCarac) == textboxBuscar.Text);
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
            Contrato item = (Contrato)grillaContratos.SelectedItem;
            VerContrato(item.Numero);
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