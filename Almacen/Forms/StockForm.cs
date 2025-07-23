using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class StockForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvStock, dgvBajoStock;
        private TextBox txtBuscar;
        private NumericUpDown txtCantidadAjuste;
        private ComboBox cmbTipoAjuste;
        private Button btnAjustarStock, btnReporte;
        private Label lblTotalProductos, lblProductosBajoStock, lblValorInventario;

        public StockForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
            CargarDatos();
        }

        private void InicializarFormulario()
        {
            this.Text = "Control de Stock";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel principal
            var panelPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            panelPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            panelPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            this.Controls.Add(panelPrincipal);

            // Panel superior izquierdo - Lista de stock
            var panelStock = new Panel { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(panelStock, 0, 0);

            var lblTituloStock = new Label
            {
                Text = "INVENTARIO ACTUAL",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelStock.Controls.Add(lblTituloStock);

            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(10, 45),
                Size = new Size(50, 23)
            };
            panelStock.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Location = new Point(70, 42),
                Size = new Size(300, 23)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            panelStock.Controls.Add(txtBuscar);

            dgvStock = new DataGridView
            {
                Location = new Point(10, 75),
                Size = new Size(panelStock.Width - 20, panelStock.Height - 85),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelStock.Controls.Add(dgvStock);

            // Panel superior derecho - Ajuste de stock
            var panelAjuste = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            panelPrincipal.Controls.Add(panelAjuste, 1, 0);

            var lblTituloAjuste = new Label
            {
                Text = "AJUSTE DE STOCK",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelAjuste.Controls.Add(lblTituloAjuste);

            var y = 50;
            var spacing = 35;

            var lblTipoAjuste = new Label { Text = "Tipo:", Location = new Point(10, y), Size = new Size(80, 23) };
            cmbTipoAjuste = new ComboBox 
            { 
                Location = new Point(100, y), 
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTipoAjuste.Items.AddRange(new[] { "Entrada", "Salida", "Ajuste Inventario" });
            cmbTipoAjuste.SelectedIndex = 0;
            panelAjuste.Controls.AddRange(new Control[] { lblTipoAjuste, cmbTipoAjuste });
            y += spacing;

            var lblCantidad = new Label { Text = "Cantidad:", Location = new Point(10, y), Size = new Size(80, 23) };
            txtCantidadAjuste = new NumericUpDown 
            { 
                Location = new Point(100, y), 
                Size = new Size(100, 23),
                Minimum = 1,
                Maximum = 9999,
                Value = 1
            };
            panelAjuste.Controls.AddRange(new Control[] { lblCantidad, txtCantidadAjuste });
            y += spacing + 10;

            btnAjustarStock = new Button 
            { 
                Text = "Ajustar Stock", 
                Location = new Point(10, y), 
                Size = new Size(120, 35),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnAjustarStock.Click += BtnAjustarStock_Click;
            panelAjuste.Controls.Add(btnAjustarStock);
            y += 50;

            // Estadísticas
            var lblEstadisticas = new Label
            {
                Text = "ESTADÍSTICAS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, y),
                Size = new Size(150, 25)
            };
            panelAjuste.Controls.Add(lblEstadisticas);
            y += 30;

            lblTotalProductos = new Label 
            { 
                Text = "Total productos: 0", 
                Location = new Point(10, y), 
                Size = new Size(200, 20),
                Font = new Font("Arial", 9)
            };
            y += 25;

            lblProductosBajoStock = new Label 
            { 
                Text = "Productos bajo stock: 0", 
                Location = new Point(10, y), 
                Size = new Size(200, 20),
                Font = new Font("Arial", 9),
                ForeColor = Color.Red
            };
            y += 25;

            lblValorInventario = new Label 
            { 
                Text = "Valor inventario: $0.00", 
                Location = new Point(10, y), 
                Size = new Size(200, 20),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };

            panelAjuste.Controls.AddRange(new Control[] { lblTotalProductos, lblProductosBajoStock, lblValorInventario });

            // Panel inferior - Productos con bajo stock
            var panelBajoStock = new Panel { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(panelBajoStock, 0, 1);

            var lblTituloBajoStock = new Label
            {
                Text = "PRODUCTOS CON BAJO STOCK",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 10),
                Size = new Size(250, 25)
            };
            panelBajoStock.Controls.Add(lblTituloBajoStock);

            dgvBajoStock = new DataGridView
            {
                Location = new Point(10, 40),
                Size = new Size(panelBajoStock.Width - 20, panelBajoStock.Height - 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelBajoStock.Controls.Add(dgvBajoStock);

            // Panel inferior derecho - Reportes
            var panelReportes = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            panelPrincipal.Controls.Add(panelReportes, 1, 1);

            var lblTituloReportes = new Label
            {
                Text = "REPORTES",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(150, 25)
            };
            panelReportes.Controls.Add(lblTituloReportes);

            btnReporte = new Button 
            { 
                Text = "Generar Reporte\nde Inventario", 
                Location = new Point(10, 50), 
                Size = new Size(120, 50),
                BackColor = Color.Blue,
                ForeColor = Color.White
            };
            btnReporte.Click += BtnReporte_Click;
            panelReportes.Controls.Add(btnReporte);
        }

        private void CargarDatos()
        {
            CargarStock();
            CargarProductosBajoStock();
            ActualizarEstadisticas();
        }

        private void CargarStock()
        {
            var productos = dataManager.Productos.ToList();
            
            dgvStock.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Precio = p.Precio,
                ValorStock = p.Stock * p.Precio,
                Estado = p.TieneBajoStock ? "BAJO STOCK" : "OK"
            }).ToList();

            if (dgvStock.Columns.Count > 0)
            {
                dgvStock.Columns["Id"].Visible = false;
                dgvStock.Columns["Codigo"].HeaderText = "Código";
                dgvStock.Columns["Nombre"].HeaderText = "Producto";
                dgvStock.Columns["Categoria"].HeaderText = "Categoría";
                dgvStock.Columns["Stock"].HeaderText = "Stock";
                dgvStock.Columns["StockMinimo"].HeaderText = "Stock Mín.";
                dgvStock.Columns["Precio"].HeaderText = "Precio";
                dgvStock.Columns["ValorStock"].HeaderText = "Valor Stock";
                dgvStock.Columns["Estado"].HeaderText = "Estado";

                dgvStock.Columns["Precio"].DefaultCellStyle.Format = "C2";
                dgvStock.Columns["ValorStock"].DefaultCellStyle.Format = "C2";

                // Colorear filas con bajo stock
                foreach (DataGridViewRow row in dgvStock.Rows)
                {
                    if (row.Cells["Estado"].Value.ToString() == "BAJO STOCK")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
        }

        private void CargarProductosBajoStock()
        {
            var productosBajoStock = dataManager.ObtenerProductosBajoStock();
            
            dgvBajoStock.DataSource = productosBajoStock.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Diferencia = p.StockMinimo - p.Stock,
                Precio = p.Precio
            }).ToList();

            if (dgvBajoStock.Columns.Count > 0)
            {
                dgvBajoStock.Columns["Id"].Visible = false;
                dgvBajoStock.Columns["Codigo"].HeaderText = "Código";
                dgvBajoStock.Columns["Nombre"].HeaderText = "Producto";
                dgvBajoStock.Columns["Stock"].HeaderText = "Stock";
                dgvBajoStock.Columns["StockMinimo"].HeaderText = "Stock Mín.";
                dgvBajoStock.Columns["Diferencia"].HeaderText = "Necesario";
                dgvBajoStock.Columns["Precio"].HeaderText = "Precio";
                dgvBajoStock.Columns["Precio"].DefaultCellStyle.Format = "C2";

                // Colorear todas las filas en rojo
                foreach (DataGridViewRow row in dgvBajoStock.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void ActualizarEstadisticas()
        {
            var totalProductos = dataManager.Productos.Count;
            var productosBajoStock = dataManager.ObtenerProductosBajoStock().Count;
            var valorInventario = dataManager.Productos.Sum(p => p.Stock * p.Precio);

            lblTotalProductos.Text = $"Total productos: {totalProductos}";
            lblProductosBajoStock.Text = $"Productos bajo stock: {productosBajoStock}";
            lblValorInventario.Text = $"Valor inventario: {valorInventario:C2}";
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var productos = dataManager.BuscarProductos(txtBuscar.Text);
            
            dgvStock.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Precio = p.Precio,
                ValorStock = p.Stock * p.Precio,
                Estado = p.TieneBajoStock ? "BAJO STOCK" : "OK"
            }).ToList();
        }

        private void BtnAjustarStock_Click(object sender, EventArgs e)
        {
            if (dgvStock.SelectedRows.Count > 0)
            {
                var row = dgvStock.SelectedRows[0];
                var productoId = (int)row.Cells["Id"].Value;
                var producto = dataManager.ObtenerProducto(productoId);
                
                if (producto != null)
                {
                    var tipoAjuste = cmbTipoAjuste.SelectedItem.ToString();
                    var cantidad = (int)txtCantidadAjuste.Value;
                    
                    var cantidadAjuste = tipoAjuste == "Salida" ? -cantidad : cantidad;
                    
                    var stockResultante = producto.Stock + cantidadAjuste;
                    
                    if (stockResultante < 0)
                    {
                        MessageBox.Show("No se puede ajustar el stock. El resultado sería negativo.", 
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var resultado = MessageBox.Show(
                        $"¿Confirma el ajuste de stock?\n\n" +
                        $"Producto: {producto.Nombre}\n" +
                        $"Stock actual: {producto.Stock}\n" +
                        $"Tipo ajuste: {tipoAjuste}\n" +
                        $"Cantidad: {cantidad}\n" +
                        $"Stock resultante: {stockResultante}",
                        "Confirmar ajuste",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        dataManager.ActualizarStock(productoId, cantidadAjuste);
                        
                        MessageBox.Show("Stock ajustado exitosamente.", "Éxito", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        CargarDatos();
                        txtCantidadAjuste.Value = 1;
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para ajustar el stock.", 
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnReporte_Click(object sender, EventArgs e)
        {
            var reporte = "REPORTE DE INVENTARIO\n";
            reporte += $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n";
            
            reporte += "RESUMEN:\n";
            reporte += $"Total de productos: {dataManager.Productos.Count}\n";
            reporte += $"Productos con bajo stock: {dataManager.ObtenerProductosBajoStock().Count}\n";
            reporte += $"Valor total del inventario: {dataManager.Productos.Sum(p => p.Stock * p.Precio):C2}\n\n";
            
            reporte += "PRODUCTOS CON BAJO STOCK:\n";
            reporte += "Código\t\tNombre\t\t\tStock\tStock Mín.\n";
            reporte += new string('-', 60) + "\n";
            
            foreach (var producto in dataManager.ObtenerProductosBajoStock())
            {
                reporte += $"{producto.Codigo}\t\t{producto.Nombre.PadRight(20)}\t{producto.Stock}\t{producto.StockMinimo}\n";
            }
            
            reporte += "\n\nINVENTARIO COMPLETO:\n";
            reporte += "Código\t\tNombre\t\t\tStock\tPrecio\t\tValor\n";
            reporte += new string('-', 80) + "\n";
            
            foreach (var producto in dataManager.Productos.OrderBy(p => p.Codigo))
            {
                var valor = producto.Stock * producto.Precio;
                reporte += $"{producto.Codigo}\t\t{producto.Nombre.PadRight(20)}\t{producto.Stock}\t{producto.Precio:C2}\t\t{valor:C2}\n";
            }

            // Mostrar en ventana de texto
            var formReporte = new Form
            {
                Text = "Reporte de Inventario",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var txtReporte = new TextBox
            {
                Text = reporte,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Courier New", 9)
            };

            formReporte.Controls.Add(txtReporte);
            formReporte.ShowDialog();
        }
    }
}