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
            Cargardatos();

        }
        private void Cargardatos()
        {
            Conexion db = new Conexion();
            MySqlConnection con = db.ObtenerConexionAbierta();
            if (con != null && con.State == ConnectionState.Open)
            {
                try
                {
                    string query = "SELECT producto_id, codigo, descripcion, precio, stock FROM productos";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    DataTable tablaproductos = new DataTable();
                    adapter.Fill(tablaproductos);
                    dgvProductos.DataSource = tablaproductos;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar el catálogo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

            }
        }
        //nuevo
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            decimal preciovalido;
            int stockvalido;

            if(!decimal.TryParse(txtPrecio.Text, out preciovalido))
            {
                MessageBox.Show("Ingresa un precio valido", "Formato invalido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtStock.Text, out stockvalido))
            {
                MessageBox.Show("Ingresa un stock valido", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            Conexion db = new Conexion();
            MySqlConnection con = db.ObtenerConexionAbierta();

            if (con != null && con.State == ConnectionState.Open)
            {
                try
                {
                    string query = "SELECT producto_id, codigo, descripcion, precio, stock FROM productos";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@desc", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@precio", preciovalido);
                    cmd.Parameters.AddWithValue("@stock", stockvalido);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto registrado correctamente", "Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
                Limpiarformulario();
                Cargardatos();
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                string idString = fila.Cells["producto_id"].Value.ToString();
                string codigo = fila.Cells["codigo"].Value.ToString();
                string descripcion = fila.Cells["descripcion"].Value.ToString();
                string precio = fila.Cells["precio"].Value.ToString();
                string stock = fila.Cells["stock"].Value.ToString();
                idProductoseleccionado = Convert.ToInt32(idString);
                txtCodigo.Text = codigo;
                txtNombre.Text = descripcion;    
                txtPrecio.Text = precio;         
                txtStock.Text = stock;           
            }
        }
        //editar
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idProductoseleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal preciovalido;
            int stockvalido;
            if (!decimal.TryParse(txtPrecio.Text, out preciovalido) || !int.TryParse(txtStock.Text, out stockvalido))
            {
                MessageBox.Show("Verifique que los campos numéricos sean válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            Conexion db = new Conexion();
            MySqlConnection con = db.ObtenerConexionAbierta();

            if (con != null && con.State == ConnectionState.Open)
            {
                try
                {
                    string query = "UPDATE productos SET codigo = @codigo, descripcion = @desc, precio = @precio, stock = @stock WHERE producto_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@desc", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@precio", preciovalido);
                    cmd.Parameters.AddWithValue("@stock", stockvalido);
                    cmd.Parameters.AddWithValue("@id", idProductoseleccionado);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Limpiarformulario();
                    Cargardatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }

        }
        //eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProductoseleccionado == 0)
            {
                MessageBox.Show("Seleccione el producto que desea eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            DialogResult confirmacion = MessageBox.Show("Desea eliminar este producto?", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion == DialogResult.Yes)
            {
                Conexion db = new Conexion();
                MySqlConnection con = db.ObtenerConexionAbierta();

                if (con != null && con.State == ConnectionState.Open)
                {
                    try
                    {
                        string query = "DELETE FROM productos WHERE producto_id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", idProductoseleccionado);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("El producto ha sido removido.", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Limpiarformulario();
                        Cargardatos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al intentar eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }

        }
        private void Limpiarformulario()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategorias.Text = "";
            idProductoseleccionado = 0;
        }
    }
}
