using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        private int idProductoseleccionado = 0;

        public frmProductos()
        {
            InitializeComponent();
        }
        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }
        private void CargarDatos()
        {
            Conexion db = new Conexion();
            MySqlConnection con = db.ObtenerConexionAbierta();
            if (con.State == ConnectionState.Open)
            {
                try
                {
                    string query = "SELECT id_producto, codigo, descripcion, precio, stock FROM productos";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    DataTable tablaproductos = new DataTable();
                    adapter.Fill(tablaproductos);
                    dataGridView1.DataSource = tablaproductos;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
}
