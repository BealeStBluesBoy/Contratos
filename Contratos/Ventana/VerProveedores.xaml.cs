using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Contratos
{
    /// <summary>
    /// Lógica de interacción para VerProveedores.xaml
    /// </summary>
    public partial class VerProveedores : Window
    {
        private List<GridItem> items = new List<GridItem>();
        public string CuitCuilSeleccionado { get; set; }

        public VerProveedores()
        {
            InitializeComponent();
            PopulateGrid();
        }

        private class GridItem
        {
            public string CuitCuil { get; set; }
            public string RazonSocial { get; set; }
            public string IngresosBrutos { get; set; }
            public string InicioActividades { get; set; }

            public GridItem(string cuitCuil, string razonSocial, string ingresosBrutos, string inicioActividades)
            {
                CuitCuil = cuitCuil;
                RazonSocial = razonSocial;
                IngresosBrutos = ingresosBrutos;
                InicioActividades = inicioActividades;
            }
        }

        private void PopulateGrid()
        {
            ControladorProveedor dbProveedor = new ControladorProveedor();
            var provs = dbProveedor.VerTodos();
            List<GridItem> lista = new List<GridItem>();
            if (provs != null)
            {
                provs.ForEach(x => {
                    lista.Add(new GridItem(x.CuitCuil, x.RazonSocial, x.IngresosBrutos, x.InicioActividades.ToString("dd MMM yyyy")));
                });
                grillaProveedores.ItemsSource = lista;
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCellInfo cell = grillaProveedores.SelectedCells[0];
            CuitCuilSeleccionado = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
            Close();
        }

        private void NuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            AltaProveedor altaProveedorVentana = new AltaProveedor()
            { Owner = this };
            altaProveedorVentana.ShowDialog();
            PopulateGrid();
        }
    }
}
