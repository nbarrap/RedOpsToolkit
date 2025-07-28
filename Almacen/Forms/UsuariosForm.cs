using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class UsuariosForm : Form
    {
        private DataManager dataManager;
        private DataGridView dgvUsuarios;
        private TextBox txtBuscar;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnCerrar;

        public UsuariosForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Usuarios";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 400);

            // Panel superior con controles
            var panelSuperior = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 248, 255)
            };
            this.Controls.Add(panelSuperior);

            // Título
            var lblTitulo = new Label
            {
                Text = "GESTIÓN DE USUARIOS",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 112),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelSuperior.Controls.Add(lblTitulo);

            // Búsqueda
            var lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(400, 20),
                Size = new Size(50, 20)
            };
            panelSuperior.Controls.Add(lblBuscar);

            txtBuscar = new TextBox
            {
                Location = new Point(460, 18),
                Size = new Size(200, 25)
            };
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            panelSuperior.Controls.Add(txtBuscar);

            // Panel de botones
            var panelBotones = new Panel
            {
                Width = 120,
                Dock = DockStyle.Right,
                BackColor = Color.FromArgb(240, 248, 255)
            };
            this.Controls.Add(panelBotones);

            // Botones
            btnAgregar = new Button
            {
                Text = "Agregar",
                Size = new Size(100, 30),
                Location = new Point(10, 20),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAgregar.Click += BtnAgregar_Click;
            panelBotones.Controls.Add(btnAgregar);

            btnEditar = new Button
            {
                Text = "Editar",
                Size = new Size(100, 30),
                Location = new Point(10, 60),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEditar.Click += BtnEditar_Click;
            panelBotones.Controls.Add(btnEditar);

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Size = new Size(100, 30),
                Location = new Point(10, 100),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEliminar.Click += BtnEliminar_Click;
            panelBotones.Controls.Add(btnEliminar);

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(100, 30),
                Location = new Point(10, 140),
                BackColor = Color.FromArgb(128, 128, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.Click += (s, e) => this.Close();
            panelBotones.Controls.Add(btnCerrar);

            // DataGridView
            dgvUsuarios = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Configurar columnas
            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("NombreUsuario", "Usuario");
            dgvUsuarios.Columns.Add("Nombre", "Nombre");
            dgvUsuarios.Columns.Add("Apellido", "Apellido");
            dgvUsuarios.Columns.Add("Email", "Email");
            dgvUsuarios.Columns.Add("Tipo", "Tipo");
            dgvUsuarios.Columns.Add("Activo", "Activo");
            dgvUsuarios.Columns.Add("UltimoAcceso", "Último Acceso");

            dgvUsuarios.Columns["Id"].Width = 50;
            dgvUsuarios.Columns["NombreUsuario"].Width = 100;
            dgvUsuarios.Columns["Nombre"].Width = 100;
            dgvUsuarios.Columns["Apellido"].Width = 100;
            dgvUsuarios.Columns["Email"].Width = 150;
            dgvUsuarios.Columns["Tipo"].Width = 100;
            dgvUsuarios.Columns["Activo"].Width = 60;
            dgvUsuarios.Columns["UltimoAcceso"].Width = 120;

            this.Controls.Add(dgvUsuarios);

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.Rows.Clear();

            var usuarios = string.IsNullOrWhiteSpace(txtBuscar.Text) 
                ? dataManager.Usuarios.ToList()
                : dataManager.BuscarUsuarios(txtBuscar.Text);

            foreach (var usuario in usuarios.OrderBy(u => u.NombreUsuario))
            {
                dgvUsuarios.Rows.Add(
                    usuario.Id,
                    usuario.NombreUsuario,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    usuario.Tipo.ToString(),
                    usuario.Activo ? "Sí" : "No",
                    usuario.UltimoAcceso?.ToString("dd/MM/yyyy HH:mm") ?? "Nunca"
                );
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            var form = new UsuarioForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CargarUsuarios();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["Id"].Value);
                var usuario = dataManager.ObtenerUsuario(id);
                
                if (usuario != null)
                {
                    var form = new UsuarioForm(usuario);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        CargarUsuarios();
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para editar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["Id"].Value);
                var usuario = dataManager.ObtenerUsuario(id);
                
                if (usuario != null)
                {
                    var resultado = MessageBox.Show($"¿Está seguro que desea eliminar el usuario '{usuario.NombreUsuario}'?", 
                        "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (resultado == DialogResult.Yes)
                    {
                        dataManager.EliminarUsuario(id);
                        CargarUsuarios();
                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para eliminar.", "Información", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}