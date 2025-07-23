using System;
using System.Drawing;
using System.Windows.Forms;
using Almacen.Forms;
using Almacen.Data;

namespace Almacen
{
    public partial class MainForm : Form
    {
        private DataManager dataManager;

        public MainForm()
        {
            InitializeComponent();
            dataManager = DataManager.Instance;
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            this.Text = "Sistema de Gestión de Almacén";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 400);

            // Crear el panel principal
            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 248, 255)
            };
            this.Controls.Add(panelPrincipal);

            // Título principal
            var lblTitulo = new Label
            {
                Text = "SISTEMA DE GESTIÓN DE ALMACÉN",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 112),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel para los botones
            var panelBotones = new Panel
            {
                Size = new Size(500, 350),
                Location = new Point((this.Width - 500) / 2, 120)
            };
            panelPrincipal.Controls.Add(panelBotones);

            // Crear botones con diseño mejorado
            var btnClientes = CrearBoton("GESTIÓN DE\nCLIENTES", Color.FromArgb(70, 130, 180), 0, 0);
            var btnProductos = CrearBoton("GESTIÓN DE\nPRODUCTOS", Color.FromArgb(34, 139, 34), 250, 0);
            var btnPedidos = CrearBoton("GESTIÓN DE\nPEDIDOS", Color.FromArgb(255, 140, 0), 0, 120);
            var btnVentas = CrearBoton("GESTIÓN DE\nVENTAS", Color.FromArgb(220, 20, 60), 250, 120);
            var btnStock = CrearBoton("CONTROL DE\nSTOCK", Color.FromArgb(138, 43, 226), 125, 240);

            // Asignar eventos
            btnClientes.Click += (s, e) => AbrirFormulario(new ClientesForm());
            btnProductos.Click += (s, e) => AbrirFormulario(new ProductosForm());
            btnPedidos.Click += (s, e) => AbrirFormulario(new PedidosForm());
            btnVentas.Click += (s, e) => AbrirFormulario(new VentasForm());
            btnStock.Click += (s, e) => AbrirFormulario(new StockForm());

            // Agregar botones al panel
            panelBotones.Controls.AddRange(new Control[] { btnClientes, btnProductos, btnPedidos, btnVentas, btnStock });

            // Panel de información
            var panelInfo = new Panel
            {
                Size = new Size(600, 80),
                Location = new Point((this.Width - 600) / 2, this.Height - 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelPrincipal.Controls.Add(panelInfo);

            // Etiquetas de información
            var lblInfo = new Label
            {
                Text = "Información del Sistema",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 5),
                Size = new Size(200, 25)
            };

            var lblEstadisticas = new Label
            {
                Text = ObtenerEstadisticas(),
                Font = new Font("Arial", 10),
                Location = new Point(10, 30),
                Size = new Size(580, 45),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            panelInfo.Controls.AddRange(new Control[] { lblInfo, lblEstadisticas });

            // Actualizar el layout cuando la ventana cambie de tamaño
            this.Resize += (s, e) =>
            {
                panelBotones.Location = new Point((this.Width - 500) / 2, 120);
                panelInfo.Location = new Point((this.Width - 600) / 2, this.Height - 150);
                lblEstadisticas.Text = ObtenerEstadisticas();
            };
        }

        private Button CrearBoton(string texto, Color color, int x, int y)
        {
            var button = new Button
            {
                Text = texto,
                Size = new Size(200, 100),
                Location = new Point(x, y),
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(255, color.R + 30),
                Math.Min(255, color.G + 30),
                Math.Min(255, color.B + 30)
            );

            return button;
        }

        private void AbrirFormulario(Form formulario)
        {
            formulario.ShowDialog();
        }

        private string ObtenerEstadisticas()
        {
            var totalClientes = dataManager.Clientes.Count;
            var totalProductos = dataManager.Productos.Count;
            var productosBajoStock = dataManager.ObtenerProductosBajoStock().Count;
            var ventasHoy = dataManager.ObtenerTotalVentasDelDia(DateTime.Today);

            return $"Clientes: {totalClientes} | Productos: {totalProductos} | " +
                   $"Productos bajo stock: {productosBajoStock} | Ventas hoy: ${ventasHoy:F2}";
        }
    }
}