﻿using System;
using MySql.Data.MySqlClient;

namespace BibliotecaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=biblioteca;User ID=root;Password=;";


            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Biblioteca ===");
                Console.WriteLine("1. Crear Usuario");
                Console.WriteLine("2. Actualizar Usuario");
                Console.WriteLine("3. Eliminar Usuario");
                Console.WriteLine("4. Agregar Libro");
                Console.WriteLine("5. Actualizar Libro");
                Console.WriteLine("6. Eliminar Libro");
                Console.WriteLine("7. Agregar Género");
                Console.WriteLine("8. Crear Préstamo");
                Console.WriteLine("9. Listar Usuarios Activos");
                Console.WriteLine("10. Listar Libros Activos");
                Console.WriteLine("11. Devolver Libro");
                Console.WriteLine("12. Salir");
                Console.Write("Seleccione una opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        switch (opcion)
                        {
                            case 1:
                                CrearUsuario(connection);
                                break;
                            case 2:
                                ActualizarUsuario(connection);
                                break;
                            case 3:
                                EliminarUsuario(connection);
                                break;
                            case 4:
                                AgregarLibro(connection);
                                break;
                            case 5:
                                ActualizarLibro(connection);
                                break;
                            case 6:
                                EliminarLibro(connection);
                                break;
                            case 7:
                                AgregarGenero(connection);
                                break;
                            case 8:
                                CrearPrestamo(connection);
                                break;
                            case 9:
                                ListarUsuariosActivos(connection);
                                break;
                            case 10:
                                ListarLibrosActivos(connection);
                                break;
                            case 11:
                                ListarPrestamos(connection);
                                DevolverLibro(connection);
                                break;
                            case 12:
                                Console.WriteLine("Saliendo...");
                                return;
                            default:
                                Console.WriteLine("Opción no válida.");
                                break;
                        }
                        Console.WriteLine("Presiona cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida. Intente de nuevo.");
                    Console.ReadKey();
                }
            }
        }

        static void ListarPrestamos(MySqlConnection connection)
        {
            Console.Clear();

            string query = @"
            SELECT 
                Prestamos.id AS PrestamoID,
                Usuarios.nombre AS UsuarioNombre,
                Usuarios.apellido AS UsuarioApellido,
                Libros.nombre_libro AS LibroTitulo,
                Prestamos.fecha_prestamo,
                Prestamos.fecha_devolucion_estimada,
                Prestamos.fecha_devolucion_real
            FROM 
                Prestamos
            JOIN 
                Usuarios ON Prestamos.usuario_id = Usuarios.id
            JOIN 
                Libros ON Prestamos.libro_id = Libros.id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("=== Lista de Préstamos ===");
                    Console.WriteLine("ID  | Usuario               | Libro                | Fecha Préstamo    | Fecha Devolución Estimada | Fecha Devolución Real");
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------");

                    while (reader.Read())
                    {
                        // Formateo de cada fila para mostrar los datos en columnas
                        Console.WriteLine($"{reader["PrestamoID"],-3} | {reader["UsuarioNombre"]} {reader["UsuarioApellido"],-18} | {reader["LibroTitulo"],-20} | {reader["fecha_prestamo"],-15} | {reader["fecha_devolucion_estimada"],-25} | {reader["fecha_devolucion_real"],-25}");
                    }
                }
            }
            Console.WriteLine();
        }



        static void CrearUsuario(MySqlConnection connection)
        {
            Console.Clear();

            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese el apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Ingrese el DNI: ");
            string dni = Console.ReadLine();

            Console.Write("Ingrese el teléfono: ");
            string telefono = Console.ReadLine();
            Console.Write("Ingrese el email: ");
            string email = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(apellido) && !string.IsNullOrWhiteSpace(dni)
                && !string.IsNullOrWhiteSpace(telefono) && !string.IsNullOrWhiteSpace(email))
            {
                string query = "INSERT INTO Usuarios (nombre, apellido, dni, telefono, email, estado) VALUES (@nombre, @apellido, @dni, @telefono, @email, 1)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);

                    cmd.Parameters.AddWithValue("@apellido", apellido);

                    cmd.Parameters.AddWithValue("@dni", dni);

                    cmd.Parameters.AddWithValue("@telefono", telefono);

                    cmd.Parameters.AddWithValue("@email", email);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(filasAfectadas > 0 ? "Usuario creado exitosamente." : "Error al crear el usuario.");
                }



            }
            else
            {
                Console.WriteLine("Por favor, ingrese todos los datos.");
            }

        }

        static void ActualizarUsuario(MySqlConnection connection)
        {
            Console.Clear();
            ListarUsuariosActivos(connection);

            Console.Write("Ingrese el ID del usuario que desea actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID no válido.");
                return;
            }

            Console.Write("Ingrese el nuevo nombre (deje en blanco para no cambiar): ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el nuevo apellido (deje en blanco para no cambiar): ");
            string apellido = Console.ReadLine();
            Console.Write("Ingrese el nuevo DNI (deje en blanco para no cambiar): ");
            string dni = Console.ReadLine();
            Console.Write("Ingrese el nuevo teléfono (deje en blanco para no cambiar): ");
            string telefono = Console.ReadLine();
            Console.Write("Ingrese el nuevo email (deje en blanco para no cambiar): ");
            string email = Console.ReadLine();

            string query = "UPDATE Usuarios SET ";
            bool first = true;

            if (!string.IsNullOrWhiteSpace(nombre)) query += "nombre = @nombre" + (first ? "" : ", ");
            if (!string.IsNullOrWhiteSpace(apellido)) query += (first ? "" : ", ") + "apellido = @apellido";

            if (!string.IsNullOrWhiteSpace(dni)) query += (first ? "" : ", ") + "dni = @dni";

            if (!string.IsNullOrWhiteSpace(telefono)) query += (first ? "" : ", ") + "telefono = @telefono";
            if (!string.IsNullOrWhiteSpace(email)) query += (first ? "" : ", ") + "email = @email";

            query += " WHERE id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                if (!string.IsNullOrWhiteSpace(nombre)) cmd.Parameters.AddWithValue("@nombre", nombre);
                if (!string.IsNullOrWhiteSpace(apellido)) cmd.Parameters.AddWithValue("@apellido", apellido);

                if (!string.IsNullOrWhiteSpace(dni)) cmd.Parameters.AddWithValue("@dni", dni);
                if (!string.IsNullOrWhiteSpace(telefono)) cmd.Parameters.AddWithValue("@telefono", telefono);
                if (!string.IsNullOrWhiteSpace(email)) cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Usuario actualizado exitosamente." : "No se encontró el usuario con el ID especificado.");
            }
        }

        static void EliminarUsuario(MySqlConnection connection)
        {
            Console.Clear();
            ListarUsuariosActivos(connection);

            Console.Write("Ingrese el ID del usuario a inactivar: ");
            if (!int.TryParse(Console.ReadLine(), out int usuarioId))
            {
                Console.WriteLine("ID no válido.");
                return;
            }

            string updateQuery = "UPDATE Usuarios SET estado = 0 WHERE id = @usuarioId";
            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
            {
                updateCmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                int rowsAffected = updateCmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Usuario inactivado exitosamente." : "No se pudo inactivar el usuario. Verifique el ID.");
            }
        }



        static void AgregarLibro(MySqlConnection connection)
        {
            Console.Clear();

            Console.Write("Ingrese el nombre del libro: ");
            string nombreLibro = Console.ReadLine();
            Console.Write("Ingrese el autor: ");
            string autor = Console.ReadLine();
            Console.Write("Ingrese la fecha de lanzamiento (aaaa-mm-dd): ");
            string fechaLanzamiento = Console.ReadLine();
            Console.Write("Ingrese el ID del género: ");
            ListarGeneros(connection);
            if (!int.TryParse(Console.ReadLine(), out int generoId))
            {
                Console.WriteLine("ID de género no válido.");
                return;
            }

            string query = "INSERT INTO Libros (nombre_libro, autor, fecha_lanzamiento, id_genero, estado) VALUES (@nombreLibro, @autor, @fechaLanzamiento, @generoId, 1)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@nombreLibro", nombreLibro);
                cmd.Parameters.AddWithValue("@autor", autor);
                cmd.Parameters.AddWithValue("@fechaLanzamiento", fechaLanzamiento);
                cmd.Parameters.AddWithValue("@generoId", generoId);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Libro agregado exitosamente." : "Error al agregar el libro.");
            }
        }

        static void ActualizarLibro(MySqlConnection connection)
        {
            Console.Clear();
            ListarLibrosActivos(connection);

            Console.Write("Ingrese el ID del libro que desea actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID no válido.");
                return;
            }

            Console.Write("Ingrese el nuevo nombre del libro (deje en blanco para no cambiar): ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el nuevo autor (deje en blanco para no cambiar): ");
            string autor = Console.ReadLine();
            Console.Write("Ingrese la nueva fecha de lanzamiento (aaaa-mm-dd) (deje en blanco para no cambiar): ");
            string fechaLanzamiento = Console.ReadLine();

            string query = "UPDATE Libros SET ";
            bool first = true;
            if (!string.IsNullOrWhiteSpace(nombre)) query += "nombre_libro = @nombre" + (first ? "" : ", ");
            if (!string.IsNullOrWhiteSpace(autor)) query += (first ? "" : ", ") + "autor = @autor";
            if (!string.IsNullOrWhiteSpace(fechaLanzamiento)) query += (first ? "" : ", ") + "fecha_lanzamiento = @fechaLanzamiento";
            query += " WHERE id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                if (!string.IsNullOrWhiteSpace(nombre)) cmd.Parameters.AddWithValue("@nombre", nombre);
                if (!string.IsNullOrWhiteSpace(autor)) cmd.Parameters.AddWithValue("@autor", autor);
                if (!string.IsNullOrWhiteSpace(fechaLanzamiento)) cmd.Parameters.AddWithValue("@fechaLanzamiento", fechaLanzamiento);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Libro actualizado exitosamente." : "No se encontró el libro con el ID especificado.");
            }
        }

        static void EliminarLibro(MySqlConnection connection)
        {
            Console.Clear();
            ListarLibrosActivos(connection);
            Console.Write("Ingrese el ID del libro a inactivar: ");
            if (!int.TryParse(Console.ReadLine(), out int libroId))
            {
                Console.WriteLine("ID no válido.");
                return;
            }

            string updateQuery = "UPDATE Libros SET estado = 0 WHERE id = @libroId";
            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
            {
                updateCmd.Parameters.AddWithValue("@libroId", libroId);
                int rowsAffected = updateCmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Libro inactivado exitosamente." : "No se pudo inactivar el libro. Verifique el ID.");
            }
        }

        static void AgregarGenero(MySqlConnection connection)
        {
            Console.Clear();
            Console.Write("Ingrese el género: ");
            string genero = Console.ReadLine();

            string query = "INSERT INTO Generos (genero) VALUES (@genero)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@genero", genero);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Género agregado exitosamente." : "Error al agregar el género.");
            }
        }

        static void ListarGeneros(MySqlConnection connection)
        {
            Console.Clear();
            string query = "SELECT * FROM Generos";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("=== Géneros ===");
                    Console.WriteLine("ID  | Género");
                    Console.WriteLine("---------------");

                    while (reader.Read())
                    {
                        // Formateo de cada fila para mostrar los datos en columnas
                        Console.WriteLine($"{reader["id"],-3} | {reader["genero"]}");
                    }
                }
            }
        }


        static void ListarUsuariosActivos(MySqlConnection connection)
        {
            Console.Clear();
            string query = "SELECT * FROM Usuarios WHERE estado = 1";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("=== Usuarios Activos ===");
                    Console.WriteLine("ID  | Nombre Completo       | DNI        | Teléfono       | Email");
                    Console.WriteLine("-----------------------------------------------------------------------");

                    while (reader.Read())
                    {
                        // Formateo de cada fila para mostrar los datos en columnas
                        Console.WriteLine($"{reader["id"],-3} | {reader["nombre"]} {reader["apellido"],-18} | {reader["dni"],-10} | {reader["telefono"],-12} | {reader["email"]}");
                    }
                }
            }
            Console.WriteLine();
        }


        static void ListarLibrosActivos(MySqlConnection connection)
        {
            Console.Clear();
            string query = "SELECT * FROM Libros WHERE estado = 1";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("=== Usuarios Activos ===");
                    Console.WriteLine("ID  | Titulo     | Autor     | Fecha de Lanzamiento     |");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["id"]}   | {reader["nombre_libro"]}     | {reader["autor"]}     | {reader["fecha_lanzamiento"]}");
                    }
                }
            }
        }

        static void CrearPrestamo(MySqlConnection connection)
        {
            Console.Clear();
            Console.Write("Ingrese el ID del usuario: ");
            if (!int.TryParse(Console.ReadLine(), out int usuarioId))
            {
                Console.WriteLine("ID de usuario no válido.");
                return;
            }

            Console.Write("Ingrese el ID del libro: ");
            if (!int.TryParse(Console.ReadLine(), out int libroId))
            {
                Console.WriteLine("ID de libro no válido.");
                return;
            }

            string verificarUsuario = "SELECT estado FROM Usuarios WHERE id = @usuarioId";
            string verificarLibro = "SELECT estado FROM Libros WHERE id = @libroId";

            bool usuarioActivo = false;
            bool libroActivo = false;

            using (MySqlCommand cmdUsuario = new MySqlCommand(verificarUsuario, connection))
            using (MySqlCommand cmdLibro = new MySqlCommand(verificarLibro, connection))
            {
                cmdUsuario.Parameters.AddWithValue("@usuarioId", usuarioId);
                cmdLibro.Parameters.AddWithValue("@libroId", libroId);

                object resultadoUsuario = cmdUsuario.ExecuteScalar();
                object resultadoLibro = cmdLibro.ExecuteScalar();

                if (resultadoUsuario == null)
                {
                    Console.WriteLine("El usuario no existe.");
                    return;
                }
                else
                {
                    usuarioActivo = Convert.ToInt32(resultadoUsuario) == 1;
                }

                if (resultadoLibro == null)
                {
                    Console.WriteLine("El libro no existe.");
                    return;
                }
                else
                {
                    libroActivo = Convert.ToInt32(resultadoLibro) == 1;
                }

                if (!usuarioActivo)
                {
                    Console.WriteLine("El usuario está eliminado y no puede realizar un préstamo.");
                    return;
                }

                if (!libroActivo)
                {
                    Console.WriteLine("El libro está inactivo y no puede ser prestado.");
                    return;
                }
            }

            string fechaPrestamo = DateTime.Now.ToString("aaaa-MM-dd");
            string fechaDevolucionEstimada = DateTime.Now.AddDays(7).ToString("aaaa-MM-dd");

            string query = "INSERT INTO Prestamos (usuario_id, libro_id, fecha_prestamo, fecha_devolucion_estimada) VALUES (@usuarioId, @libroId, @fechaPrestamo, @fechaDevolucionEstimada)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@libroId", libroId);
                cmd.Parameters.AddWithValue("@fechaPrestamo", fechaPrestamo);
                cmd.Parameters.AddWithValue("@fechaDevolucionEstimada", fechaDevolucionEstimada);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Préstamo creado exitosamente." : "Error al crear el préstamo.");
            }
        }

        static void DevolverLibro(MySqlConnection connection)
        {
            Console.Write("Ingrese el ID del préstamo: ");
            if (!int.TryParse(Console.ReadLine(), out int prestamoId))
            {
                Console.WriteLine("ID de préstamo no válido.");
                return;
            }

            string selectQuery = "SELECT fecha_devolucion_estimada, usuario_id FROM Prestamos WHERE id = @prestamoId";
            using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection))
            {
                selectCmd.Parameters.AddWithValue("@prestamoId", prestamoId);
                MySqlDataReader reader = selectCmd.ExecuteReader();

                if (reader.Read())
                {
                    DateTime fechaDevolucionEstimada = reader.GetDateTime("fecha_devolucion_estimada");
                    int usuarioId = reader.GetInt32("usuario_id");
                    reader.Close();

                    Console.Write("Ingrese la fecha de devolución (aaaa-MM-DD): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime fechaDevolucionReal))
                    {
                        Console.WriteLine("Fecha no válida.");
                        return;
                    }

                    int diasRetraso = (fechaDevolucionReal - fechaDevolucionEstimada).Days;

                    string updateQuery = "UPDATE Prestamos SET fecha_devolucion_real = @fechaDevolucionReal WHERE id = @prestamoId";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@fechaDevolucionReal", fechaDevolucionReal);
                        updateCmd.Parameters.AddWithValue("@prestamoId", prestamoId);
                        updateCmd.ExecuteNonQuery();
                    }

                    if (diasRetraso > 0)
                    {
                        Console.WriteLine($"El libro fue devuelto {diasRetraso} días tarde.");

                        string inactivarUsuario = "UPDATE Usuarios SET estado = 0 WHERE id = @usuarioId";
                        using (MySqlCommand cmdInactivar = new MySqlCommand(inactivarUsuario, connection))
                        {
                            cmdInactivar.Parameters.AddWithValue("@usuarioId", usuarioId);
                            cmdInactivar.ExecuteNonQuery();
                            Console.WriteLine("El usuario ha sido inactivado debido a la devolución tardía.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Libro devuelto a tiempo.");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontró el préstamo con ese ID.");
                }
            }
        }
    }
}