using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Almacen.Forms;

namespace Almacen
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Mostrar formulario de login
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Si el login es exitoso, mostrar el formulario principal
                var mainForm = new MainForm(loginForm.UsuarioAutenticado);
                Application.Run(mainForm);
            }
            else
            {
                // Si se cancela el login, cerrar la aplicación
                Application.Exit();
            }
        }
    }
}
