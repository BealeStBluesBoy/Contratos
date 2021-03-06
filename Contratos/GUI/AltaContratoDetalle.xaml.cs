﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Contratos.Controlador;

namespace Contratos.GUI
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
            var condiciones = ControladorCondicion.VerTodos();
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
                Unidad = ControladorCondicion.VerCondicion(nombreCondicion.Text).Unidad;
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
