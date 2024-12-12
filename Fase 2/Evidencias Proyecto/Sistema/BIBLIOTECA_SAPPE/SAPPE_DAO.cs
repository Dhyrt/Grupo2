using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using BIBLIOTECA_SAPPE;

namespace BIBLIOTECA_SAPPE
{
    public class SAPPE_DAO
    {
        private SQLiteConnection conexion;

        public SAPPE_DAO()
        {
            string connectionString = @"Data Source=C:\Users\carol\Desktop\SAPPE\db.sqlite3;";
            conexion = new SQLiteConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                    Console.WriteLine("Conexión a la base de datos SQLite abierta.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }


        //----------------------------------------------------------------//
        //-----------------------METODOS AGREGAR--------------------------//
        //----------------------------------------------------------------//

        public bool AgregarCategoria(string nombreCategoria)
        {
            string query = "INSERT INTO Categoria (nombre_categoria) VALUES (@nombre_categoria)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_categoria", nombreCategoria);
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Devuelve true si se insertó correctamente
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la categoría: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregaMarca(string nombreMarca)
        {
            string query = "INSERT INTO Marca (nombre_marca) VALUES (@nombre_marca)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_marca", nombreMarca);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la marca: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarDescuento(int diasVencimiento, int porcentajeDescuento)
        {
            string query = "INSERT INTO PoliticaDescuentos (dias_para_vencimiento, descuento) VALUES (@dias_para_vencimiento, @descuento)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@dias_para_vencimiento", diasVencimiento);
                    command.Parameters.AddWithValue("@descuento", porcentajeDescuento);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el descuento: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarTienda(string nombreTienda, string fraseFooter, string direccion, int idComuna, string logo, string color, string colorSecundario, string imgLogin, string imgRegistro)
        {
            string query = "INSERT INTO Tienda (nombre_tienda, frase_footer, direccion, comuna_id, logo, color, color_secundario, img_login, img_registro) " +
                           "VALUES (@nombre_tienda, @frase_footer, @direccion, @id_comuna, @logo, @color, @color_secundario, @img_login, @img_registro)";

            try
            {
                string nombreLogo = Path.GetFileName(logo);
                string rutaDestinoLogo = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreLogo);
                File.Copy(logo, rutaDestinoLogo, true);
                string rutaLogoRelativa = Path.Combine("tienda", nombreLogo);

                string nombreImgLogin = Path.GetFileName(imgLogin);
                string rutaDestinoImgLogin = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreImgLogin);
                File.Copy(imgLogin, rutaDestinoImgLogin, true);
                string rutaImgLoginRelativa = Path.Combine("tienda", nombreImgLogin);

                string nombreImgRegistro = Path.GetFileName(imgRegistro);
                string rutaDestinoImgRegistro = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreImgRegistro);
                File.Copy(imgRegistro, rutaDestinoImgRegistro, true);
                string rutaImgRegistroRelativa = Path.Combine("tienda", nombreImgRegistro);

                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_tienda", nombreTienda);
                    command.Parameters.AddWithValue("@frase_footer", fraseFooter ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@direccion", direccion);
                    command.Parameters.AddWithValue("@id_comuna", idComuna);
                    command.Parameters.AddWithValue("@logo", rutaLogoRelativa);
                    command.Parameters.AddWithValue("@color", color ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@color_secundario", colorSecundario ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@img_login", rutaImgLoginRelativa);
                    command.Parameters.AddWithValue("@img_registro", rutaImgRegistroRelativa);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la tienda: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


        public bool AgregarProveedor(string nombreProveedor, int telefono, string correo, string direccion, int comuna)
        {
            string query = "INSERT INTO Proveedor (nombre_proveedor, telefono, correo, direccion, comuna_id) VALUES (@nombre_proveedor, @telefono, @correo, @direccion, @comuna_id)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_proveedor", nombreProveedor);
                    command.Parameters.AddWithValue("@telefono", telefono);
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@direccion", direccion);
                    command.Parameters.AddWithValue("@comuna_id", comuna);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el proveedor: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarProducto(string nombreProducto, int precio, string descripcion, int volumen, int idCategoria, string imagen, int idMarca, int idPerecible, int idUnidadMedida)
        {
            string query = "INSERT INTO Producto (nombre_producto, precio_unitario, descripcion, volumen, img, categoria_id, marca_id, unidad_medida_id, perecible_id) " +
                   "VALUES (@nombre_producto, @precio_unitario, @descripcion, @volumen, @img, @categoria_id, @marca_id, @unidad_medida_id, @perecible_id)";

            string nombreImagen = Path.GetFileName(imagen);

            // Ruta de destino donde se copiará la imagen en Django
            string rutaDestino = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\productos", nombreImagen);

            try
            {
                File.Copy(imagen, rutaDestino, true);
                string rutaImagenRelativa = Path.Combine("productos", nombreImagen);
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_producto", nombreProducto);
                    command.Parameters.AddWithValue("@precio_unitario", precio);
                    command.Parameters.AddWithValue("@descripcion", descripcion);
                    command.Parameters.AddWithValue("@volumen", volumen);
                    command.Parameters.AddWithValue("@img", rutaImagenRelativa);
                    command.Parameters.AddWithValue("@categoria_id", idCategoria);
                    command.Parameters.AddWithValue("@marca_id", idMarca);
                    command.Parameters.AddWithValue("@unidad_medida_id", idUnidadMedida);
                    command.Parameters.AddWithValue("@perecible_id", idPerecible);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el producto: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarBodega(string nombre, int capacidadMax)
        {
            string query = "INSERT INTO Bodega (nombre, capacidad_maxima) VALUES (@nombre, @capacidad_maxima)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@capacidad_maxima", capacidadMax);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la bodega: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarSeccionBodega(string nombreSeccion) {
            string query = "INSERT INTO Seccion_Bodega (nombre_seccion) VALUES (@nombre_seccion)";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_seccion", nombreSeccion);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar la seccion de la bodega: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public string GenerarIdUnico()
        {
            const int longitud = 7;
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            while (true)
            {
                var idUnico = new string(Enumerable.Repeat(caracteres, longitud)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                if (!IdLoteExiste(idUnico)) // Verifica que el ID no exista
                {
                    return idUnico;
                }
            }
        }

        private bool IdLoteExiste(string idLote)
        {
            const string query = "SELECT COUNT(*) FROM Lotes WHERE id_lote = @id_lote";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_lote", idLote);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool AgregarLote(int cantidad, DateTime? fechaVencimiento, int idProducto, int idProveedor, int idSeccionBodega, int idBodega)
        {
            string idLote = GenerarIdUnico();
            DateTime fechaEntrada = DateTime.Now;
            string query = "INSERT INTO Lotes (id_lote, cantidad, fecha_entrada, fecha_vencimiento, producto_id, proveedor_id, seccion_bodega_id, bodega_id) " +
                           "VALUES (@id_lote, @cantidad, @fecha_entrada, @fecha_vencimiento, @producto_id, @proveedor_id, @seccion_bodega_id, @bodega_id)";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_lote", idLote);
                    command.Parameters.AddWithValue("@cantidad", cantidad);
                    command.Parameters.AddWithValue("@fecha_entrada", fechaEntrada);
                    // Si la fecha de vencimiento es null, pasamos DBNull.Value
                    command.Parameters.AddWithValue("@fecha_vencimiento",
                    fechaVencimiento.HasValue ? (object)fechaVencimiento.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    command.Parameters.AddWithValue("@producto_id", idProducto);
                    command.Parameters.AddWithValue("@proveedor_id", idProveedor);
                    command.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                    command.Parameters.AddWithValue("@bodega_id", idBodega);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        // Verificar si es necesario crear una oferta para el lote
                        if (fechaVencimiento.HasValue)
                        {
                            ActualizarInventario(idProducto, cantidad, idSeccionBodega, idBodega);
                            RegistrarMovimiento(cantidad, idLote, idBodega);
                            CrearOfertaSiAplica(idLote, fechaVencimiento.Value);
                        }
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error al agregar el lote.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el lote: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        private void CrearOfertaSiAplica(string idLote, DateTime fechaVencimiento)
        {
            try
            {
                // Obtener políticas de descuento
                string query = "SELECT id_descuento, dias_para_vencimiento, descuento FROM PoliticaDescuentos";
                using (var command = new SQLiteCommand(query, conexion))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int diasParaVencimiento = reader.GetInt32(1);
                        if ((fechaVencimiento - DateTime.Now).TotalDays <= diasParaVencimiento)
                        {
                            int idDescuento = reader.GetInt32(0);
                            int descuento = reader.GetInt32(2);

                            // Calcular la fecha de inicio (hoy) y la fecha de fin (fecha de vencimiento)
                            DateTime fechaInicio = DateTime.Now;
                            DateTime fechaFin = fechaVencimiento;

                            // Insertar la oferta
                            InsertarOferta(idLote, idDescuento, descuento, fechaInicio, fechaFin);
                            break; // Solo aplicamos la primera política que cumpla
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar y crear la oferta: {ex.Message}");
            }
        }


        private void InsertarOferta(string idLote, int idDescuento, int descuento, DateTime fechaInicio, DateTime fechaFin)
        {
            string query = "INSERT INTO Ofertas (fecha_inicio, fecha_fin, lote_id, descuento_id, estado_oferta_id) " +
                           "VALUES (@fecha_inicio, @fecha_fin, @lote_id, @descuento_id, @estado_oferta_id)";
            try
            {
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@fecha_inicio", fechaInicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@fecha_fin", fechaFin.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@lote_id", idLote);
                    command.Parameters.AddWithValue("@descuento_id", idDescuento);
                    command.Parameters.AddWithValue("@estado_oferta_id", 1); // Estado por defecto (ej. "Activa")

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar la oferta: {ex.Message}");
            }
        }

        private void ActualizarInventario(int idProducto, int cantidad, int idSeccionBodega, int idBodega)
        {
            try
            {
                // Verificar si ya existe un registro en el inventario para este producto, sección y bodega
                string selectQuery = "SELECT cantidad_disponible FROM Inventario WHERE producto_id = @producto_id AND seccion_bodega_id = @seccion_bodega_id AND bodega_id = @bodega_id";
                int cantidadActual = 0;

                using (var selectCommand = new SQLiteCommand(selectQuery, conexion))
                {
                    selectCommand.Parameters.AddWithValue("@producto_id", idProducto);
                    selectCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                    selectCommand.Parameters.AddWithValue("@bodega_id", idBodega);

                    var result = selectCommand.ExecuteScalar();
                    if (result != null)
                    {
                        cantidadActual = Convert.ToInt32(result);
                    }
                }

                // Actualizar o insertar la cantidad en el inventario
                if (cantidadActual > 0)
                {
                    // Actualizar
                    string updateQuery = "UPDATE Inventario SET cantidad_disponible = cantidad_disponible + @cantidad " +
                                         "WHERE producto_id = @producto_id AND seccion_bodega_id = @seccion_bodega_id AND bodega_id = @bodega_id";

                    using (var updateCommand = new SQLiteCommand(updateQuery, conexion))
                    {
                        updateCommand.Parameters.AddWithValue("@cantidad", cantidad);
                        updateCommand.Parameters.AddWithValue("@producto_id", idProducto);
                        updateCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                        updateCommand.Parameters.AddWithValue("@bodega_id", idBodega);

                        updateCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Insertar
                    string insertQuery = "INSERT INTO Inventario (producto_id, cantidad_disponible, seccion_bodega_id, bodega_id) " +
                                         "VALUES (@producto_id, @cantidad, @seccion_bodega_id, @bodega_id)";

                    using (var insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.AddWithValue("@producto_id", idProducto);
                        insertCommand.Parameters.AddWithValue("@cantidad", cantidad);
                        insertCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                        insertCommand.Parameters.AddWithValue("@bodega_id", idBodega);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el inventario: {ex.Message}");
            }
        }

        private void RegistrarMovimiento(int cantidad, string idLote, int idBodega)
        {
            string query = "INSERT INTO Movimiento_Bodega (cantidad, fecha, tipo_movimiento_id, lote_id, bodega_id) " +
                           "VALUES (@cantidad, @fecha, @tipo_movimiento_id, @lote_id, @bodega_id)";

            try
            {
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@cantidad", cantidad);
                    command.Parameters.AddWithValue("@fecha", DateTime.Now);
                    command.Parameters.AddWithValue("@tipo_movimiento_id", 1);
                    command.Parameters.AddWithValue("@lote_id", idLote);
                    command.Parameters.AddWithValue("@bodega_id", idBodega);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el movimiento: {ex.Message}");
            }
        }

        public bool AgregarAnuncio(string titulo, string descripcion, DateTime fechaInicio, DateTime fechaFin, string imagen)
        {
            string query = "INSERT INTO Anuncios (titulo, descripcion, fecha_inicio, fecha_fin, img) " +
                           "VALUES (@titulo, @descripcion, @fecha_inicio, @fecha_fin, @img)";

            string nombreImagen = Path.GetFileName(imagen);

            // Ruta de destino donde se copiará la imagen en Django
            string rutaDestino = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\anuncios", nombreImagen);

            try
            {
                File.Copy(imagen, rutaDestino, true);
                string rutaImagenRelativa = Path.Combine("anuncios", nombreImagen);
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@titulo", titulo);
                    command.Parameters.AddWithValue("@descripcion", descripcion);
                    command.Parameters.AddWithValue("@fecha_inicio", fechaInicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@fecha_fin", fechaFin.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@img", rutaImagenRelativa);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el anuncio: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        //----------------------------------------------------------------//
        //-----------------------METODOS LISTAR---------------------------//
        //----------------------------------------------------------------//

        public List<KeyValuePair<int, string>> CargarCategorias()
        {
            List<KeyValuePair<int, string>> categorias = new List<KeyValuePair<int, string>>();
            string query = "SELECT id_categoria, nombre_categoria FROM Categoria";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idCategoria = Convert.ToInt32(reader["id_categoria"]);
                            string nombreCategoria = reader["nombre_categoria"].ToString();
                            categorias.Add(new KeyValuePair<int, string>(idCategoria, nombreCategoria));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar categorías: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
            return categorias;
        }

        public List<KeyValuePair<int, string>> CargarMarcas()
        {
            List<KeyValuePair<int, string>> marcas = new List<KeyValuePair<int, string>>();
            string query = "SELECT id_marca, nombre_marca FROM Marca";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idMarca = Convert.ToInt32(reader["id_marca"]);
                            string nombreMarca = reader["nombre_marca"].ToString();
                            marcas.Add(new KeyValuePair<int, string>(idMarca, nombreMarca));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las marcas: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
            return marcas;
        }

        public List<KeyValuePair<int, string>> CargarPerecible()
        {
            List<KeyValuePair<int, string>> perecibles = new List<KeyValuePair<int, string>>();
            string query = "SELECT id_perecibilidad, perecibilidad FROM PerecibilidadProducto";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPerecible = Convert.ToInt32(reader["id_perecibilidad"]);
                            string perecible = reader["perecibilidad"].ToString();
                            perecibles.Add(new KeyValuePair<int, string>(idPerecible, perecible));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las perecibilidades: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
            return perecibles;
        }

        public List<ComunaDTO> CargarComunas()
        {
            List<ComunaDTO> comunas = new List<ComunaDTO>();
            string query = @"SELECT id_comuna, nombre_comuna, id_region_id
                            FROM Comunas";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComunaDTO comuna = new ComunaDTO
                            {
                                IdComuna = Convert.ToInt32(reader["id_comuna"]),
                                Region = Convert.ToInt32(reader["id_region_id"]),
                                NombreComuna = reader["nombre_comuna"].ToString()
                            };
                            comunas.Add(comuna);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar comunas: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return comunas;
        }

        public List<UnidadMedidaDTO> CargarMedidas()
        {
            List<UnidadMedidaDTO> medidas = new List<UnidadMedidaDTO>();
            string query = @"SELECT id_unidad_medida, unidad_medida, peso_gramos
                            FROM Unidad_Medida";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UnidadMedidaDTO medida = new UnidadMedidaDTO
                            {
                                IdUnidadMedida = Convert.ToInt32(reader["id_unidad_medida"]),
                                UnidadMedida = reader["unidad_medida"].ToString(),
                                Peso = Convert.ToInt32(reader["peso_gramos"])
                            };
                            medidas.Add(medida);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar unidades de medida: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return medidas;
        }

        public List<ProveedorDTO> CargarProveedores()
        {
            List<ProveedorDTO> proveedores = new List<ProveedorDTO>();
            string query = @"SELECT p.id_proveedor, p.nombre_proveedor, p.correo, p.telefono, p.direccion, p.comuna_id, c.nombre_comuna
                            FROM Proveedor p
                            JOIN Comunas c ON p.comuna_id = c.id_comuna";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProveedorDTO proveedor = new ProveedorDTO
                            {
                                IdProveedor = Convert.ToInt32(reader["id_proveedor"]),
                                NombreProveedor = reader["nombre_proveedor"].ToString(),
                                Correo = reader["correo"].ToString(),
                                Telefono = Convert.ToInt32(reader["telefono"]),
                                Direccion = reader["direccion"].ToString(),
                                Comuna = reader["nombre_comuna"].ToString(),
                                IdComuna = Convert.ToInt32(reader["comuna_id"])
                            };
                            proveedores.Add(proveedor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar comunas: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return proveedores;
        }

        public List<ProductoDTO> CargarProductos()
        {
            List<ProductoDTO> productos = new List<ProductoDTO>();
            string query = @"SELECT p.id_producto, p.nombre_producto, p.descripcion, 
                            p.img, p.categoria_id, c.nombre_categoria, p.marca_id, m.nombre_marca, p.perecible_id, p.unidad_medida_id, p.volumen
                            FROM Producto p
                            JOIN Categoria c ON p.categoria_id = c.id_categoria
                            JOIN Marca m ON p.marca_id = m.id_marca";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductoDTO producto = new ProductoDTO
                            {
                                IdProducto = Convert.ToInt32(reader["id_producto"]),
                                NombreProducto = reader["nombre_producto"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                Img = reader["img"].ToString(),
                                NombreCategoria = reader["nombre_categoria"].ToString(),
                                Categoria = Convert.ToInt32(reader["categoria_id"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                Marca = Convert.ToInt32(reader["marca_id"])
                            };
                            productos.Add(producto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar productos: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return productos;
        }

        public List<SeccionBodegaDTO> CargarSeccionesBodega()
        {
            List<SeccionBodegaDTO> seccionesBodega = new List<SeccionBodegaDTO>();
            string query = "SELECT id_seccion, nombre_seccion FROM Seccion_Bodega";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var seccion = new SeccionBodegaDTO
                            {
                                IdSeccion = Convert.ToInt32(reader["id_seccion"]),
                                NombreSeccion = reader["nombre_seccion"].ToString()
                            };
                            seccionesBodega.Add(seccion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar secciones de bodega: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return seccionesBodega;
        }


        public List<BodegaDTO> CargarBodegas()
        {
            List<BodegaDTO> bodegas = new List<BodegaDTO>();
            string query = @"SELECT b.id_bodega, b.nombre, b.capacidad_maxima
                     FROM Bodega b";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BodegaDTO bodega = new BodegaDTO
                            {
                                IdBodega = Convert.ToInt32(reader["id_bodega"]),
                                NombreBodega = reader["nombre"].ToString(),
                                CapacidadMax = Convert.ToInt32(reader["capacidad_maxima"])
                            };
                            bodegas.Add(bodega);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar bodegas: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return bodegas;
        }


        public List<TiendaDTO> CargarTiendas()
        {
            List<TiendaDTO> tiendas = new List<TiendaDTO>();
            string query = @"SELECT t.id_tienda, t.nombre_tienda, t.frase_footer, t.direccion, 
                            t.logo, t.color, t.color_secundario, t.img_login, t.img_registro, 
                            c.id_comuna, c.nombre_comuna
                    FROM Tienda t
                    JOIN Comunas c ON t.comuna_id = c.id_comuna";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TiendaDTO tienda = new TiendaDTO
                            {
                                IdTienda = Convert.ToInt32(reader["id_tienda"]),
                                NombreTienda = reader["nombre_tienda"].ToString(),
                                FraseFooter = reader["frase_footer"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Logo = reader["logo"].ToString(),
                                Color = reader["color"].ToString(),
                                ColorSecundario = reader["color_secundario"].ToString(),
                                ImgLogin = reader["img_login"].ToString(),
                                ImgRegistro = reader["img_registro"].ToString(),
                                NombreComuna = reader["nombre_comuna"].ToString(),
                                Comuna = Convert.ToInt32(reader["comuna_id"])
                            };
                            tiendas.Add(tienda);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar tiendas: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return tiendas;
        }

        public List<InventarioDTO> ListarInventarios()
        {
            List<InventarioDTO> inventarios = new List<InventarioDTO>();
            string query = @"SELECT i.id_inventario, p.nombre_producto, i.cantidad_disponible, 
                            sb.nombre_seccion, b.nombre
                     FROM Inventario i
                     JOIN Producto p ON i.producto_id = p.id_producto
                     JOIN Seccion_Bodega sb ON i.seccion_bodega_id = sb.id_seccion
                     JOIN Bodega b ON i.bodega_id = b.id_bodega";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var inventario = new InventarioDTO
                            {
                                IdInventario = Convert.ToInt32(reader["id_inventario"]),
                                NombreProducto = reader["nombre_producto"].ToString(),
                                CantidadDisponible = Convert.ToInt32(reader["cantidad_disponible"]),
                                NombreSeccion = reader["nombre_seccion"].ToString(),
                                NombreBodega = reader["nombre"].ToString()
                            };

                            inventarios.Add(inventario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al listar inventarios: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return inventarios;
        }





        //----------------------------------------------------------------//
        //-----------------------METODOS ELIMINAR-------------------------//
        //----------------------------------------------------------------//

        public bool EliminarCategoria(int idCategoria)
        {
            string query = "DELETE FROM Categoria WHERE id_categoria = @id_categoria";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_categoria", idCategoria);
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Devuelve true si se eliminó algún registro
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la categoría: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool EliminaDescuento(int idDescuento)
        {
            string query = "DELETE FROM Descuentos WHERE id_descuento = @id_descuento";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_descuento", idDescuento);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el descuento: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool EliminarMarca(int idMarca)
        {
            string query = "DELETE FROM Marca WHERE id_marca = @id_marca";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_marca", idMarca);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la marca: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool EliminarProveedor(int idProveedor)
        {
            string query = "DELETE FROM Proveedor WHERE id_proveedor = @id_proveedor";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_proveedor", idProveedor);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el proveedor: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool EliminarProducto(int idProducto)
        {
            string checkQuery = "SELECT COUNT(*) FROM Lotes WHERE producto_id = @id_producto";
            string deleteQuery = "DELETE FROM Producto WHERE id_producto = @id_producto";

            try
            {
                OpenConnection();

                using (var checkCommand = new SQLiteCommand(checkQuery, conexion))
                {
                    checkCommand.Parameters.AddWithValue("@id_producto", idProducto);
                    long count = (long)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine("No se puede eliminar el producto porque está asociado a un lote.");
                        return false;
                    }
                }

                using (var deleteCommand = new SQLiteCommand(deleteQuery, conexion))
                {
                    deleteCommand.Parameters.AddWithValue("@id_producto", idProducto);
                    int result = deleteCommand.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el producto: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


        public bool EliminarAnuncio(int idAnuncio)
        {
            string query = "DELETE FROM Anuncios WHERE id_anuncio = @id_anuncio";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_anuncio", idAnuncio);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el anuncio: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool EliminarSeccionBodega(int idSeccion)
        {
            bool exito = false;
            string verificarUsoQuery = "SELECT COUNT(*) FROM Lotes WHERE seccion_bodega = @idSeccion";

            try
            {
                OpenConnection();

                using (var command = new SQLiteCommand(verificarUsoQuery, conexion))
                {
                    command.Parameters.AddWithValue("@idSeccion", idSeccion);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        Console.WriteLine("No se puede eliminar la sección, ya está en uso en un lote.");
                        return false;
                    }
                }

                // Si no está en uso, procedemos con la eliminación
                string query = "DELETE FROM Seccion_Bodega WHERE id_seccion = @idSeccion";

                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@idSeccion", idSeccion);
                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        exito = true; // Si se eliminó al menos una fila, la operación fue exitosa
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la sección de bodega: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return exito;
        }

        public bool EliminarBodega(int idBodega)
        {
            bool exito = false;
            string verificarUsoQuery = "SELECT COUNT(*) FROM Lotse WHERE bodega = @idBodega";

            try
            {
                OpenConnection();

                using (var command = new SQLiteCommand(verificarUsoQuery, conexion))
                {
                    command.Parameters.AddWithValue("@idBodega", idBodega);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)
                    {
                        Console.WriteLine("No se puede eliminar la bodega, ya está en uso en un lote.");
                        return false;
                    }
                }

                string query = "DELETE FROM Bodega WHERE id_bodega = @idBodega";

                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@idBodega", idBodega);
                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la bodega: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return exito;
        }


        //----------------------------------------------------------------//
        //-----------------------METODOS MODIFICAR------------------------//
        //----------------------------------------------------------------//

        public bool ModificarCategoria(int idCategoria, string nuevoNombre)
        {
            string query = "UPDATE Categoria SET nombre_categoria = @nombre_categoria WHERE id_categoria = @id_categoria";
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_categoria", idCategoria);
                    command.Parameters.AddWithValue("@nombre_categoria", nuevoNombre);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la categoría: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarTienda(int idTienda, string nombreTienda, string fraseFooter, string direccion, int idComuna, string logo, string color, string colorSecundario, string imgLogin, string imgRegistro)
        {
            string query = "UPDATE Tienda SET nombre_tienda = @nombre_tienda, frase_footer = @frase_footer, direccion = @direccion, comuna_id = @id_comuna, " +
                           "logo = @logo, color = @color, color_secundario = @color_secundario, img_login = @img_login, img_registro = @img_registro " +
                           "WHERE id_tienda = @id_tienda";

            try
            {
                string nombreLogo = Path.GetFileName(logo);
                string rutaDestinoLogo = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreLogo);
                File.Copy(logo, rutaDestinoLogo, true);
                string rutaLogoRelativa = Path.Combine("tienda", nombreLogo);

                string nombreImgLogin = Path.GetFileName(imgLogin);
                string rutaDestinoImgLogin = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreImgLogin);
                File.Copy(imgLogin, rutaDestinoImgLogin, true);
                string rutaImgLoginRelativa = Path.Combine("tienda", nombreImgLogin);

                string nombreImgRegistro = Path.GetFileName(imgRegistro);
                string rutaDestinoImgRegistro = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\tienda", nombreImgRegistro);
                File.Copy(imgRegistro, rutaDestinoImgRegistro, true);
                string rutaImgRegistroRelativa = Path.Combine("tienda", nombreImgRegistro);

                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@id_tienda", idTienda);
                    command.Parameters.AddWithValue("@nombre_tienda", nombreTienda);
                    command.Parameters.AddWithValue("@frase_footer", fraseFooter ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@direccion", direccion);
                    command.Parameters.AddWithValue("@id_comuna", idComuna);
                    command.Parameters.AddWithValue("@logo", rutaLogoRelativa);
                    command.Parameters.AddWithValue("@color", color ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@color_secundario", colorSecundario ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@img_login", rutaImgLoginRelativa);
                    command.Parameters.AddWithValue("@img_registro", rutaImgRegistroRelativa);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar la tienda: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


        public bool ModificarProducto(int idProducto, string nombreProducto, int precio, string descripcion, string imagen, int idCategoria, int idMarca, int idPerecible, int idUnidadMedida, int volumen)
        {
            string nombreImagen = Path.GetFileName(imagen);
            string rutaDestino = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\productos", nombreImagen);
            string rutaImagenRelativa = Path.Combine("productos", nombreImagen);
            // Verificar si la imagen en la base de datos es diferente de la imagen nueva
            string imagenActualQuery = "SELECT img FROM Producto WHERE id_producto = @id_producto";
            string imagenActual = "";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(imagenActualQuery, conexion))
                {
                    command.Parameters.AddWithValue("@id_producto", idProducto);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imagenActual = reader["img"].ToString();
                        }
                    }
                }

                if (imagenActual != rutaImagenRelativa)
                {
                    try
                    {
                        if (!File.Exists(rutaDestino))
                        {
                            File.Copy(imagen, rutaDestino, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al copiar la imagen: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar la imagen: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }

            // Query de actualización en la base de datos
            string query = "UPDATE Producto SET nombre_producto = @nombre_producto, precio = @precio, descripcion = @descripcion, " +
                           "stock = @stock, img = @img, categoria_id = @categoria_id, volumen = @volumen, marca_id = @marca_id, perecible_id = @perecible_id, unidad_medida_id = @unidad_medida_id " +
                           "WHERE id_producto = @id_producto";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombre_producto", nombreProducto);
                    command.Parameters.AddWithValue("@precio", precio);
                    command.Parameters.AddWithValue("@descripcion", descripcion);
                    command.Parameters.AddWithValue("@img", rutaImagenRelativa);
                    command.Parameters.AddWithValue("@id_categoria_id", idCategoria);
                    command.Parameters.AddWithValue("@id_marca_id", idMarca);
                    command.Parameters.AddWithValue("@id_proveedor_id", idPerecible);
                    command.Parameters.AddWithValue("@id_proveedor_id", idUnidadMedida);
                    command.Parameters.AddWithValue("@id_proveedor_id", volumen);
                    command.Parameters.AddWithValue("@id_producto", idProducto);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el producto: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarMarca(int idMarca, string nuevoNombre)
        {
            string query = "UPDATE Marca SET nombre_marca = @nuevo_nombre WHERE id_marca = @id_marca";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nuevo_nombre", nuevoNombre);
                    command.Parameters.AddWithValue("@id_marca", idMarca);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar la marca: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarDescuento(int idDescuento, int nuevosDiasVencimiento, int nuevoPorcentaje)
        {
            string query = "UPDATE Descuentos SET dias_para_vencimiento = @nuevos_dias, descuento = @nuevo_descuento WHERE id_descuento = @id_descuento";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nuevos_dias", nuevosDiasVencimiento);
                    command.Parameters.AddWithValue("@nuevo_descuento", nuevoPorcentaje);
                    command.Parameters.AddWithValue("@id_descuento", idDescuento);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el descuento: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarLocal(int idLocal, string nuevoNombre, string nuevaDireccion, int nuevaComuna)
        {
            string query = "UPDATE Locales SET nombre_local = @nuevo_nombre, direccion = @nueva_direccion, comuna_id = @nueva_comuna WHERE id_local = @id_local";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nuevo_nombre", nuevoNombre);
                    command.Parameters.AddWithValue("@nueva_direccion", nuevaDireccion);
                    command.Parameters.AddWithValue("@nueva_comuna", nuevaComuna);
                    command.Parameters.AddWithValue("@id_local", idLocal);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el local: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarProveedor(int idProveedor, string nuevoNombre, int nuevoTelefono, string nuevoCorreo, string nuevaDireccion, int nuevaComuna, int nuevoPrefijo)
        {
            string query = "UPDATE Proveedor SET nombre_proveedor = @nuevo_nombre, telefono = @nuevo_telefono, correo = @nuevo_correo, direccion = @nueva_direccion, comuna_id = @nueva_comuna, prefijo_id = @nuevo_prefijo WHERE id_proveedor = @id_proveedor";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nuevo_nombre", nuevoNombre);
                    command.Parameters.AddWithValue("@nuevo_telefono", nuevoTelefono);
                    command.Parameters.AddWithValue("@nuevo_correo", nuevoCorreo);
                    command.Parameters.AddWithValue("@nueva_direccion", nuevaDireccion);
                    command.Parameters.AddWithValue("@nueva_comuna", nuevaComuna);
                    command.Parameters.AddWithValue("@nuevo_prefijo", nuevoPrefijo);
                    command.Parameters.AddWithValue("@id_proveedor", idProveedor);
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el proveedor: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarAnuncio(int idAnuncio, string nuevoTitulo, string nuevaDescripcion, DateTime nuevaFechaInicio, DateTime nuevaFechaFin, string nuevaImagen)
        {
            string query = "UPDATE Anuncios SET titulo = @nuevo_titulo, descripcion = @nueva_descripcion, fecha_inicio = @nueva_fecha_inicio, fecha_fin = @nueva_fecha_fin, img = @nueva_img WHERE id_anuncio = @id_anuncio";

            string nombreImagen = Path.GetFileName(nuevaImagen);
            string rutaDestino = Path.Combine(@"C:\Users\carol\Desktop\SAPPE\media\anuncios", nombreImagen);
            string rutaImagenRelativa = Path.Combine("anuncios", nombreImagen);
            // Verificar si la imagen en la base de datos es diferente de la imagen nueva
            string imagenActualQuery = "SELECT img FROM Anuncios WHERE id_anuncio = @id_anuncio";
            string imagenActual = "";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(imagenActualQuery, conexion))
                {
                    command.Parameters.AddWithValue("@id_anuncio", idAnuncio);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imagenActual = reader["img"].ToString();
                        }
                    }
                }

                // Si la imagen actual es diferente a la nueva imagen, copiamos la nueva imagen
                if (imagenActual != rutaImagenRelativa)
                {
                    try
                    {
                        // Verificar si la imagen ya existe en la carpeta de destino
                        if (!File.Exists(rutaDestino))
                        {
                            // Si no existe, copiar la nueva imagen
                            File.Copy(nuevaImagen, rutaDestino, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al copiar la imagen: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar la imagen: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nuevo_titulo", nuevoTitulo);
                    command.Parameters.AddWithValue("@nueva_descripcion", nuevaDescripcion);
                    command.Parameters.AddWithValue("@nueva_fecha_inicio", nuevaFechaInicio.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@nueva_fecha_fin", nuevaFechaFin.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@nueva_img", rutaImagenRelativa);
                    command.Parameters.AddWithValue("@id_anuncio", idAnuncio);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar el anuncio: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool ModificarSeccionBodega(int idSeccion, string nuevoNombreSeccion)
        {
            bool exito = false;
            string query = "UPDATE Seccion_Bodega SET nombre_seccion = @nombreSeccion WHERE id_seccion = @idSeccion";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@nombreSeccion", nuevoNombreSeccion);
                    command.Parameters.AddWithValue("@idSeccion", idSeccion);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar la sección de bodega: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return exito;
        }

        public bool ModificarBodega(int idBodega, int nuevaCapacidadMaxima)
        {
            bool exito = false;
            string query = "UPDATE Bodega SET capacidad_maxima = @capacidadMaxima WHERE id_bodega = @idBodega";

            try
            {
                OpenConnection();
                using (var command = new SQLiteCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@capacidadMaxima", nuevaCapacidadMaxima);
                    command.Parameters.AddWithValue("@idBodega", idBodega);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar la bodega: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return exito;
        }


        private bool ActualizarInventario(LoteDTO lote, int idSeccionBodega, int idBodega)
        {
            try
            {
                string selectQuery = "SELECT cantidad_disponible FROM Inventario WHERE producto_id = @producto_id AND seccion_bodega_id = @seccion_bodega_id AND bodega_id = @bodega_id";
                int cantidadActual = 0;

                using (var selectCommand = new SQLiteCommand(selectQuery, conexion))
                {
                    selectCommand.Parameters.AddWithValue("@producto_id", lote.Producto);
                    selectCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                    selectCommand.Parameters.AddWithValue("@bodega_id", idBodega);

                    var result = selectCommand.ExecuteScalar();
                    if (result != null)
                    {
                        cantidadActual = Convert.ToInt32(result);
                    }
                }

                if (cantidadActual > 0)
                {
                    string updateQuery = "UPDATE Inventario SET cantidad_disponible = cantidad_disponible + @cantidad " +
                                         "WHERE producto_id = @producto_id AND seccion_bodega_id = @seccion_bodega_id AND bodega_id = @bodega_id";
                    using (var updateCommand = new SQLiteCommand(updateQuery, conexion))
                    {
                        updateCommand.Parameters.AddWithValue("@cantidad", lote.Cantidad);
                        updateCommand.Parameters.AddWithValue("@producto_id", lote.Producto);
                        updateCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                        updateCommand.Parameters.AddWithValue("@bodega_id", idBodega);
                        updateCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    string insertQuery = "INSERT INTO Inventario (producto_id, cantidad_disponible, seccion_bodega_id, bodega_id) " +
                                         "VALUES (@producto_id, @cantidad, @seccion_bodega_id, @bodega_id)";
                    using (var insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.AddWithValue("@producto_id", lote.Producto);
                        insertCommand.Parameters.AddWithValue("@cantidad", lote.Cantidad);
                        insertCommand.Parameters.AddWithValue("@seccion_bodega_id", idSeccionBodega);
                        insertCommand.Parameters.AddWithValue("@bodega_id", idBodega);
                        insertCommand.ExecuteNonQuery();
                    }
                }

                // Si no hubo errores, devolvemos true
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el inventario: {ex.Message}");
                return false; // Si ocurre un error, devolvemos false
            }
        }




    }
}
