using System;
using System.Drawing;
using System.Windows.Forms;
using Almacen.Data;
using Almacen.Models;

namespace Almacen.Forms
{
    public partial class LoginForm : Form
    {
        private DataManager dataManager;
        private TextBox txtUsuario;
        private TextBox txtContraseña;
        private Button btnLogin;
        private Button btnCancelar;
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblContraseña;
        private Panel panelLogin;

        public Usuario UsuarioAutenticado { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
        }

        private void InitializeComponent()
        {
            this.Text = "Iniciar Sesión - Sistema de Almacén";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Panel principal
            panelLogin = new Panel
            {
                Size = new Size(350, 220),
                Location = new Point(25, 30),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(panelLogin);

            // Título
            lblTitulo = new Label
            {
                Text = "INICIAR SESIÓN",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 112),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 20),
                Size = new Size(250, 30)
            };
            panelLogin.Controls.Add(lblTitulo);

            // Label Usuario
            lblUsuario = new Label
            {
                Text = "Usuario:",
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(30, 70),
                Size = new Size(80, 20)
            };
            panelLogin.Controls.Add(lblUsuario);

            // TextBox Usuario
            txtUsuario = new TextBox
            {
                Location = new Point(30, 95),
                Size = new Size(290, 25),
                Font = new Font("Arial", 10)
            };
            panelLogin.Controls.Add(txtUsuario);

            // Label Contraseña
            lblContraseña = new Label
            {
                Text = "Contraseña:",
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(30, 130),
                Size = new Size(80, 20)
            };
            panelLogin.Controls.Add(lblContraseña);

            // TextBox Contraseña
            txtContraseña = new TextBox
            {
                Location = new Point(30, 155),
                Size = new Size(290, 25),
                Font = new Font("Arial", 10),
                PasswordChar = '*'
            };
            panelLogin.Controls.Add(txtContraseña);

            // Botón Login
            btnLogin = new Button
            {
                Text = "Iniciar Sesión",
                Location = new Point(30, 190),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;
            panelLogin.Controls.Add(btnLogin);

            // Botón Cancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(200, 190),
                Size = new Size(120, 25),
                BackColor = Color.FromArgb(178, 34, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += BtnCancelar_Click;
            panelLogin.Controls.Add(btnCancelar);

            // Configurar tecla Enter para login
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancelar;

            // Establecer foco inicial
            txtUsuario.Focus();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor ingrese el nombre de usuario.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show("Por favor ingrese la contraseña.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContraseña.Focus();
                return;
            }

            // Intentar autenticar
            var usuario = dataManager.AutenticarUsuario(txtUsuario.Text, txtContraseña.Text);
            
            if (usuario != null)
            {
                if (!usuario.Activo)
                {
                    MessageBox.Show("El usuario está desactivado. Contacte al administrador.", 
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UsuarioAutenticado = usuario;
                usuario.UltimoAcceso = DateTime.Now;
                dataManager.ActualizarUsuario(usuario);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtUsuario.Focus();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}