using Microsoft.Data.Sqlite;
using espacioProducto;
namespace espacioProductoRepository;

public class ProductoRepository
{
    private string connectionString = "Data Source=DB/Tienda_final.db;";

    public int Alta(Producto producto)
    {
        int nuevoId = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @prec)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@desc", producto.descripcion);
                command.Parameters.AddWithValue("@prec", producto.precio);
                nuevoId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return nuevoId;
    }

    public List<Producto> GetAll()
    {
        List<Producto> listaProductos = new List<Producto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT idProducto, Descripcion, Precio FROM Productos";

            using (var command = new SqliteCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaProductos.Add(new Producto
                    {

                        idProducto = reader.GetInt32(reader.GetOrdinal("idProducto")),
                        descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        precio = reader.GetDecimal(reader.GetOrdinal("Precio")),

                    });

                }
            }
        }
        return listaProductos;
    }

    public int Baja(int id)
    {
        int resultado;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "DELETE FROM Productos WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                resultado = command.ExecuteNonQuery();
            }

        }
        return resultado;
    }

    public int ModificarProducto(int id, Producto producto)
    {
        int resultado;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "UPDATE Productos SET Descripcion = @desc, Precio = @prec WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                command.Parameters.AddWithValue("@desc", producto.descripcion);
                command.Parameters.AddWithValue("@prec", producto.precio);
                resultado = command.ExecuteNonQuery();
            }
        }
        return resultado;
    }

    public Producto Detalles(int id)
    {
        Producto producto = new Producto();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Productos WHERE idProducto = @identificador";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@identificador", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto.descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                        producto.precio = reader.GetDecimal(reader.GetOrdinal("Precio"));
                    }

                }
            }
        }
        return producto;
    }
}