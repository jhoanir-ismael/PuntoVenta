using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Punto
{
    public class Conexion
    {
        
        private string cadenaConexion = "Server=localhost;Database=puntodb;Uid=root;Pwd=;Port=3306;SslMode=Disabled;";

        public MySqlConnection ObtenerConexionAbierta()
        {
            
            string cadenaConexion = "Server=localhost;Database=puntodb;Uid=root;Pwd=;Port=3306;SslMode=Disabled;";
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
                return conexion; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error crítico al conectar a MySQL: " + ex.Message, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; 
            }
        }
    }
}

