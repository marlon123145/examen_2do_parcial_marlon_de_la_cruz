using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace examen_marlon_de_la_cruz
{
    public partial class frmAlquiler : Form
    {
        private readonly List<Alquiler> alquileres = new List<Alquiler>();
        private const decimal DESCUENTO = 0.15m;
        private const decimal DOMICILIO = 500m;
        private const decimal ITBIS = 0.18m;

        public frmAlquiler()
        {
            InitializeComponent();
            nudDias.Minimum = 1;
            nudDias.Maximum = 30;
            nudDias.Value = 1;
            rbBasico.Checked = true;
            rbSucursal.Checked = true;
            dtpSalida.Value = DateTime.Today;
            LimpiarResultados();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MessageBox.Show("Escribe el nombre del cliente.");
                txtCliente.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtVehiculo.Text))
            {
                MessageBox.Show("Escribe el nombre del vehículo.");
                txtVehiculo.Focus();
                return;
            }
            if (!decimal.TryParse(txtTarifa.Text, NumberStyles.Number,
                CultureInfo.CurrentCulture, out decimal tarifa) || tarifa <= 0)
            {
                MessageBox.Show("Escribe una tarifa válida, mayor que cero.");
                txtTarifa.Focus();
                return;
            }

            Alquiler alquiler = new Alquiler
            {
                Vehiculo = txtVehiculo.Text.Trim(),
                Tarifa = tarifa,
                Dias = (int)nudDias.Value,
                SeguroCompleto = rbCompleto.Checked,
                GPS = chkGps.Checked
            };
            alquileres.Add(alquiler);

            ListViewItem fila = new ListViewItem(alquiler.Dias.ToString());
            fila.SubItems.Add(alquiler.Vehiculo + (alquiler.SeguroCompleto ? " (completo)" : ""));
            fila.SubItems.Add(alquiler.Tarifa.ToString("N2"));
            fila.SubItems.Add(alquiler.CalcularTotal().ToString("N2"));
            fila.Tag = alquiler;
            lstAlquileres.Items.Add(fila);

            LimpiarResultados();
            txtVehiculo.Clear();
            txtTarifa.Clear();
            nudDias.Value = 1;
            rbBasico.Checked = true;
            chkGps.Checked = false;
            txtVehiculo.Focus();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (alquileres.Count == 0)
            {
                MessageBox.Show("Agrega al menos un vehículo antes de calcular.");
                return;
            }

            decimal subtotal = alquileres.Sum(a => a.CalcularTotal());
            decimal descuento = chkCorporativo.Checked ? subtotal * DESCUENTO : 0m;
            decimal domicilio = rbDomicilio.Checked ? DOMICILIO : 0m;
            decimal baseImponible = subtotal - descuento + domicilio;
            decimal impuesto = baseImponible * ITBIS;
            decimal total = baseImponible + impuesto;

            txtSubtotal.Text = subtotal.ToString("N2");
            txtDescuento.Text = descuento.ToString("N2");
            txtDomicilio.Text = domicilio.ToString("N2");
            txtItbis.Text = impuesto.ToString("N2");
            txtTotal.Text = total.ToString("N2");
            MessageBox.Show("Total a pagar: RD$ " + total.ToString("N2"),
                "RentCar Express", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lstAlquileres_DoubleClick(object sender, EventArgs e)
        {
            if (lstAlquileres.SelectedItems.Count == 0) return;
            ListViewItem fila = lstAlquileres.SelectedItems[0];
            if (MessageBox.Show("¿Eliminar este vehículo de la lista?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            alquileres.Remove((Alquiler)fila.Tag);
            lstAlquileres.Items.Remove(fila);
            LimpiarResultados();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            alquileres.Clear();
            lstAlquileres.Items.Clear();
            txtCliente.Clear();
            chkCorporativo.Checked = false;
            dtpSalida.Value = DateTime.Today;
            txtVehiculo.Clear();
            txtTarifa.Clear();
            nudDias.Value = 1;
            rbBasico.Checked = true;
            chkGps.Checked = false;
            rbSucursal.Checked = true;
            LimpiarResultados();
            txtCliente.Focus();
        }

        private void LimpiarResultados()
        {
            txtSubtotal.Text = "0.00";
            txtDescuento.Text = "0.00";
            txtDomicilio.Text = "0.00";
            txtItbis.Text = "0.00";
            txtTotal.Text = "0.00";
        }

        private void btnSalir_Click(object sender, EventArgs e) => Close();

        private void frmAlquiler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("¿Deseas salir del programa?", "Confirmar salida",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }
    }
}
