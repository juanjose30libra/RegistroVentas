using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistroVentas
{
    public partial class Form1 : Form
    {
        private List<Venta> listaVentas = new List<Venta>();

        public Form1()
        {
            InitializeComponent();
            dgvVentas.DataSource = listaVentas;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void MostrarDatos()
        {
            dgvVentas.DataSource = null;
            dgvVentas.DataSource = listaVentas;

            dgvVentas.Columns["Numero"].HeaderText = "N°";
            dgvVentas.Columns["Producto"].HeaderText = "Producto";
            dgvVentas.Columns["Cantidad"].HeaderText = "Cantidad";
            dgvVentas.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
            dgvVentas.Columns["Subtotal"].HeaderText = "Subtotal";

            dgvVentas.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            dgvVentas.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
        }

        private bool ValidarDatos()
        {
            if (txtProducto.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el producto");
                txtProducto.Focus();
                return false;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad))
            {
                MessageBox.Show("Cantidad inválida");
                txtCantidad.Focus();
                return false;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero");
                txtCantidad.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Precio inválido");
                txtPrecio.Focus();
                return false;
            }

            if (precio <= 0)
            {
                MessageBox.Show("El precio debe ser mayor a cero");
                txtPrecio.Focus();
                return false;
            }

            return true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;
            foreach (Venta venta in listaVentas)
            {
                if (venta.Producto.ToUpper() == txtProducto.Text.ToUpper())
                {
                    MessageBox.Show("Ese producto ya fue registrado.");
                    txtProducto.Focus();
                    return;
                }
            }

            Venta v = new Venta();
            v.Numero = listaVentas.Count + 1;
            v.Producto = txtProducto.Text;
            v.Cantidad = int.Parse(txtCantidad.Text);
            v.PrecioUnitario = decimal.Parse(txtPrecio.Text.Trim());
            listaVentas.Add(v);

            MostrarDatos();

            MessageBox.Show("Producto agregado correctamente.");

            // Limpiar los datos ingresados
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();

            // Limpiar los resultados calculados
            txtValorVenta.Clear();
            txtIGV.Clear();
            txtTotal.Clear();

            // Volver al primer TextBox
            txtProducto.Focus();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (listaVentas.Count == 0)
            {
                MessageBox.Show("No existen ventas registradas.");
                return;
            }

            decimal total = 0;
            foreach (Venta v in listaVentas)
            {
                total += v.Subtotal;
            }

            decimal valorVenta = total / 1.18m;
            decimal igv = total - valorVenta;

            txtValorVenta.Text = valorVenta.ToString("0.00");
            txtIGV.Text = igv.ToString("0.00");
            txtTotal.Text = total.ToString("0.00");
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();
            txtValorVenta.Clear();
            txtIGV.Clear();
            txtTotal.Clear();
            listaVentas.Clear();

            MostrarDatos();
            txtProducto.Focus();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
        !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
        !char.IsDigit(e.KeyChar) &&
        e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAgregar.PerformClick();
            }
        }
    }
}
