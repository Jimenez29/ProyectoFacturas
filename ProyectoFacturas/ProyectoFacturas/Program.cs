using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;

namespace ProyectoFacturas
{
    class Program
    {
        static string connString = @"Data Source=DESKTOP-4KMBN5C\SQLEXPRESS;Initial Catalog=SistemaFacturas;Integrated Security=True";

        static void Main(string[] args)


        {
            // Menu
            Console.WriteLine("Sistema Facturas" );
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
                    case "1": RegistrarCliente(); break;
                    case "2": ModificarCliente(); break;
                    case "3": EliminarCliente(); break;
                    case "4": BuscarCliente(); break;
                    case "5": RegistrarFactura(); break;
                    case "6": ModificarFactura(); break;
                    case "7": EliminarFactura(); break;
                    case "8": BuscarFactura(); break;
                    case "9": return;
                    default: Console.WriteLine("Opción no válida, intente de nuevo."); break;
                }
            }
        }

        //  Clientes

        static void RegistrarCliente()
        {
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el correo: ");
            string correo = Console.ReadLine();
            Console.Write("Ingrese el telefono: ");
            string telefono = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO Clientes (Nombre, Correo, Telefono) VALUES (@Nombre, @Correo, @Telefono)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Cliente registrado con exito.");
                }
            }
        }

        static void ModificarCliente()
        {
            Console.Write("Ingrese ID del cliente que desea modificar: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el correo: ");
            string correo = Console.ReadLine();
            Console.Write("Ingrese telefono: ");
            string telefono = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Clientes SET Nombre=@Nombre, Correo=@Correo, Telefono=@Telefono WHERE ID_Cliente=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Cliente modificado con exito." : "No se encontro el cliente.");
                }
            }
        }

        static void EliminarCliente()
        {
            Console.Write("Ingrese ID del cliente que desea eliminar: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Clientes WHERE ID_Cliente=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Cliente eliminado con exito." : "No se encontro el cliente.");
                }
            }
        }

        static void BuscarCliente()
        {
            Console.Write("Ingrese ID del cliente que desea buscar: ");
            int id = int.Parse(Console.ReadLine());

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
                            Console.WriteLine("No se encontro el cliente.");
                        }
                    }
                }
            }
        }

        //  Facturas 

        static void RegistrarFactura()
        {
            Console.Write("Ingrese ID del cliente: ");
            int clienteID = int.Parse(Console.ReadLine());
            Console.Write("Ingrese numero de factura electronica: ");
            string numeroFactura = Console.ReadLine();
            Console.Write("Ingrese la fecha de emision en formato (YYYY-MM-DD): ");
            string fecha = Console.ReadLine();
            Console.Write("Ingrese monto total: ");
            decimal monto = decimal.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "INSERT INTO Facturas (ClienteID, NumeroFacturaElectronica, FechaEmision, MontoTotal) VALUES (@ClienteID, @NumeroFactura, @Fecha, @Monto)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@Monto", monto);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Factura registrada con exito.");
                }
            }
        }

        static void ModificarFactura()
        {
            Console.Write("Ingrese ID de la factura que desea modificar: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el estado actual solo se permiten los siguinetes (Pendiente/Pagada/Cancelada): ");
            string estado = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Facturas SET Estado=@Estado WHERE ID_Factura=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Factura modificada con exito." : "No se encontro la factura.");
                }
            }
        }

        static void EliminarFactura()
        {
            Console.Write("Ingrese ID de la factura que desea eliminar: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Facturas WHERE ID_Factura=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Factura eliminada con exito." : "No se encontro la factura.");
                }
            }
        }

        static void BuscarFactura()
        {
            Console.Write("Ingrese ID de la factura que desea buscar: ");
            int id = int.Parse(Console.ReadLine());

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
                            Console.WriteLine("No se encontro la factura.");
                        }
                    }
                }
            }
        }
    }
}