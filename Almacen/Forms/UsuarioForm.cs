using System;
using System.Drawing;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class UsuarioForm : Form
    {
        private DataManager dataManager;
        private Usuario usuario;
        private bool esEdicion;

        private TextBox txtNombreUsuario;
        private TextBox txtContraseña;
        private TextBox txtConfirmarContraseña;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private ComboBox cmbTipo;
        private CheckBox chkActivo;
        private Button btnGuardar;
        private Button btnCancelar;

        public UsuarioForm(Usuario usuarioExistente = null)
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            usuario = usuarioExistente ?? new Usuario();
            esEdicion = usuarioExistente != null;
            
            if (esEdicion)
            {
                CargarDatosUsuario();
            }
        }

        private void InitializeComponent()
        {
            this.Text = esEdicion ? "Editar Usuario" : "Nuevo Usuario";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Panel principal
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(240, 248, 255)
            };
            this.Controls.Add(panel);

            // Título
            var lblTitulo = new Label
            {
                Text = esEdicion ? "EDITAR USUARIO" : "NUEVO USUARIO",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 112),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panel.Controls.Add(lblTitulo);

            // Nombre de Usuario
            var lblNombreUsuario = new Label
            {
                Text = "Nombre de Usuario:",
                Location = new Point(20, 70),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblNombreUsuario);

            txtNombreUsuario = new TextBox
            {
                Location = new Point(150, 68),
                Size = new Size(250, 25)
            };
            panel.Controls.Add(txtNombreUsuario);

            // Contraseña
            var lblContraseña = new Label
            {
                Text = "Contraseña:",
                Location = new Point(20, 105),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblContraseña);

            txtContraseña = new TextBox
            {
                Location = new Point(150, 103),
                Size = new Size(250, 25),
                PasswordChar = '*'
            };
            panel.Controls.Add(txtContraseña);

            // Confirmar Contraseña
            var lblConfirmarContraseña = new Label
            {
                Text = "Confirmar Contraseña:",
                Location = new Point(20, 140),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblConfirmarContraseña);

            txtConfirmarContraseña = new TextBox
            {
                Location = new Point(150, 138),
                Size = new Size(250, 25),
                PasswordChar = '*'
            };
            panel.Controls.Add(txtConfirmarContraseña);

            // Nombre
            var lblNombre = new Label
            {
                Text = "Nombre:",
                Location = new Point(20, 175),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblNombre);

            txtNombre = new TextBox
            {
                Location = new Point(150, 173),
                Size = new Size(250, 25)
            };
            panel.Controls.Add(txtNombre);

            // Apellido
            var lblApellido = new Label
            {
                Text = "Apellido:",
                Location = new Point(20, 210),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblApellido);

            txtApellido = new TextBox
            {
                Location = new Point(150, 208),
                Size = new Size(250, 25)
            };
            panel.Controls.Add(txtApellido);

            // Email
            var lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 245),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Location = new Point(150, 243),
                Size = new Size(250, 25)
            };
            panel.Controls.Add(txtEmail);

            // Tipo de Usuario
            var lblTipo = new Label
            {
                Text = "Tipo de Usuario:",
                Location = new Point(20, 280),
                Size = new Size(120, 20)
            };
            panel.Controls.Add(lblTipo);

            cmbTipo = new ComboBox
            {
                Location = new Point(150, 278),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTipo.Items.AddRange(new object[] { 
                TipoUsuario.Administrador, 
                TipoUsuario.Empleado, 
                TipoUsuario.Vendedor 
            });
            panel.Controls.Add(cmbTipo);

            // Activo
            chkActivo = new CheckBox
            {
                Text = "Usuario Activo",
                Location = new Point(150, 315),
                Size = new Size(120, 20),
                Checked = true
            };
            panel.Controls.Add(chkActivo);

            // Botones
            btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(100, 30),
                Location = new Point(200, 350),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.Click += BtnGuardar_Click;
            panel.Controls.Add(btnGuardar);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(100, 30),
                Location = new Point(310, 350),
                BackColor = Color.FromArgb(178, 34, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();
            panel.Controls.Add(btnCancelar);

            // Configurar teclas
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void CargarDatosUsuario()
        {
            txtNombreUsuario.Text = usuario.NombreUsuario;
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            cmbTipo.SelectedItem = usuario.Tipo;
            chkActivo.Checked = usuario.Activo;
            
            // En edición, no requerir contraseña si no se cambia
            // Note: PlaceholderText not available in older .NET versions
            // txtContraseña.PlaceholderText = "Dejar vacío para mantener actual";
            // txtConfirmarContraseña.PlaceholderText = "Dejar vacío para mantener actual";
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                // Asignar valores
                usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.Email = txtEmail.Text.Trim();
                usuario.Tipo = (TipoUsuario)cmbTipo.SelectedItem;
                usuario.Activo = chkActivo.Checked;

                // Solo cambiar contraseña si se proporcionó una nueva
                if (!string.IsNullOrWhiteSpace(txtContraseña.Text))
                {
                    usuario.Contraseña = txtContraseña.Text;
                }

                if (esEdicion)
                {
                    dataManager.ActualizarUsuario(usuario);
                    MessageBox.Show("Usuario actualizado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dataManager.AgregarUsuario(usuario);
                    MessageBox.Show("Usuario creado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            // Validar nombre de usuario
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreUsuario.Focus();
                return false;
            }

            // Verificar si el nombre de usuario ya existe
            if (dataManager.ExisteNombreUsuario(txtNombreUsuario.Text.Trim(), esEdicion ? usuario.Id : (int?)null))
            {
                MessageBox.Show("El nombre de usuario ya existe.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreUsuario.Focus();
                return false;
            }

            // Validar contraseña (solo para usuarios nuevos o si se está cambiando)
            if (!esEdicion || !string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                if (string.IsNullOrWhiteSpace(txtContraseña.Text))
                {
                    MessageBox.Show("La contraseña es obligatoria.", "Error de Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraseña.Focus();
                    return false;
                }

                if (txtContraseña.Text.Length < 6)
                {
                    MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error de Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraseña.Focus();
                    return false;
                }

                if (txtContraseña.Text != txtConfirmarContraseña.Text)
                {
                    MessageBox.Show("Las contraseñas no coinciden.", "Error de Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarContraseña.Focus();
                    return false;
                }
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            // Validar email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("El email es obligatorio.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Ingrese un email válido.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validar tipo
            if (cmbTipo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un tipo de usuario.", "Error de Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTipo.Focus();
                return false;
            }

            return true;
        }
    }
}