using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class PedidosForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvPedidos, dgvDetallePedido, dgvProductos;
        private ComboBox cmbCliente, cmbEstado;
        private TextBox txtBuscarProducto, txtObservaciones;
        private NumericUpDown txtCantidad;
        private Label lblTotal, lblTotalArticulos;
        private Button btnNuevoPedido, btnAgregarProducto, btnEliminarProducto, btnGuardarPedido, btnCancelar, btnCambiarEstado;
        private DateTimePicker dtpFecha;
        
        private Pedido pedidoActual;
        private bool modoPedido = false;

        public PedidosForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
            CargarPedidos();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Pedidos";
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

            // Panel superior - Lista de pedidos
            var panelPedidos = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(panelPedidos, 0, 0);

            var lblTituloPedidos = new Label
            {
                Text = "LISTA DE PEDIDOS",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelPedidos.Controls.Add(lblTituloPedidos);

            btnNuevoPedido = new Button
            {
                Text = "Nuevo Pedido",
                Location = new Point(this.Width - 200, 10),
                Size = new Size(100, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnNuevoPedido.Click += BtnNuevoPedido_Click;
            panelPedidos.Controls.Add(btnNuevoPedido);

            btnCambiarEstado = new Button
            {
                Text = "Cambiar Estado",
                Location = new Point(this.Width - 90, 10),
                Size = new Size(100, 30),
                BackColor = Color.Orange,
                ForeColor = Color.White
            };
            btnCambiarEstado.Click += BtnCambiarEstado_Click;
            panelPedidos.Controls.Add(btnCambiarEstado);

            dgvPedidos = new DataGridView
            {
                Location = new Point(10, 50),
                Size = new Size(this.Width - 40, panelPedidos.Height - 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvPedidos.SelectionChanged += DgvPedidos_SelectionChanged;
            panelPedidos.Controls.Add(dgvPedidos);

            // Panel inferior - Nuevo pedido/detalle
            var panelNuevoPedido = new Panel { Dock = DockStyle.Fill, Visible = false };
            mainPanel.Controls.Add(panelNuevoPedido, 0, 1);

            ConfigurarPanelNuevoPedido(panelNuevoPedido);
        }

        private void ConfigurarPanelNuevoPedido(Panel panel)
        {
            var lblTitulo = new Label
            {
                Text = "NUEVO PEDIDO",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(150, 25)
            };
            panel.Controls.Add(lblTitulo);

            // Primera fila - Información del pedido
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
            
            var lblEstado = new Label { Text = "Estado:", Location = new Point(490, y), Size = new Size(50, 23) };
            cmbEstado = new ComboBox 
            { 
                Location = new Point(550, y), 
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            panel.Controls.AddRange(new Control[] { lblFecha, dtpFecha, lblCliente, cmbCliente, lblEstado, cmbEstado });

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

            // Tabla de detalle de pedido
            dgvDetallePedido = new DataGridView
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

            panel.Controls.AddRange(new Control[] { dgvProductos, dgvDetallePedido, btnEliminarProducto });

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
                ForeColor = Color.Blue
            };

            btnGuardarPedido = new Button 
            { 
                Text = "Guardar Pedido", 
                Location = new Point(750, y), 
                Size = new Size(100, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnGuardarPedido.Click += BtnGuardarPedido_Click;

            btnCancelar = new Button 
            { 
                Text = "Cancelar", 
                Location = new Point(860, y), 
                Size = new Size(70, 30)
            };
            btnCancelar.Click += BtnCancelar_Click;

            panel.Controls.AddRange(new Control[] { lblTotalArticulos, lblTotal, btnGuardarPedido, btnCancelar });

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
            CargarComboEstados();
            CargarProductosDisponibles();
        }

        private void CargarPedidos()
        {
            var pedidos = dataManager.Pedidos.OrderByDescending(p => p.FechaPedido).ToList();
            
            dgvPedidos.DataSource = pedidos.Select(p => new
            {
                Id = p.Id,
                Fecha = p.FechaPedido.ToString("dd/MM/yyyy"),
                Cliente = p.Cliente?.NombreCompleto ?? "N/A",
                Estado = p.Estado.ToString(),
                TotalArticulos = p.TotalArticulos,
                Total = p.Total
            }).ToList();

            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns["Id"].Visible = false;
                dgvPedidos.Columns["Fecha"].HeaderText = "Fecha";
                dgvPedidos.Columns["Cliente"].HeaderText = "Cliente";
                dgvPedidos.Columns["Estado"].HeaderText = "Estado";
                dgvPedidos.Columns["TotalArticulos"].HeaderText = "Artículos";
                dgvPedidos.Columns["Total"].HeaderText = "Total";
                dgvPedidos.Columns["Total"].DefaultCellStyle.Format = "C2";

                // Colorear según estado
                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    var estado = row.Cells["Estado"].Value.ToString();
                    switch (estado)
                    {
                        case "Pendiente":
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            break;
                        case "Procesando":
                            row.DefaultCellStyle.BackColor = Color.LightBlue;
                            break;
                        case "Completado":
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            break;
                        case "Cancelado":
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            break;
                    }
                }
            }
        }

        private void CargarComboClientes()
        {
            cmbCliente.Items.Clear();
            foreach (var cliente in dataManager.Clientes)
            {
                cmbCliente.Items.Add(cliente);
            }
            if (cmbCliente.Items.Count > 0)
                cmbCliente.SelectedIndex = 0;
        }

        private void CargarComboEstados()
        {
            cmbEstado.Items.Clear();
            foreach (EstadoPedido estado in Enum.GetValues(typeof(EstadoPedido)))
            {
                cmbEstado.Items.Add(estado);
            }
            cmbEstado.SelectedIndex = 0;
        }

        private void CargarProductosDisponibles()
        {
            var productos = dataManager.Productos.ToList();
            
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
            var productos = dataManager.BuscarProductos(txtBuscarProducto.Text);
            
            dgvProductos.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();
        }

        private void ActualizarDetallePedido()
        {
            if (pedidoActual?.Detalles != null)
            {
                dgvDetallePedido.DataSource = pedidoActual.Detalles.Select(d => new
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    Codigo = d.Producto?.Codigo,
                    Producto = d.Producto?.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList();

                if (dgvDetallePedido.Columns.Count > 0)
                {
                    dgvDetallePedido.Columns["Id"].Visible = false;
                    dgvDetallePedido.Columns["ProductoId"].Visible = false;
                    dgvDetallePedido.Columns["Codigo"].HeaderText = "Código";
                    dgvDetallePedido.Columns["Producto"].HeaderText = "Producto";
                    dgvDetallePedido.Columns["Cantidad"].HeaderText = "Cant.";
                    dgvDetallePedido.Columns["PrecioUnitario"].HeaderText = "Precio";
                    dgvDetallePedido.Columns["Subtotal"].HeaderText = "Subtotal";
                    dgvDetallePedido.Columns["PrecioUnitario"].DefaultCellStyle.Format = "C2";
                    dgvDetallePedido.Columns["Subtotal"].DefaultCellStyle.Format = "C2";
                }

                lblTotalArticulos.Text = $"Total Artículos: {pedidoActual.TotalArticulos}";
                lblTotal.Text = $"TOTAL: {pedidoActual.Total:C2}";
            }
        }

        private void DgvPedidos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count > 0 && !modoPedido)
            {
                var row = dgvPedidos.SelectedRows[0];
                var id = (int)row.Cells["Id"].Value;
                var pedidoSeleccionado = dataManager.ObtenerPedido(id);
                
                if (pedidoSeleccionado != null)
                {
                    MostrarDetallePedido(pedidoSeleccionado);
                }
            }
        }

        private void MostrarDetallePedido(Pedido pedido)
        {
            // Mostrar detalle en una ventana separada o panel
            var detalle = $"Pedido #{pedido.Id}\n";
            detalle += $"Cliente: {pedido.Cliente?.NombreCompleto}\n";
            detalle += $"Fecha: {pedido.FechaPedido:dd/MM/yyyy}\n";
            detalle += $"Estado: {pedido.Estado}\n";
            detalle += $"Total: {pedido.Total:C2}\n\n";
            detalle += "Productos:\n";
            
            foreach (var item in pedido.Detalles)
            {
                detalle += $"- {item.Producto?.Nombre} x{item.Cantidad} = {item.Subtotal:C2}\n";
            }

            MessageBox.Show(detalle, "Detalle del Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            pedidoActual = new Pedido();
            modoPedido = true;
            
            // Mostrar panel de nuevo pedido
            var panelNuevoPedido = this.Controls.Cast<Control>()
                .FirstOrDefault(c => c is TableLayoutPanel)
                ?.Controls.Cast<Control>()
                .Skip(1).FirstOrDefault() as Panel;
            
            if (panelNuevoPedido != null)
            {
                panelNuevoPedido.Visible = true;
            }

            btnNuevoPedido.Enabled = false;
            dtpFecha.Value = DateTime.Now;
            ActualizarDetallePedido();
        }

        private void BtnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0 && modoPedido)
            {
                var row = dgvProductos.SelectedRows[0];
                var productoId = (int)row.Cells["Id"].Value;
                var producto = dataManager.ObtenerProducto(productoId);
                var cantidad = (int)txtCantidad.Value;

                if (producto != null)
                {
                    var detalleExistente = pedidoActual.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
                    if (detalleExistente != null)
                    {
                        detalleExistente.Cantidad += cantidad;
                    }
                    else
                    {
                        var detalle = new DetallePedido
                        {
                            ProductoId = productoId,
                            Producto = producto,
                            Cantidad = cantidad,
                            PrecioUnitario = producto.Precio
                        };
                        pedidoActual.Detalles.Add(detalle);
                    }

                    ActualizarDetallePedido();
                    txtCantidad.Value = 1;
                }
            }
        }

        private void BtnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvDetallePedido.SelectedRows.Count > 0 && modoPedido)
            {
                var row = dgvDetallePedido.SelectedRows[0];
                var productoId = (int)row.Cells["ProductoId"].Value;
                
                var detalle = pedidoActual.Detalles.FirstOrDefault(d => d.ProductoId == productoId);
                if (detalle != null)
                {
                    pedidoActual.Detalles.Remove(detalle);
                    ActualizarDetallePedido();
                }
            }
        }

        private void BtnGuardarPedido_Click(object sender, EventArgs e)
        {
            if (pedidoActual?.Detalles?.Count > 0 && cmbCliente.SelectedItem is Cliente cliente)
            {
                pedidoActual.FechaPedido = dtpFecha.Value;
                pedidoActual.ClienteId = cliente.Id;
                pedidoActual.Estado = (EstadoPedido)cmbEstado.SelectedItem;
                pedidoActual.Observaciones = txtObservaciones.Text;

                try
                {
                    dataManager.AgregarPedido(pedidoActual);
                    MessageBox.Show("Pedido registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    CargarPedidos();
                    BtnCancelar_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el pedido: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente y agregar al menos un producto al pedido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            modoPedido = false;
            pedidoActual = null;
            
            // Ocultar panel de nuevo pedido
            var panelNuevoPedido = this.Controls.Cast<Control>()
                .FirstOrDefault(c => c is TableLayoutPanel)
                ?.Controls.Cast<Control>()
                .Skip(1).FirstOrDefault() as Panel;
            
            if (panelNuevoPedido != null)
            {
                panelNuevoPedido.Visible = false;
            }

            btnNuevoPedido.Enabled = true;
            
            // Limpiar campos
            txtBuscarProducto.Clear();
            txtObservaciones.Clear();
            txtCantidad.Value = 1;
        }

        private void BtnCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count > 0)
            {
                var row = dgvPedidos.SelectedRows[0];
                var id = (int)row.Cells["Id"].Value;
                var pedido = dataManager.ObtenerPedido(id);
                
                if (pedido != null)
                {
                    // Crear formulario para cambiar estado
                    var formEstado = new Form
                    {
                        Text = "Cambiar Estado del Pedido",
                        Size = new Size(350, 150),
                        StartPosition = FormStartPosition.CenterParent
                    };

                    var lblEstado = new Label
                    {
                        Text = "Nuevo Estado:",
                        Location = new Point(10, 20),
                        Size = new Size(80, 23)
                    };

                    var cmbNuevoEstado = new ComboBox
                    {
                        Location = new Point(100, 17),
                        Size = new Size(150, 23),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    foreach (EstadoPedido estado in Enum.GetValues(typeof(EstadoPedido)))
                    {
                        cmbNuevoEstado.Items.Add(estado);
                    }
                    cmbNuevoEstado.SelectedItem = pedido.Estado;

                    var btnAceptar = new Button
                    {
                        Text = "Aceptar",
                        Location = new Point(100, 60),
                        Size = new Size(75, 23),
                        DialogResult = DialogResult.OK
                    };

                    var btnCancelarEstado = new Button
                    {
                        Text = "Cancelar",
                        Location = new Point(185, 60),
                        Size = new Size(75, 23),
                        DialogResult = DialogResult.Cancel
                    };

                    formEstado.Controls.AddRange(new Control[] { lblEstado, cmbNuevoEstado, btnAceptar, btnCancelarEstado });

                    if (formEstado.ShowDialog() == DialogResult.OK)
                    {
                        var nuevoEstado = (EstadoPedido)cmbNuevoEstado.SelectedItem;
                        dataManager.ActualizarEstadoPedido(id, nuevoEstado);
                        MessageBox.Show("Estado del pedido actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarPedidos();
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un pedido para cambiar su estado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}