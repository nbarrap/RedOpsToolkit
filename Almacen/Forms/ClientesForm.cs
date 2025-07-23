using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class ClientesForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvClientes;
        private TextBox txtBuscar, txtNombre, txtApellido, txtEmail, txtTelefono, txtDireccion;
        private Button btnNuevo, btnGuardar, btnEditar, btnEliminar, btnCancelar;
        private Cliente clienteActual;
        private bool modoEdicion = false;

        public ClientesForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
            CargarClientes();
        }

        private void InicializarFormulario()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel principal
            var panelPrincipal = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            panelPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.Controls.Add(panelPrincipal);

            // Panel izquierdo - Lista de clientes
            var panelLista = new Panel { Dock = DockStyle.Fill };
            panelPrincipal.Controls.Add(panelLista, 0, 0);

            // Título y búsqueda
            var lblTitulo = new Label
            {
                Text = "LISTA DE CLIENTES",
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

            // DataGridView para clientes
            dgvClientes = new DataGridView
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
            dgvClientes.SelectionChanged += DgvClientes_SelectionChanged;
            panelLista.Controls.Add(dgvClientes);

            // Panel derecho - Formulario de cliente
            var panelFormulario = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            panelPrincipal.Controls.Add(panelFormulario, 1, 0);

            var lblTituloForm = new Label
            {
                Text = "DATOS DEL CLIENTE",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 25)
            };
            panelFormulario.Controls.Add(lblTituloForm);

            // Campos del formulario
            var y = 50;
            var spacing = 35;

            // Nombre
            var lblNombre = new Label { Text = "Nombre:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtNombre = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblNombre, txtNombre });
            y += spacing;

            // Apellido
            var lblApellido = new Label { Text = "Apellido:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtApellido = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblApellido, txtApellido });
            y += spacing;

            // Email
            var lblEmail = new Label { Text = "Email:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtEmail = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblEmail, txtEmail });
            y += spacing;

            // Teléfono
            var lblTelefono = new Label { Text = "Teléfono:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtTelefono = new TextBox { Location = new Point(110, y), Size = new Size(200, 23) };
            panelFormulario.Controls.AddRange(new Control[] { lblTelefono, txtTelefono });
            y += spacing;

            // Dirección
            var lblDireccion = new Label { Text = "Dirección:", Location = new Point(10, y), Size = new Size(100, 23) };
            txtDireccion = new TextBox { Location = new Point(110, y), Size = new Size(200, 23), Multiline = true, Height = 60 };
            panelFormulario.Controls.AddRange(new Control[] { lblDireccion, txtDireccion });
            y += 70;

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

        private void CargarClientes()
        {
            var clientes = dataManager.Clientes.ToList();
            
            dgvClientes.DataSource = clientes.Select(c => new
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy")
            }).ToList();

            if (dgvClientes.Columns.Count > 0)
            {
                dgvClientes.Columns["Id"].Visible = false;
                dgvClientes.Columns["Nombre"].HeaderText = "Nombre";
                dgvClientes.Columns["Apellido"].HeaderText = "Apellido";
                dgvClientes.Columns["Email"].HeaderText = "Email";
                dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["FechaRegistro"].HeaderText = "Fecha Registro";
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            var clientes = dataManager.BuscarClientes(txtBuscar.Text);
            
            dgvClientes.DataSource = clientes.Select(c => new
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email,
                Telefono = c.Telefono,
                FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy")
            }).ToList();
        }

        private void DgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0 && !modoEdicion)
            {
                var row = dgvClientes.SelectedRows[0];
                var id = (int)row.Cells["Id"].Value;
                clienteActual = dataManager.ObtenerCliente(id);
                
                if (clienteActual != null)
                {
                    MostrarCliente(clienteActual);
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                }
            }
        }

        private void MostrarCliente(Cliente cliente)
        {
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtEmail.Text = cliente.Email;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            clienteActual = null;
        }

        private void HabilitarEdicion(bool habilitar)
        {
            txtNombre.Enabled = habilitar;
            txtApellido.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;

            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
            btnNuevo.Enabled = !habilitar;
            btnEditar.Enabled = !habilitar && clienteActual != null;
            btnEliminar.Enabled = !habilitar && clienteActual != null;

            dgvClientes.Enabled = !habilitar;
            txtBuscar.Enabled = !habilitar;

            modoEdicion = habilitar;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarEdicion(true);
            txtNombre.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarFormulario())
            {
                var cliente = clienteActual ?? new Cliente();
                cliente.Nombre = txtNombre.Text.Trim();
                cliente.Apellido = txtApellido.Text.Trim();
                cliente.Email = txtEmail.Text.Trim();
                cliente.Telefono = txtTelefono.Text.Trim();
                cliente.Direccion = txtDireccion.Text.Trim();

                if (clienteActual == null)
                {
                    dataManager.AgregarCliente(cliente);
                    MessageBox.Show("Cliente agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataManager.ActualizarCliente(cliente);
                    MessageBox.Show("Cliente actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarClientes();
                HabilitarEdicion(false);
                LimpiarFormulario();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (clienteActual != null)
            {
                HabilitarEdicion(true);
                txtNombre.Focus();
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (clienteActual != null)
            {
                var resultado = MessageBox.Show(
                    $"¿Está seguro de eliminar el cliente {clienteActual.NombreCompleto}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    dataManager.EliminarCliente(clienteActual.Id);
                    MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                    LimpiarFormulario();
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                }
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            HabilitarEdicion(false);
            if (clienteActual != null)
            {
                MostrarCliente(clienteActual);
            }
            else
            {
                LimpiarFormulario();
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("El email es obligatorio y debe tener un formato válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }
    }
}