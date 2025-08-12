using TraductorBasico.Infrastructure.Context;
using TraductorBasico.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TraductorBasico.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(TraductorBasicoDataContext context)
        {
            try
            {
                // Asegurar que la base de datos esté creada
                await context.Database.EnsureCreatedAsync();

                // Si ya hay datos, no hacer seed
                if (await context.Frases.AnyAsync() || await context.ImagenesDiccionario.AnyAsync())
                {
                    return;
                }

                // Seed Frases
                var frases = new List<Frase>
                {
                    // Restaurante
                    new Frase { Español = "Una mesa para dos, por favor", Ingles = "A table for two, please", Pronunciacion = "A téibol for tu, plis", Categoria = "Restaurante" },
                    new Frase { Español = "¿Qué me recomienda?", Ingles = "What do you recommend?", Pronunciacion = "Wot du yu rékomend?", Categoria = "Restaurante" },
                    new Frase { Español = "La cuenta, por favor", Ingles = "The check, please", Pronunciacion = "The chek, plis", Categoria = "Restaurante" },
                    new Frase { Español = "¿Tienen menú en español?", Ingles = "Do you have a menu in Spanish?", Pronunciacion = "Du yu hav a ményu in spanish?", Categoria = "Restaurante" },
                    new Frase { Español = "Sin cebolla, por favor", Ingles = "No onions, please", Pronunciacion = "Nou ónions, plis", Categoria = "Restaurante" },

                    // Hotel
                    new Frase { Español = "Tengo una reservación", Ingles = "I have a reservation", Pronunciacion = "Ai hav a resérveshon", Categoria = "Hotel" },
                    new Frase { Español = "¿A qué hora es el check-out?", Ingles = "What time is check-out?", Pronunciacion = "Wot taim is chek-aut?", Categoria = "Hotel" },
                    new Frase { Español = "¿Hay wifi gratuito?", Ingles = "Is there free wifi?", Pronunciacion = "Is der fri waifai?", Categoria = "Hotel" },
                    new Frase { Español = "Necesito toallas limpias", Ingles = "I need clean towels", Pronunciacion = "Ai nid klin táuels", Categoria = "Hotel" },
                    new Frase { Español = "¿Dónde está el gimnasio?", Ingles = "Where is the gym?", Pronunciacion = "Wer is de gym?", Categoria = "Hotel" },

                    // Direcciones
                    new Frase { Español = "¿Dónde está el banco?", Ingles = "Where is the bank?", Pronunciacion = "Wer is de bank?", Categoria = "Direcciones" },
                    new Frase { Español = "Disculpe, estoy perdido", Ingles = "Excuse me, I'm lost", Pronunciacion = "Ekskyus mi, aim lost", Categoria = "Direcciones" },
                    new Frase { Español = "¿Está cerca de aquí?", Ingles = "Is it near here?", Pronunciacion = "Is it nir hir?", Categoria = "Direcciones" },
                    new Frase { Español = "Gire a la derecha", Ingles = "Turn right", Pronunciacion = "Tern rait", Categoria = "Direcciones" },
                    new Frase { Español = "Siga derecho", Ingles = "Go straight", Pronunciacion = "Gou streit", Categoria = "Direcciones" },

                    // Compras
                    new Frase { Español = "¿Cuánto cuesta esto?", Ingles = "How much does this cost?", Pronunciacion = "Jau mach das dis cost?", Categoria = "Compras" },
                    new Frase { Español = "¿Tienen descuento?", Ingles = "Do you have a discount?", Pronunciacion = "Du yu hav a diskaunt?", Categoria = "Compras" },
                    new Frase { Español = "¿Aceptan tarjetas de crédito?", Ingles = "Do you accept credit cards?", Pronunciacion = "Du yu aksept krédit cards?", Categoria = "Compras" },
                    new Frase { Español = "Solo estoy mirando", Ingles = "I'm just looking", Pronunciacion = "Aim yast lúking", Categoria = "Compras" },
                    new Frase { Español = "¿Tienen otra talla?", Ingles = "Do you have another size?", Pronunciacion = "Du yu hav anáder saiz?", Categoria = "Compras" },

                    // Transporte
                    new Frase { Español = "¿Dónde está la parada de autobús?", Ingles = "Where is the bus stop?", Pronunciacion = "Wer is de bas stop?", Categoria = "Transporte" },
                    new Frase { Español = "¿Cuánto cuesta el boleto?", Ingles = "How much is the ticket?", Pronunciacion = "Jau mach is de tiket?", Categoria = "Transporte" },
                    new Frase { Español = "Al aeropuerto, por favor", Ingles = "To the airport, please", Pronunciacion = "Tu de érport, plis", Categoria = "Transporte" },
                    new Frase { Español = "¿Cuánto tiempo toma?", Ingles = "How long does it take?", Pronunciacion = "Jau long das it teik?", Categoria = "Transporte" },
                    new Frase { Español = "Pare aquí, por favor", Ingles = "Stop here, please", Pronunciacion = "Stop hir, plis", Categoria = "Transporte" },

                    // Emergencias
                    new Frase { Español = "¡Ayuda!", Ingles = "Help!", Pronunciacion = "Jelp!", Categoria = "Emergencias" },
                    new Frase { Español = "Llame a la policía", Ingles = "Call the police", Pronunciacion = "Kol de polis", Categoria = "Emergencias" },
                    new Frase { Español = "Necesito un médico", Ingles = "I need a doctor", Pronunciacion = "Ai nid a dóktor", Categoria = "Emergencias" },
                    new Frase { Español = "¿Dónde está el hospital?", Ingles = "Where is the hospital?", Pronunciacion = "Wer is de jóspital?", Categoria = "Emergencias" },
                    new Frase { Español = "No hablo inglés", Ingles = "I don't speak English", Pronunciacion = "Ai dont spik inglish", Categoria = "Emergencias" }
                };

                await context.Frases.AddRangeAsync(frases);
                await context.SaveChangesAsync();

                // Seed Imágenes del Diccionario
                var imagenes = new List<ImagenDiccionario>
                {
                    // Restaurante
                    new ImagenDiccionario
                    {
                        Nombre = "Mesa de Restaurante",
                        RutaImagen = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=400&h=300&fit=crop",
                        Categoria = "Restaurante",
                        Descripcion = "Mesa elegante en un restaurante para una cena romántica",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Menú de Restaurante",
                        RutaImagen = "https://images.unsplash.com/photo-1676299806240-cc1686ef06b5?fm",
                        Categoria = "Restaurante",
                        Descripcion = "Menú abierto mostrando diferentes opciones de comida",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Camarero Sirviendo",
                        RutaImagen = "https://plus.unsplash.com/premium_photo-1661382334045-30d663feff7c?q=80&w=870&auto=format&fit=crop",
                        Categoria = "Restaurante",
                        Descripcion = "Camarero profesional sirviendo en un restaurante",
                        FechaCreacion = DateTime.Now
                    },

                    // Hotel
                    new ImagenDiccionario
                    {
                        Nombre = "Recepción de Hotel",
                        RutaImagen = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=400&h=300&fit=crop",
                        Categoria = "Hotel",
                        Descripcion = "Elegante recepción de hotel",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Habitación de Hotel",
                        RutaImagen = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=400&h=300&fit=crop",
                        Categoria = "Hotel",
                        Descripcion = "Habitación cómoda y moderna con cama king size",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Servicios del Hotel",
                        RutaImagen = "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=400&h=300&fit=crop",
                        Categoria = "Hotel",
                        Descripcion = "Amenidades y servicios disponibles en el hotel",
                        FechaCreacion = DateTime.Now
                    },

                    // Direcciones
                    new ImagenDiccionario
                    {
                        Nombre = "Mapa de la Ciudad",
                        RutaImagen = "https://images.unsplash.com/photo-1524661135-423995f22d0b?w=400&h=300&fit=crop",
                        Categoria = "Direcciones",
                        Descripcion = "Mapa detallado para encontrar ubicaciones en la ciudad",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Señales de Tráfico",
                        RutaImagen = "https://images.unsplash.com/photo-1449824913935-59a10b8d2000?w=400&h=300&fit=crop",
                        Categoria = "Direcciones",
                        Descripcion = "Señales direccionales en una intersección urbana",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Brújula",
                        RutaImagen = "https://images.unsplash.com/photo-1524146128017-b9dd0bfd2778?w=2070&auto=format&fit=crop",
                        Categoria = "Direcciones",
                        Descripcion = "Brújula tradicional encima de un mapa",
                        FechaCreacion = DateTime.Now
                    },

                    // Compras
                    new ImagenDiccionario
                    {
                        Nombre = "Centro Comercial",
                        RutaImagen = "https://images.unsplash.com/photo-1580793241553-e9f1cce181af?q=80&w=1032&auto=format&fit=crop",
                        Categoria = "Compras",
                        Descripcion = "Moderno centro comercial con múltiples tiendas",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Caja Registradora",
                        RutaImagen = "https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?w=400&h=300&fit=crop",
                        Categoria = "Compras",
                        Descripcion = "Proceso de pago en una tienda de retail",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Bolsas de Compras",
                        RutaImagen = "https://images.unsplash.com/photo-1604118464816-5e2bd7b863c2?q=80&w=870&auto=format&fit=crop",
                        Categoria = "Compras",
                        Descripcion = "Bolsas coloridas después de una sesión de compras",
                        FechaCreacion = DateTime.Now
                    },

                    // Transporte
                    new ImagenDiccionario
                    {
                        Nombre = "Aeropuerto",
                        RutaImagen = "https://images.unsplash.com/photo-1527007622069-3a0241e1cd8c?q=80&w=774&auto=format&fit=crop",
                        Categoria = "Transporte",
                        Descripcion = "Terminal de aeropuerto con vuelos internacionales",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Taxi",
                        RutaImagen = "https://images.unsplash.com/photo-1630717285906-29364ffacea0?q=80&w=435&auto=format&fit=crop",
                        Categoria = "Transporte",
                        Descripcion = "Taxi amarillo en una calle de la ciudad",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Estación de Metro",
                        RutaImagen = "https://images.unsplash.com/photo-1556695736-d287caebc48e?q=80&w=870&auto=format&fit=crop",
                        Categoria = "Transporte",
                        Descripcion = "Estación moderna de metro subterráneo",
                        FechaCreacion = DateTime.Now
                    },

                    // Emergencias
                    new ImagenDiccionario
                    {
                        Nombre = "Hospital",
                        RutaImagen = "https://images.unsplash.com/photo-1596541223130-5d31a73fb6c6?q=80&w=871&auto=format&fit=crop",
                        Categoria = "Emergencias",
                        Descripcion = "Entrada principal de un hospital moderno",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Estación de Policía",
                        RutaImagen = "https://images.unsplash.com/photo-1708299983282-9429c6ad67ee?q=80&w=435&auto=format&fit=crop",
                        Categoria = "Emergencias",
                        Descripcion = "Edificio de la estación de policía local",
                        FechaCreacion = DateTime.Now
                    },
                    new ImagenDiccionario
                    {
                        Nombre = "Teléfono de Emergencia",
                        RutaImagen = "https://images.unsplash.com/photo-1643446950085-d5cfdba8cfdf?q=80&w=415&auto=format&fit=crop",
                        Categoria = "Emergencias",
                        Descripcion = "Teléfono de emergencia en lugar público",
                        FechaCreacion = DateTime.Now
                    }
                };

                await context.ImagenesDiccionario.AddRangeAsync(imagenes);
                await context.SaveChangesAsync();

                // Obtener las frases e imágenes recién creadas con sus IDs reales
                var frasesCreadas = await context.Frases.ToListAsync();
                var imagenesCreadas = await context.ImagenesDiccionario.ToListAsync();

                // Crear relaciones usando los IDs reales generados
                var relaciones = new List<FraseImagen>();

                // Función helper para encontrar frases e imágenes por nombre/texto
                var frasesRestaurante = frasesCreadas.Where(f => f.Categoria == "Restaurante").ToList();
                var frasesHotel = frasesCreadas.Where(f => f.Categoria == "Hotel").ToList();
                var frasesDirecciones = frasesCreadas.Where(f => f.Categoria == "Direcciones").ToList();
                var frasesCompras = frasesCreadas.Where(f => f.Categoria == "Compras").ToList();
                var frasesTransporte = frasesCreadas.Where(f => f.Categoria == "Transporte").ToList();
                var frasesEmergencias = frasesCreadas.Where(f => f.Categoria == "Emergencias").ToList();

                var imagenesRestaurante = imagenesCreadas.Where(i => i.Categoria == "Restaurante").ToList();
                var imagenesHotel = imagenesCreadas.Where(i => i.Categoria == "Hotel").ToList();
                var imagenesDirecciones = imagenesCreadas.Where(i => i.Categoria == "Direcciones").ToList();
                var imagenesCompras = imagenesCreadas.Where(i => i.Categoria == "Compras").ToList();
                var imagenesTransporte = imagenesCreadas.Where(i => i.Categoria == "Transporte").ToList();
                var imagenesEmergencias = imagenesCreadas.Where(i => i.Categoria == "Emergencias").ToList();

                // Restaurante - Mesa de Restaurante
                if (imagenesRestaurante.Count > 0)
                {
                    var mesaRestaurante = imagenesRestaurante.First(i => i.Nombre.Contains("Mesa"));
                    var mesaParaDos = frasesRestaurante.FirstOrDefault(f => f.Español.Contains("mesa para dos"));
                    var laCuenta = frasesRestaurante.FirstOrDefault(f => f.Español.Contains("cuenta"));

                    if (mesaParaDos != null) relaciones.Add(new FraseImagen { FraseId = mesaParaDos.Id, ImagenId = mesaRestaurante.Id });
                    if (laCuenta != null) relaciones.Add(new FraseImagen { FraseId = laCuenta.Id, ImagenId = mesaRestaurante.Id });
                }

                // Restaurante - Menú
                if (imagenesRestaurante.Count > 1)
                {
                    var menuRestaurante = imagenesRestaurante.First(i => i.Nombre.Contains("Menú"));
                    var recomienda = frasesRestaurante.FirstOrDefault(f => f.Español.Contains("recomienda"));
                    var menuEspanol = frasesRestaurante.FirstOrDefault(f => f.Español.Contains("menú en español"));
                    var sinCebolla = frasesRestaurante.FirstOrDefault(f => f.Español.Contains("Sin cebolla"));

                    if (recomienda != null) relaciones.Add(new FraseImagen { FraseId = recomienda.Id, ImagenId = menuRestaurante.Id });
                    if (menuEspanol != null) relaciones.Add(new FraseImagen { FraseId = menuEspanol.Id, ImagenId = menuRestaurante.Id });
                    if (sinCebolla != null) relaciones.Add(new FraseImagen { FraseId = sinCebolla.Id, ImagenId = menuRestaurante.Id });
                }

                // Hotel - Recepción
                if (imagenesHotel.Count > 0)
                {
                    var recepcionHotel = imagenesHotel.First(i => i.Nombre.Contains("Recepción"));
                    var reservacion = frasesHotel.FirstOrDefault(f => f.Español.Contains("reservación"));
                    var checkout = frasesHotel.FirstOrDefault(f => f.Español.Contains("check-out"));

                    if (reservacion != null) relaciones.Add(new FraseImagen { FraseId = reservacion.Id, ImagenId = recepcionHotel.Id });
                    if (checkout != null) relaciones.Add(new FraseImagen { FraseId = checkout.Id, ImagenId = recepcionHotel.Id });
                }

                // Hotel - Habitación
                if (imagenesHotel.Count > 1)
                {
                    var habitacionHotel = imagenesHotel.First(i => i.Nombre.Contains("Habitación"));
                    var wifi = frasesHotel.FirstOrDefault(f => f.Español.Contains("wifi"));
                    var toallas = frasesHotel.FirstOrDefault(f => f.Español.Contains("toallas"));

                    if (wifi != null) relaciones.Add(new FraseImagen { FraseId = wifi.Id, ImagenId = habitacionHotel.Id });
                    if (toallas != null) relaciones.Add(new FraseImagen { FraseId = toallas.Id, ImagenId = habitacionHotel.Id });
                }

                // Direcciones - Mapa
                if (imagenesDirecciones.Count > 0)
                {
                    var mapaCiudad = imagenesDirecciones.First(i => i.Nombre.Contains("Mapa"));
                    var banco = frasesDirecciones.FirstOrDefault(f => f.Español.Contains("banco"));
                    var perdido = frasesDirecciones.FirstOrDefault(f => f.Español.Contains("perdido"));
                    var cerca = frasesDirecciones.FirstOrDefault(f => f.Español.Contains("cerca"));

                    if (banco != null) relaciones.Add(new FraseImagen { FraseId = banco.Id, ImagenId = mapaCiudad.Id });
                    if (perdido != null) relaciones.Add(new FraseImagen { FraseId = perdido.Id, ImagenId = mapaCiudad.Id });
                    if (cerca != null) relaciones.Add(new FraseImagen { FraseId = cerca.Id, ImagenId = mapaCiudad.Id });
                }

                // Direcciones - Señales
                if (imagenesDirecciones.Count > 1)
                {
                    var senales = imagenesDirecciones.First(i => i.Nombre.Contains("Señales"));
                    var derecha = frasesDirecciones.FirstOrDefault(f => f.Español.Contains("derecha"));
                    var derecho = frasesDirecciones.FirstOrDefault(f => f.Español.Contains("derecho"));

                    if (derecha != null) relaciones.Add(new FraseImagen { FraseId = derecha.Id, ImagenId = senales.Id });
                    if (derecho != null) relaciones.Add(new FraseImagen { FraseId = derecho.Id, ImagenId = senales.Id });
                }

                // Compras - Centro Comercial
                if (imagenesCompras.Count > 0)
                {
                    var centroComercial = imagenesCompras.First(i => i.Nombre.Contains("Centro"));
                    var cuantoCuesta = frasesCompras.FirstOrDefault(f => f.Español.Contains("Cuánto cuesta"));
                    var mirando = frasesCompras.FirstOrDefault(f => f.Español.Contains("mirando"));

                    if (cuantoCuesta != null) relaciones.Add(new FraseImagen { FraseId = cuantoCuesta.Id, ImagenId = centroComercial.Id });
                    if (mirando != null) relaciones.Add(new FraseImagen { FraseId = mirando.Id, ImagenId = centroComercial.Id });
                }

                // Compras - Caja
                if (imagenesCompras.Count > 1)
                {
                    var caja = imagenesCompras.First(i => i.Nombre.Contains("Caja"));
                    var descuento = frasesCompras.FirstOrDefault(f => f.Español.Contains("descuento"));
                    var tarjetas = frasesCompras.FirstOrDefault(f => f.Español.Contains("tarjetas"));

                    if (descuento != null) relaciones.Add(new FraseImagen { FraseId = descuento.Id, ImagenId = caja.Id });
                    if (tarjetas != null) relaciones.Add(new FraseImagen { FraseId = tarjetas.Id, ImagenId = caja.Id });
                }

                // Transporte - Aeropuerto
                if (imagenesTransporte.Count > 0)
                {
                    var aeropuerto = imagenesTransporte.First(i => i.Nombre.Contains("Aeropuerto"));
                    var alAeropuerto = frasesTransporte.FirstOrDefault(f => f.Español.Contains("aeropuerto"));

                    if (alAeropuerto != null) relaciones.Add(new FraseImagen { FraseId = alAeropuerto.Id, ImagenId = aeropuerto.Id });
                }

                // Transporte - Taxi
                if (imagenesTransporte.Count > 1)
                {
                    var taxi = imagenesTransporte.First(i => i.Nombre.Contains("Taxi"));
                    var pare = frasesTransporte.FirstOrDefault(f => f.Español.Contains("Pare aquí"));

                    if (pare != null) relaciones.Add(new FraseImagen { FraseId = pare.Id, ImagenId = taxi.Id });
                }

                // Emergencias - Hospital
                if (imagenesEmergencias.Count > 0)
                {
                    var hospital = imagenesEmergencias.First(i => i.Nombre.Contains("Hospital"));
                    var medico = frasesEmergencias.FirstOrDefault(f => f.Español.Contains("médico"));
                    var dondeHospital = frasesEmergencias.FirstOrDefault(f => f.Español.Contains("hospital"));

                    if (medico != null) relaciones.Add(new FraseImagen { FraseId = medico.Id, ImagenId = hospital.Id });
                    if (dondeHospital != null && dondeHospital.Id != medico?.Id)
                        relaciones.Add(new FraseImagen { FraseId = dondeHospital.Id, ImagenId = hospital.Id });
                }

                // Emergencias - Policía
                if (imagenesEmergencias.Count > 1)
                {
                    var policia = imagenesEmergencias.First(i => i.Nombre.Contains("Policía"));
                    var llamarPolicia = frasesEmergencias.FirstOrDefault(f => f.Español.Contains("policía"));
                    var ayuda = frasesEmergencias.FirstOrDefault(f => f.Español.Contains("Ayuda"));

                    if (llamarPolicia != null) relaciones.Add(new FraseImagen { FraseId = llamarPolicia.Id, ImagenId = policia.Id });
                    if (ayuda != null) relaciones.Add(new FraseImagen { FraseId = ayuda.Id, ImagenId = policia.Id });
                }

                // Emergencias - Teléfono
                if (imagenesEmergencias.Count > 2)
                {
                    var telefono = imagenesEmergencias.First(i => i.Nombre.Contains("Teléfono"));
                    var noHablo = frasesEmergencias.FirstOrDefault(f => f.Español.Contains("No hablo"));

                    if (noHablo != null) relaciones.Add(new FraseImagen { FraseId = noHablo.Id, ImagenId = telefono.Id });
                }

                // Guardar las relaciones
                if (relaciones.Any())
                {
                    await context.FraseImagenes.AddRangeAsync(relaciones);
                    await context.SaveChangesAsync();
                }

                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}