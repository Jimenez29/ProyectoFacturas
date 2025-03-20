using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace PruebasProyecto
{

    [TestClass]
    public sealed class UnitTestTest1
    {
        public static readonly string connString = @"Data Source=DESKTOP-4KMBN5C\SQLEXPRESS;Initial Catalog=SistemaFacturas;Integrated Security=True";


        

        //--------------------------------------------------------------------

        // Test para probar la Conexion
        [TestMethod]
        public void TestConexion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT 1", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        Assert.AreEqual(1, Convert.ToInt32(result), "La consulta SELECT 1 no retornó el valor esperado.");
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Error al probar la conexión: " + ex.ToString());
            }
        }


        //--------------------------------------------------------------------
        
        //Prueba1: Funcionalidad de inserción de las tablas:
        //Test Registro Factura
        [TestMethod]
        public void TestRegistroFactura()
        {
            int clienteID = 7;
            string numeroFactura = "35601";
            string fecha = "2025-12-12";
            decimal monto = 2500;

            ProyectoFacturas.Program.RegistrarFactura((clienteID, numeroFactura, fecha, monto));

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT TOP 1 * FROM Facturas WHERE NumeroFacturaElectronica = @numeroFactura ORDER BY ID_Factura DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numeroFactura", numeroFactura);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Assert.IsTrue(reader.Read(), "No se encontró ninguna factura en la base de datos.");
                        Assert.AreEqual(clienteID, (int)reader["ClienteID"], "El ClienteID no coincide.");
                        Assert.AreEqual(numeroFactura, reader["NumeroFacturaElectronica"].ToString(), "El Número de Factura no coincide.");
                        Assert.AreEqual(DateTime.Parse(fecha), (DateTime)reader["FechaEmision"], "La Fecha de Emisión no coincide.");
                        Assert.AreEqual(monto, (decimal)reader["MontoTotal"], "El Monto Total no coincide.");
                        Assert.AreEqual("pendiente", reader["Estado"].ToString().ToLower(), "El Estado de la factura no es 'pendiente'.");
                    }
                }
            }
        }
        //--------------------------------------------------------------------

        //Test Resgistro cliente
        [TestMethod]
        public void TestRegistroCliente()
        {
            string nombre = "Manuel";
            string telefono = "98990088";
            string correo = "manuelj23@gmail.com";

            ProyectoFacturas.Program.RegistrarCliente((nombre, correo, telefono));

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT TOP 1 * FROM Clientes WHERE Correo = @correo ORDER BY ID_Cliente DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Assert.IsTrue(reader.Read(), "No se encontró ningún cliente en la base de datos.");
                        Assert.AreEqual(nombre, reader["Nombre"].ToString(), "El Nombre no coincide.");
                        Assert.AreEqual(telefono, reader["Telefono"].ToString(), "El Teléfono no coincide.");
                        Assert.AreEqual(correo, reader["Correo"].ToString(), "El Correo no coincide.");
                    }
                }
            }
        }


        //--------------------------------------------------------------------

        //Prueba3:Rendimiento
        //Test Registro de 100 datos
        [TestMethod]
        public void TestCienDatos()
        {
            int clienteID = 7;
            string fecha = "2025-12-12";
            decimal monto = 2500;
            int cantidadFacturas = 100;

            for (int i = 0; i < cantidadFacturas; i++)
            {
                string numeroFactura = (35601 + i).ToString(); 

                ProyectoFacturas.Program.RegistrarFactura((clienteID, numeroFactura, fecha, monto));
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Facturas WHERE NumeroFacturaElectronica BETWEEN @inicio AND @fin";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@inicio", 35601);
                    cmd.Parameters.AddWithValue("@fin", 35600 + cantidadFacturas);

                    int cantidadInsertada = (int)cmd.ExecuteScalar();
                    Assert.AreEqual(cantidadFacturas, cantidadInsertada, "No se insertaron correctamente las 100 facturas.");
                }
            }
        }
        //--------------------------------------------------------------------

        //Prueba4:Integridad de datos:
        //Test Monto Invalido

        [TestMethod]
        public void TestMontoMalo()
        {
            int clienteID = 7;
            string numeroFactura = "40001";
            string fecha = "2025-12-12";
            string montoInvalido = "2000o"; 

            try
            {
                ProyectoFacturas.Program.RegistrarFactura((clienteID, numeroFactura, fecha, Convert.ToDecimal(montoInvalido)));
                Assert.Fail("Seregistro el monto invalido.");
            }
            catch (FormatException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Fallo: " + ex.Message);
            }
        }
        //--------------------------------------------------------------------

        //Prueba5:Correcta eliminación y modificación de datos:
        //Test Modificar

        [TestMethod]
        public void TestModificarFactura()
        {
            int facturaId = 9; 

            ProyectoFacturas.Program.ModificarFactura((facturaId, "cancelado"));

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string selectQuery = "SELECT Estado FROM Facturas WHERE ID_Factura = @Id";
                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", facturaId);
                    string estado = cmd.ExecuteScalar().ToString();
                    Assert.AreEqual("cancelado", estado, "El estado de la factura no fue modificado correctamente.");
                }
            }
        }
        //--------------------------------------------------------------------
        //Test Eliminar

        [TestMethod]
        public void TestEliminarFactura()
        {
            int facturaId = 9; 

          
            ProyectoFacturas.Program.EliminarFactura(facturaId);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string deleteCheckQuery = "SELECT COUNT(*) FROM Facturas WHERE ID_Factura = @Id";
                using (SqlCommand cmd = new SqlCommand(deleteCheckQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", facturaId);
                    int count = (int)cmd.ExecuteScalar();
                    Assert.AreEqual(0, count, "La factura no fue eliminada correctamente.");
                }
            }
        }


    }
}

//--------------------------------------------------------------------
