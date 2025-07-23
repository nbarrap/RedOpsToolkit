using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class VentasForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvVentas, dgvDetalleVenta, dgvProductos;
        private ComboBox cmbCliente, cmbTipoPago;
        private TextBox txtBuscarProducto, txtObservaciones;
        private NumericUpDown txtCantidad;
        private Label lblTotal, lblTotalArticulos;
        private Button btnNuevaVenta, btnAgregarProducto, btnEliminarProducto, btnGuardarVenta, btnCancelar;
        private DateTimePicker dtpFecha;
        
        private Venta ventaActual;
        private bool modoVenta = false;

        public VentasForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
            CargarVentas();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Ventas";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            this.Controls.Add(mainPanel);

            // Panel superior - Lista de ventas
            var panelVentas = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(panelVentas, 0, 0);

            var lblTituloVentas = new Label
            {
                Text = "HISTORIAL DE VENTAS",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelVentas.Controls.Add(lblTituloVentas);

            btnNuevaVenta = new Button
            {
                Text = "Nueva Venta",
                Location = new Point(this.Width - 150, 10),
                Size = new Size(100, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnNuevaVenta.Click += BtnNuevaVenta_Click;
            panelVentas.Controls.Add(btnNuevaVenta);

            dgvVentas = new DataGridView
            {
                Location = new Point(10, 50),
                Size = new Size(this.Width - 40, panelVentas.Height - 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelVentas.Controls.Add(dgvVentas);

            // Panel inferior - Nueva venta
            var panelNuevaVenta = new Panel { Dock = DockStyle.Fill, Visible = false };
            mainPanel.Controls.Add(panelNuevaVenta, 0, 1);

            // Configurar el panel de nueva venta
            ConfigurarPanelNuevaVenta(panelNuevaVenta);
        }

        private void ConfigurarPanelNuevaVenta(Panel panel)
        {
            var lblTitulo = new Label
            {
                Text = "NUEVA VENTA",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(150, 25)
            };
            panel.Controls.Add(lblTitulo);

            // Primera fila - Información de la venta
            var y = 45;
            
            var lblFecha = new Label { Text = "Fecha:", Location = new Point(10, y), Size = new Size(50, 23) };
            dtpFecha = new DateTimePicker { Location = new Point(70, y), Size = new Size(120, 23) };
            
            var lblCliente = new Label { Text = "Cliente:", Location = new Point(210, y), Size = new Size(50, 23) };
            cmbCliente = new ComboBox 
            { 
                Location = new Point(270, y), 
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            var lblTipoPago = new Label { Text = "Tipo Pago:", Location = new Point(490, y), Size = new Size(70, 23) };
            cmbTipoPago = new ComboBox 
            { 
                Location = new Point(570, y), 
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            panel.Controls.AddRange(new Control[] { lblFecha, dtpFecha, lblCliente, cmbCliente, lblTipoPago, cmbTipoPago });

            y += 35;

            // Segunda fila - Búsqueda de productos
            var lblBuscar = new Label { Text = "Buscar Producto:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtBuscarProducto = new TextBox { Location = new Point(120, y), Size = new Size(200, 23) };
            txtBuscarProducto.TextChanged += TxtBuscarProducto_TextChanged;

            var lblCantidad = new Label { Text = "Cantidad:", Location = new Point(340, y), Size = new Size(60, 23) };
            txtCantidad = new NumericUpDown { Location = new Point(410, y), Size = new Size(80, 23), Minimum = 1, Value = 1 };

            btnAgregarProducto = new Button { Text = "Agregar", Location = new Point(500, y), Size = new Size(80, 25) };
            btnAgregarProducto.Click += BtnAgregarProducto_Click;

            panel.Controls.AddRange(new Control[] { lblBuscar, txtBuscarProducto, lblCantidad, txtCantidad, btnAgregarProducto });

            y += 35;

            // Tabla de productos disponibles
            dgvProductos = new DataGridView
            {
                Location = new Point(10, y),
                Size = new Size(400, 120),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Tabla de detalle de venta
            dgvDetalleVenta = new DataGridView
            {
                Location = new Point(420, y),
                Size = new Size(400, 120),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnEliminarProducto = new Button 
            { 
                Text = "Eliminar Producto", 
                Location = new Point(830, y), 
                Size = new Size(100, 30) 
            };
            btnEliminarProducto.Click += BtnEliminarProducto_Click;

            panel.Controls.AddRange(new Control[] { dgvProductos, dgvDetalleVenta, btnEliminarProducto });

            y += 130;

            // Totales y botones
            lblTotalArticulos = new Label 
            { 
                Text = "Total Artículos: 0", 
                Location = new Point(420, y), 
                Size = new Size(150, 23),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            lblTotal = new Label 
            { 
                Text = "TOTAL: $0.00", 
                Location = new Point(580, y), 
                Size = new Size(150, 23),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red
            };

            btnGuardarVenta = new Button 
            { 
                Text = "Guardar Venta", 
                Location = new Point(750, y), 
                Size = new Size(100, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnGuardarVenta.Click += BtnGuardarVenta_Click;

            btnCancelar = new Button 
            { 
                Text = "Cancelar", 
                Location = new Point(860, y), 
                Size = new Size(70, 30)
            };
            btnCancelar.Click += BtnCancelar_Click;

            panel.Controls.AddRange(new Control[] { lblTotalArticulos, lblTotal, btnGuardarVenta, btnCancelar });

            // Observaciones
            y += 40;
            var lblObservaciones = new Label { Text = "Observaciones:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtObservaciones = new TextBox 
            { 
                Location = new Point(120, y), 
                Size = new Size(300, 23),
                Multiline = true,
                Height = 50
            };

            panel.Controls.AddRange(new Control[] { lblObservaciones, txtObservaciones });

            // Cargar datos iniciales
            CargarComboClientes();
            CargarComboTipoPago();
            CargarProductosDisponibles();
        }

        private void CargarVentas()
        {
            var ventas = dataManager.Ventas.OrderByDescending(v => v.FechaVenta).ToList();
            
            dgvVentas.DataSource = ventas.Select(v => new
            {
                Id = v.Id,
                Fecha = v.FechaVenta.ToString("dd/MM/yyyy HH:mm"),
                Cliente = v.NombreCliente,
                TipoPago = v.TipoPago.ToString(),
                TotalArticulos = v.TotalArticulos,
                Total = v.Total
            }).ToList();

            if (dgvVentas.Columns.Count > 0)
            {
                dgvVentas.Columns["Id"].Visible = false;
                dgvVentas.Columns["Fecha"].HeaderText = "Fecha";
                dgvVentas.Columns["Cliente"].HeaderText = "Cliente";
                dgvVentas.Columns["TipoPago"].HeaderText = "Tipo Pago";
                dgvVentas.Columns["TotalArticulos"].HeaderText = "Artículos";
                dgvVentas.Columns["Total"].HeaderText = "Total";
                dgvVentas.Columns["Total"].DefaultCellStyle.Format = "C2";
            }
        }

        private void CargarComboClientes()
        {
            cmbCliente.Items.Clear();
            cmbCliente.Items.Add(new { Text = "Cliente Ocasional", Value = (int?)null });
            
            foreach (var cliente in dataManager.Clientes)
            {
                cmbCliente.Items.Add(new { Text = cliente.NombreCompleto, Value = (int?)cliente.Id });
            }
            
            cmbCliente.DisplayMember = "Text";
            cmbCliente.ValueMember = "Value";
            cmbCliente.SelectedIndex = 0;
        }

        private void CargarComboTipoPago()
        {
            cmbTipoPago.Items.Clear();
            foreach (TipoPago tipo in Enum.GetValues(typeof(TipoPago)))
            {
                cmbTipoPago.Items.Add(tipo);
            }
            cmbTipoPago.SelectedIndex = 0;
        }

        private void CargarProductosDisponibles()
        {
            var productos = dataManager.Productos.Where(p => p.Stock > 0).ToList();
            
            dgvProductos.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();

            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["Id"].Visible = false;
                dgvProductos.Columns["Codigo"].HeaderText = "Código";
                dgvProductos.Columns["Nombre"].HeaderText = "Producto";
                dgvProductos.Columns["Precio"].HeaderText = "Precio";
                dgvProductos.Columns["Stock"].HeaderText = "Stock";
                dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
            }
        }

        private void TxtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            var productos = dataManager.BuscarProductos(txtBuscarProducto.Text)
                                     .Where(p => p.Stock > 0).ToList();
            
            dgvProductos.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();
        }

        private void ActualizarDetalleVenta()
        {
            if (ventaActual?.Detalles != null)
            {
                dgvDetalleVenta.DataSource = ventaActual.Detalles.Select(d => new
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    Codigo = d.Producto?.Codigo,
                    Producto = d.Producto?.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList();

                if (dgvDetalleVenta.Columns.Count > 0)
                {
                    dgvDetalleVenta.Columns["Id"].Visible = false;
                    dgvDetalleVenta.Columns["ProductoId"].Visible = false;
                    dgvDetalleVenta.Columns["Codigo"].HeaderText = "Código";
                    dgvDetalleVenta.Columns["Producto"].HeaderText = "Producto";
                    dgvDetalleVenta.Columns["Cantidad"].HeaderText = "Cant.";
                    dgvDetalleVenta.Columns["PrecioUnitario"].HeaderText = "Precio";
                    dgvDetalleVenta.Columns["Subtotal"].HeaderText = "Subtotal";
                    dgvDetalleVenta.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dgvDetalleVenta.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                }

                lblTotalArticulos.Text = $"Total Artículos: {ventaActual.TotalArticulos}";
                lblTotal.Text = $"TOTAL: {ventaActual.TotalCalculado:C2}";
            }
        }

        private void BtnNuevaVenta_Click(object sender, EventArgs e)
        {
            ventaActual = new Venta();
            modoVenta = true;
            
            // Mostrar panel de nueva venta
            var panelNuevaVenta = this.Controls.Cast<Control>()
                .FirstOrDefault(c => c is TableLayoutPanel)
                ?.Controls.Cast<Control>()
                .Skip(1).FirstOrDefault() as Panel;
            
            if (panelNuevaVenta != null)
            {
                panelNuevaVenta.Visible = true;
            }

            btnNuevaVenta.Enabled = false;
            dtpFecha.Value = DateTime.Now;
            ActualizarDetalleVenta();
        }

        private void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0 && modoVenta)
            {
                var row = dgvProductos.SelectedRows[0];
                var productoId = (int)row.Cells["Id"].Value;
                var producto = dataManager.ObtenerProducto(productoId);
                var cantidad = (int)txtCantidad.Value;

                if (producto != null && dataManager.TieneStockSuficiente(productoId, cantidad))
                {
                    // Verificar si el producto ya está en el detalle
                    var detalleExistente = ventaActual.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
                    if (detalleExistente != null)
                    {
                        detalleExistente.Cantidad += cantidad;
                    }
                    else
                    {
                        var detalle = new DetalleVenta
                        {
                            ProductoId = productoId,
                            Producto = producto,
                            Cantidad = cantidad,
                            PrecioUnitario = producto.Precio
                        };
                        ventaActual.Detalles.Add(detalle);
                    }

                    ActualizarDetalleVenta();
                    txtCantidad.Value = 1;
                }
                else
                {
                    MessageBox.Show("Stock insuficiente para este producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDetalleVenta.SelectedRows.Count > 0 && modoVenta)
            {
                var row = dgvDetalleVenta.SelectedRows[0];
                var productoId = (int)row.Cells["ProductoId"].Value;
                
                var detalle = ventaActual.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
                if (detalle != null)
                {
                    ventaActual.Detalles.Remove(detalle);
                    ActualizarDetalleVenta();
                }
            }
        }

        private void BtnGuardarVenta_Click(object sender, EventArgs e)
        {
            if (ventaActual?.Detalles?.Count > 0)
            {
                ventaActual.FechaVenta = dtpFecha.Value;
                
                var clienteSeleccionado = cmbCliente.SelectedItem as dynamic;
                ventaActual.ClienteId = clienteSeleccionado?.Value;
                
                ventaActual.TipoPago = (TipoPago)cmbTipoPago.SelectedItem;
                ventaActual.Observaciones = txtObservaciones.Text;

                try
                {
                    dataManager.AgregarVenta(ventaActual);
                    MessageBox.Show("Venta registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    CargarVentas();
                    BtnCancelar_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe agregar al menos un producto a la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            modoVenta = false;
            ventaActual = null;
            
            // Ocultar panel de nueva venta
            var panelNuevaVenta = this.Controls.Cast<Control>()
                .FirstOrDefault(c => c is TableLayoutPanel)
                ?.Controls.Cast<Control>()
                .Skip(1).FirstOrDefault() as Panel;
            
            if (panelNuevaVenta != null)
            {
                panelNuevaVenta.Visible = false;
            }

            btnNuevaVenta.Enabled = true;
            
            // Limpiar campos
            txtBuscarProducto.Clear();
            txtObservaciones.Clear();
            txtCantidad.Value = 1;
        }
    }
}