using System;
using System.Data.SqlClient;

namespace ProyectoFacturas
{
    public class Program
    {
       public static string connString = @"Data Source=DESKTOP-4KMBN5C\SQLEXPRESS;Initial Catalog=SistemaFacturas;Integrated Security=True";

        public static void Main(string[] args)
        {
            // Menú
            Console.WriteLine("Sistema Facturas");
            while (true)
            {
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Registrar Cliente");
                Console.WriteLine("2. Modificar Cliente");
                Console.WriteLine("3. Eliminar Cliente");
                Console.WriteLine("4. Buscar Cliente");
                Console.WriteLine("5. Registrar Factura");
                Console.WriteLine("6. Modificar Factura");
                Console.WriteLine("7. Eliminar Factura");
                Console.WriteLine("8. Buscar Factura");
                Console.WriteLine("9. Salir");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": RegistrarCliente(ObtenerDatosCliente()); break;
                    case "2": ModificarCliente(ObtenerDatosClienteParaModificar()); break;
                    case "3": EliminarCliente(ObtenerIdCliente()); break;
                    case "4": BuscarCliente(ObtenerIdCliente()); break;
                    case "5": RegistrarFactura(ObtenerDatosFactura()); break;
                    case "6": ModificarFactura(ObtenerDatosFacturaParaModificar()); break;
                    case "7": EliminarFactura(ObtenerIdFactura()); break;
                    case "8": BuscarFactura(ObtenerIdFactura()); break;
                    case "9": return;
                    default: Console.WriteLine("Opción no válida, intente de nuevo."); break;
                }
            }
        }

        // Métodos para obtener datos

        public static (string nombre, string correo, string telefono) ObtenerDatosCliente()
        {
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el correo: ");
            string correo = Console.ReadLine();
            Console.Write("Ingrese el teléfono: ");
            string telefono = Console.ReadLine();
            return (nombre, correo, telefono);
        }

        public static (int id, string nombre, string correo, string telefono) ObtenerDatosClienteParaModificar()
        {
            Console.Write("Ingrese el ID del cliente que desea modificar: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el nuevo nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el nuevo correo: ");
            string correo = Console.ReadLine();
            Console.Write("Ingrese el nuevo teléfono: ");
            string telefono = Console.ReadLine();
            return (id, nombre, correo, telefono);
        }

        public static int ObtenerIdCliente()
        {
            Console.Write("Ingrese el ID del cliente: ");
            return int.Parse(Console.ReadLine());
        }

        public static (int clienteID, string numeroFactura, string fecha, decimal monto) ObtenerDatosFactura()
        {
            Console.Write("Ingrese el ID del cliente: ");
            int clienteID = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el número de factura electrónica: ");
            string numeroFactura = Console.ReadLine();
            Console.Write("Ingrese la fecha de emisión (YYYY-MM-DD): ");
            string fecha = Console.ReadLine();
            Console.Write("Ingrese el monto total: ");
            decimal monto = decimal.Parse(Console.ReadLine());
            return (clienteID, numeroFactura, fecha, monto);
        }

        public static (int idFactura, string estado) ObtenerDatosFacturaParaModificar()
        {
            Console.Write("Ingrese el ID de la factura que desea modificar: ");
            int idFactura = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el estado de la factura (Pendiente/Pagada/Cancelada): ");
            string estado = Console.ReadLine();
            return (idFactura, estado);
        }

        public static int ObtenerIdFactura()
        {
            Console.Write("Ingrese el ID de la factura: ");
            return int.Parse(Console.ReadLine());
        }

        // Métodos para operaciones en la base de datos

        //  Clientes

        public static void RegistrarCliente((string nombre, string correo, string telefono) datosCliente)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO Clientes (Nombre, Correo, Telefono) VALUES (@Nombre, @Correo, @Telefono)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", datosCliente.nombre);
                    cmd.Parameters.AddWithValue("@Correo", datosCliente.correo);
                    cmd.Parameters.AddWithValue("@Telefono", datosCliente.telefono);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Cliente registrado con éxito.");
                }
            }
        }

        public static void ModificarCliente((int id, string nombre, string correo, string telefono) datosCliente)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Clientes SET Nombre=@Nombre, Correo=@Correo, Telefono=@Telefono WHERE ID_Cliente=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", datosCliente.id);
                    cmd.Parameters.AddWithValue("@Nombre", datosCliente.nombre);
                    cmd.Parameters.AddWithValue("@Correo", datosCliente.correo);
                    cmd.Parameters.AddWithValue("@Telefono", datosCliente.telefono);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Cliente modificado con éxito." : "No se encontró el cliente.");
                }
            }
        }

        public static void EliminarCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Clientes WHERE ID_Cliente=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Cliente eliminado con éxito." : "No se encontró el cliente.");
                }
            }
        }

        public static void BuscarCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM Clientes WHERE ID_Cliente=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["ID_Cliente"]}, Nombre: {reader["Nombre"]}, Correo: {reader["Correo"]}, Teléfono: {reader["Telefono"]}");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el cliente.");
                        }
                    }
                }
            }
        }

        // Facturas

        public static void RegistrarFactura((int clienteID, string numeroFactura, string fecha, decimal monto) datosFactura)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Verificar si el ClienteID existe
                string checkQuery = "SELECT COUNT(*) FROM Clientes WHERE ID_Cliente = @ClienteID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ClienteID", datosFactura.clienteID);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.WriteLine("Error: El ID del cliente no existe. No se puede registrar la factura.");
                        return;
                    }
                }

                // Insertar la factura si el ClienteID es válido
                string query = "INSERT INTO Facturas (ClienteID, NumeroFacturaElectronica, FechaEmision, MontoTotal) VALUES (@ClienteID, @NumeroFactura, @Fecha, @Monto)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", datosFactura.clienteID);
                    cmd.Parameters.AddWithValue("@NumeroFactura", datosFactura.numeroFactura);
                    cmd.Parameters.AddWithValue("@Fecha", datosFactura.fecha);
                    cmd.Parameters.AddWithValue("@Monto", datosFactura.monto);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Factura registrada con éxito.");
                }
            }
        }


        public static void ModificarFactura((int idFactura, string estado) datosFactura)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Facturas SET Estado=@Estado WHERE ID_Factura=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", datosFactura.idFactura);
                    cmd.Parameters.AddWithValue("@Estado", datosFactura.estado);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Factura modificada con éxito." : "No se encontró la factura.");
                }
            }
        }

        public static void EliminarFactura(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Facturas WHERE ID_Factura=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Factura eliminada con éxito." : "No se encontró la factura.");
                }
            }
        }

        public static void BuscarFactura(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM Facturas WHERE ID_Factura=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["ID_Factura"]}, ClienteID: {reader["ClienteID"]}, Número: {reader["NumeroFacturaElectronica"]}, Fecha: {reader["FechaEmision"]}, Monto: {reader["MontoTotal"]}, Estado: {reader["Estado"]}");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró la factura.");
                        }
                    }
                }
            }
        }
    }
}