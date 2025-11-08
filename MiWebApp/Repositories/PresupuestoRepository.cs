using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Repositories;

using Models;

public class PresupuestoRepository
{
    string connectionString = "Data Source=DB/Tienda_final.db";

    public bool Alta(Presupuesto presupuesto)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            int idPresupuesto;
            string sql = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@name, @date);SELECT last_insert_rowid();";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@name", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@date", presupuesto.FechaCreacion);

                idPresupuesto = (int)(long)command.ExecuteScalar();

                foreach (var item in presupuesto.PresupuestoDetalle)
                {
                    string queryDetalle = "INSERT INT PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";
                    using (var commandDetail = new SqliteCommand(queryDetalle, connection))
                    {
                        commandDetail.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                        commandDetail.Parameters.AddWithValue("@idProducto", item.Producto.IdProducto);
                        commandDetail.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        commandDetail.ExecuteNonQuery();
                    }
                }
            }
            return idPresupuesto > 0;
        }
    }
    
    public bool AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        Presupuesto presupuesto = GetDetalles(idPresupuesto);

        ProductoRepository productoRepository = new ProductoRepository();
        Producto producto = productoRepository.Detalles(idProducto);

        if (presupuesto != null && producto != null)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPre, @idPro, @cantidad)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idPre", idPresupuesto);
                    command.Parameters.AddWithValue("@idPro", idProducto);
                    command.Parameters.AddWithValue("@cantidad", cantidad);

                    command.ExecuteNonQuery();

                    presupuesto.PresupuestoDetalle.Add(new PresupuestoDetalle(cantidad, producto));

                    return true;
                }
            }
        }
        return false;
    }

    public bool ActualizarPresupuesto(Presupuesto presupuesto)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            int cant;
            connection.Open();

            string sqlDetalle = "UPDATE Presupuestos SET NombreDestinatario = @Nombre, FechaCreacion = @Fecha WHERE idPresupuesto = @id";

            using (var command = new SqliteCommand(sqlDetalle, connection))
            {
                command.Parameters.AddWithValue("@Nombre", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@Fecha", presupuesto.FechaCreacion);
                command.Parameters.AddWithValue("@id", presupuesto.IdPresupuesto);

                cant = command.ExecuteNonQuery();
            }
            return cant > 0;
        }
    }

    public Presupuesto GetDetalles(int id)
    {
        Presupuesto presupuesto = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Presupuestos p INNER JOIN PresupuestosDetalle pd ON p.idPresupuesto = pd.idPresupuesto INNER JOIN Productos pr ON pd.idProducto = pr.idProducto WHERE p.idPresupuesto = @id";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        presupuesto = new Presupuesto
                        {
                            IdPresupuesto = reader.GetInt32(reader.GetOrdinal("IdPresupuesto")),
                            NombreDestinatario = reader.GetString(reader.GetOrdinal("NombreDestinatario")),
                            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            PresupuestoDetalle = new List<PresupuestoDetalle>()
                        };
                    }
                }
            }

            if (presupuesto != null)
            {
                string sqlDetalle = @"SELECT pd.idProducto, pd.Cantidad, p.Descripcion, p.Precio
                                  FROM PresupuestosDetalle pd
                                  JOIN Productos p ON pd.idProducto = p.IdProducto
                                  WHERE pd.idPresupuesto = @id";
                using (var command = new SqliteCommand(sqlDetalle, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presupuesto.PresupuestoDetalle.Add(new PresupuestoDetalle
                            {
                                Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                Producto = new Producto
                                {
                                    IdProducto = reader.GetInt32(reader.GetOrdinal("idProducto")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    Precio = reader.GetDouble(reader.GetOrdinal("Precio"))
                                }
                            });
                        }
                    }
                }
            }

        }

        return presupuesto;
    }

    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> listaPresupuesto = new List<Presupuesto>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";

            var command = new SqliteCommand(sql, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaPresupuesto.Add(new Presupuesto
                    {
                        IdPresupuesto = reader.GetInt32(reader.GetOrdinal("IdPresupuesto")),
                        NombreDestinatario = reader.GetString(reader.GetOrdinal("NombreDestinatario")),
                        FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                    });
                }
            }
        }
        return listaPresupuesto;
    }

    public bool Eliminar(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
        {
            pragma.ExecuteNonQuery();
        }

        using var transaction = connection.BeginTransaction();
        try
        {
            string sqlDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            using (var cmdDetalle = new SqliteCommand(sqlDetalle, connection, transaction))
            {
                cmdDetalle.Parameters.AddWithValue("@id", id);
                cmdDetalle.ExecuteNonQuery();
            }

            string sqlCabecera = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            using (var cmdCabecera = new SqliteCommand(sqlCabecera, connection, transaction))
            {
                cmdCabecera.Parameters.AddWithValue("@id", id);
                int filasAfectadas = cmdCabecera.ExecuteNonQuery();

                transaction.Commit();

                return filasAfectadas == 1;
            }
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

}