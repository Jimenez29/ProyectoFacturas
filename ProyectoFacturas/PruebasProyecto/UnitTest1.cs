using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace PruebasProyecto

{
    [TestClass]
    public sealed class UnitTestTest1
    {
        private readonly string connString = @"Data Source=DESKTOP-4KMBN5C\SQLEXPRESS;Initial Catalog=SistemaFacturas;Integrated Security=True";

        [TestMethod]
        public void RegistrarFactura_DeberiaGuardarCorrectamente()
        {
            // Datos de prueba
            int clienteID = 7;
            string numeroFactura = "35601";
            string fecha = "2025-12-12";
            decimal monto = 2500;

            // Llamamos al método a probar
            ProyectoFacturas.Program.RegistrarFactura((clienteID, numeroFactura, fecha, monto));

            // Verificamos que la factura fue insertada
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT TOP 1 ID_Factura, ClienteID, NumeroFacturaElectronica, FechaEmision, MontoTotal, Estado FROM Facturas ORDER BY ID_Factura DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Assert.IsTrue(reader.Read(), "No se encontró ninguna factura en la base de datos.");

                    int idFactura = reader.GetInt32(0);
                    int clienteID_db = reader.GetInt32(1);
                    string numeroFactura_db = reader.GetString(2);
                    DateTime fecha_db = reader.GetDateTime(3);
                    decimal monto_db = reader.GetDecimal(4);
                    string estado_db = reader.GetString(5);

                    // Validaciones
                    Assert.AreEqual(clienteID, clienteID_db, "El ClienteID no coincide.");
                    Assert.AreEqual(numeroFactura, numeroFactura_db, "El Número de Factura no coincide.");
                    Assert.AreEqual(DateTime.Parse(fecha), fecha_db, "La Fecha de Emisión no coincide.");
                    Assert.AreEqual(monto, monto_db, "El Monto Total no coincide.");
                    Assert.AreEqual("pendiente", estado_db, "El Estado de la factura no es 'pendiente'.");
                }
            }
        }
    }

}

