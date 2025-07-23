using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class ProductosForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvProductos;
        private TextBox txtBuscar, txtCodigo, txtNombre, txtDescripcion, txtPrecio, txtStock, txtStockMinimo, txtCategoria;
        private Button btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private Producto productoActual;
        private bool modoEdicion = false;

        public ProductosForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
            CargarProductos();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Productos";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel principal
            var panelPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.Controls.Add(panelPrincipal);

            // Panel izquierdo - Lista de productos
            var panelLista = new Panel { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(panelLista, 0, 0);

            // Título y búsqueda
            var lblTitulo = new Label
            {
                Text = "LISTA DE PRODUCTOS",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelLista.Controls.Add(lblTitulo);

            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(10, 45),
                Size = new Size(50, 23)
            };
            panelLista.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Location = new Point(70, 42),
                Size = new Size(300, 23)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            panelLista.Controls.Add(txtBuscar);

            // DataGridView para productos
            dgvProductos = new DataGridView
            {
                Location = new Point(10, 75),
                Size = new Size(panelLista.Width - 20, panelLista.Height - 85),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvProductos.SelectionChanged += DgvProductos_SelectionChanged;
            panelLista.Controls.Add(dgvProductos);

            // Panel derecho - Formulario de producto
            var panelFormulario = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            panelPrincipal.Controls.Add(panelFormulario, 1, 0);

            var lblTituloForm = new Label
            {
                Text = "DATOS DEL PRODUCTO",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(250, 25)
            };
            panelFormulario.Controls.Add(lblTituloForm);

            // Campos del formulario
            var y = 50;
            var spacing = 35;

            // Código
            var lblCodigo = new Label { Text = "Código:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtCodigo = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblCodigo, txtCodigo });
            y += spacing;

            // Nombre
            var lblNombre = new Label { Text = "Nombre:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtNombre = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblNombre, txtNombre });
            y += spacing;

            // Descripción
            var lblDescripcion = new Label { Text = "Descripción:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtDescripcion = new TextBox { Location = new Point(110, y), Size = new Size(200, 23), Multiline = true, Height = 60 };
            panelFormulario.Controls.AddRange(new Control[] { lblDescripcion, txtDescripcion });
            y += 70;

            // Categoría
            var lblCategoria = new Label { Text = "Categoría:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtCategoria = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblCategoria, txtCategoria });
            y += spacing;

            // Precio
            var lblPrecio = new Label { Text = "Precio:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtPrecio = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblPrecio, txtPrecio });
            y += spacing;

            // Stock
            var lblStock = new Label { Text = "Stock:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtStock = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblStock, txtStock });
            y += spacing;

            // Stock Mínimo
            var lblStockMinimo = new Label { Text = "Stock Mínimo:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtStockMinimo = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblStockMinimo, txtStockMinimo });
            y += spacing + 10;

            // Botones
            btnNuevo = new Button { Text = "Nuevo", Location = new Point(10, y), Size = new Size(75, 30) };
            btnGuardar = new Button { Text = "Guardar", Location = new Point(95, y), Size = new Size(75, 30), Enabled = false };
            btnEditar = new Button { Text = "Editar", Location = new Point(180, y), Size = new Size(75, 30), Enabled = false };
            btnEliminar = new Button { Text = "Eliminar", Location = new Point(265, y), Size = new Size(75, 30), Enabled = false };
            y += 40;
            btnCancelar = new Button { Text = "Cancelar", Location = new Point(10, y), Size = new Size(75, 30), Enabled = false };

            // Eventos de botones
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            panelFormulario.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar });
        }

        private void CargarProductos()
        {
            var productos = dataManager.Productos.ToList();
            
            dgvProductos.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Precio = p.Precio,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Estado = p.TieneBajoStock ? "BAJO STOCK" : "OK"
            }).ToList();

            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["Id"].Visible = false;
                dgvProductos.Columns["Codigo"].HeaderText = "Código";
                dgvProductos.Columns["Nombre"].HeaderText = "Nombre";
                dgvProductos.Columns["Categoria"].HeaderText = "Categoría";
                dgvProductos.Columns["Precio"].HeaderText = "Precio";
                dgvProductos.Columns["Stock"].HeaderText = "Stock";
                dgvProductos.Columns["StockMinimo"].HeaderText = "Stock Mín.";
                dgvProductos.Columns["Estado"].HeaderText = "Estado";

                // Formato de precio
                dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                
                // Color para productos con bajo stock
                foreach (DataGridViewRow row in dgvProductos.Rows)
                {
                    if (row.Cells["Estado"].Value.ToString() == "BAJO STOCK")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var productos = dataManager.BuscarProductos(txtBuscar.Text);
            
            dgvProductos.DataSource = productos.Select(p => new
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Categoria = p.Categoria,
                Precio = p.Precio,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                Estado = p.TieneBajoStock ? "BAJO STOCK" : "OK"
            }).ToList();
        }

        private void DgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0 && !modoEdicion)
            {
                var row = dgvProductos.SelectedRows[0];
                var id = (int)row.Cells["Id"].Value;
                productoActual = dataManager.ObtenerProducto(id);
                
                if (productoActual != null)
                {
                    MostrarProducto(productoActual);
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                }
            }
        }

        private void MostrarProducto(Producto producto)
        {
            txtCodigo.Text = producto.Codigo;
            txtNombre.Text = producto.Nombre;
            txtDescripcion.Text = producto.Descripcion;
            txtCategoria.Text = producto.Categoria;
            txtPrecio.Text = producto.Precio.ToString("F2");
            txtStock.Text = producto.Stock.ToString();
            txtStockMinimo.Text = producto.StockMinimo.ToString();
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtCategoria.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtStockMinimo.Clear();
            productoActual = null;
        }

        private void HabilitarEdicion(bool habilitar)
        {
            txtCodigo.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
            txtCategoria.Enabled = habilitar;
            txtPrecio.Enabled = habilitar;
            txtStock.Enabled = habilitar;
            txtStockMinimo.Enabled = habilitar;

            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
            btnNuevo.Enabled = !habilitar;
            btnEditar.Enabled = !habilitar && productoActual != null;
            btnEliminar.Enabled = !habilitar && productoActual != null;

            dgvProductos.Enabled = !habilitar;
            txtBuscar.Enabled = !habilitar;

            modoEdicion = habilitar;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarEdicion(true);
            txtCodigo.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarFormulario())
            {
                var producto = productoActual ?? new Producto();
                producto.Codigo = txtCodigo.Text.Trim();
                producto.Nombre = txtNombre.Text.Trim();
                producto.Descripcion = txtDescripcion.Text.Trim();
                producto.Categoria = txtCategoria.Text.Trim();
                producto.Precio = decimal.Parse(txtPrecio.Text);
                producto.Stock = int.Parse(txtStock.Text);
                producto.StockMinimo = int.Parse(txtStockMinimo.Text);

                if (productoActual == null)
                {
                    dataManager.AgregarProducto(producto);
                    MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataManager.ActualizarProducto(producto);
                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarProductos();
                HabilitarEdicion(false);
                LimpiarFormulario();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (productoActual != null)
            {
                HabilitarEdicion(true);
                txtCodigo.Focus();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (productoActual != null)
            {
                var resultado = MessageBox.Show(
                    $"¿Está seguro de eliminar el producto {productoActual.Nombre}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    dataManager.EliminarProducto(productoActual.Id);
                    MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    LimpiarFormulario();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                }
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            HabilitarEdicion(false);
            if (productoActual != null)
            {
                MostrarProducto(productoActual);
            }
            else
            {
                LimpiarFormulario();
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser un número válido mayor que cero.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("El stock debe ser un número entero mayor o igual a cero.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo) || stockMinimo < 0)
            {
                MessageBox.Show("El stock mínimo debe ser un número entero mayor o igual a cero.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockMinimo.Focus();
                return false;
            }

            // Verificar que el código no exista (solo para productos nuevos)
            if (productoActual == null)
            {
                var codigoExistente = dataManager.Productos.Any(p => p.Codigo.Equals(txtCodigo.Text.Trim(), StringComparison.OrdinalIgnoreCase));
                if (codigoExistente)
                {
                    MessageBox.Show("Ya existe un producto con ese código.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodigo.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}