using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, ingresa un usuario y una contraseña.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Conexion db = new Conexion();
            MySqlConnection con = db.ObtenerConexionAbierta();
            

            if (con.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    string query = "SELECT nombre_completo FROM usuarios WHERE username = @user AND password = @pass";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@user", txtUser.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        string nombreUsuario = resultado.ToString();
                        MessageBox.Show("Bienvenido, " + nombreUsuario, "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        frmPrincipal mainForm = new frmPrincipal();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la autenticación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
