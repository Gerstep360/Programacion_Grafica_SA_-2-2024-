using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using OpenTK_Tarea_5.Extra;

namespace OpenTK_Tarea_5.Clases_Base
{
    internal class Serializador
    {
        private readonly JsonSerializerOptions _opciones;
        private readonly string _rutaDefault = "Modelos 3D\\";

        // Constructor para configurar las opciones de serialización
        public Serializador(bool incluirCampos = true, bool indentar = true)
        {
            _opciones = new JsonSerializerOptions
            {
                WriteIndented = indentar,
                IncludeFields = incluirCampos,
                ReferenceHandler = ReferenceHandler.Preserve // Agregar soporte para referencias cíclicas
            };
        }

        // Filtra y muestra los hashes de los objetos en el escenario
        public void FiltrarHashes(Escenario escenario)
        {
            var hashes = escenario.ObtenerTodosLosHashes().ToArray();

            if (hashes.Length == 0)
            {
                Console.WriteLine("No se encontraron modelos en el sistema.");
                return;
            }

            Console.WriteLine("Hashes de modelos disponibles:");
            for (int i = 0; i < hashes.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {hashes[i]}");
            }
        }

        // Serializar y guardar un objeto 3D en un archivo JSON
        public void Guardar3D(string idModelo, Escenario escenario)
        {
            if (string.IsNullOrEmpty(idModelo))
            {
                Console.WriteLine("El nombre del archivo no puede ser nulo o vacío.");
                return;
            }

            var objeto3D = escenario.ObtenerObjeto(idModelo);

            if (objeto3D == null)
            {
                Console.WriteLine($"El modelo con ID '{idModelo}' no existe.");
                return;
            }

            try
            {
                // Convertir el objeto a DTO
                var objetoDTO = ConvertirAObjetoDTO(objeto3D);
                string rutaCompleta = Path.Combine(_rutaDefault, $"{idModelo}.json");

                // Crear el directorio si no existe
                CrearDirectorioSiNoExiste(rutaCompleta);

                // Serializar y guardar el archivo JSON
                var jsonString = JsonSerializer.Serialize(objetoDTO, _opciones);
                File.WriteAllText(rutaCompleta, jsonString);
                Console.WriteLine($"Objeto 3D guardado en {rutaCompleta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el objeto 3D: {ex.Message}");
            }
        }

        // Método para cargar un objeto 3D desde un archivo JSON
        public Objeto? Cargar3D(string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo))
            {
                Console.WriteLine("El nombre del archivo no puede ser nulo o vacío.");
                return null;
            }

            string rutaArchivo = Path.Combine(_rutaDefault, $"{nombreArchivo}.json");

            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine($"El archivo {rutaArchivo} no existe.");
                return null;
            }

            try
            {
                // Leer el archivo JSON
                var jsonString = File.ReadAllText(rutaArchivo);

                // Deserializar a ObjetoDTO
                var objetoDTO = JsonSerializer.Deserialize<ObjetoDTO>(jsonString, _opciones);
                if (objetoDTO == null)
                {
                    Console.WriteLine($"Error al deserializar el archivo {rutaArchivo}: el resultado es null.");
                    return null;
                }

                // Reconstruir el objeto desde DTO
                return ReconstruirObjetoDesdeDTO(objetoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el objeto 3D: {ex.Message}");
                return null;
            }
        }

        // Filtrar y mostrar los archivos .json disponibles
        public string[] FiltrarArchivos()
        {
            string[] archivosJson = Directory.GetFiles(_rutaDefault, "*.json");

            if (archivosJson.Length == 0)
            {
                Console.WriteLine("No se encontraron archivos .json en la carpeta especificada.");
                return Array.Empty<string>();
            }

            Console.WriteLine("Archivos disponibles:");
            for (int i = 0; i < archivosJson.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {Path.GetFileName(archivosJson[i])}");
            }

            return archivosJson;
        }

        // Método auxiliar para crear directorio si no existe
        private void CrearDirectorioSiNoExiste(string rutaCompleta)
        {
            string? directorio = Path.GetDirectoryName(rutaCompleta);
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
        }

        // Convertir el Objeto a ObjetoDTO para serializarlo
        private ObjetoDTO ConvertirAObjetoDTO(Objeto objeto)
        {
            var objetoDTO = new ObjetoDTO
            {
                Posicion = new float[] { objeto.Posicion.X, objeto.Posicion.Y, objeto.Posicion.Z },
                Rotacion = new float[] { objeto.Rotacion.X, objeto.Rotacion.Y, objeto.Rotacion.Z },
                Escala = new float[] { objeto.Escala.X, objeto.Escala.Y, objeto.Escala.Z },
                Partes = new Dictionary<string, ParteDTO>()
            };

            foreach (var parteKvp in objeto.Partes)
            {
                var parteDTO = new ParteDTO
                {
                    Puntos = new Dictionary<string, PuntoDTO>(),
                    Indices = parteKvp.Value.Poligonos.Values.SelectMany(p => p.Indices).ToArray(),
                    Poligonos = ConvertirPoligonosDTO(parteKvp.Value.Poligonos)
                };

                objetoDTO.Partes.Add(parteKvp.Key, parteDTO);
            }

            return objetoDTO;
        }

        // Convertir un diccionario de poligonos a PoligonoDTO
        private Dictionary<string, PoligonoDTO> ConvertirPoligonosDTO(Dictionary<string, Poligono> poligonos)
        {
            var poligonosDTO = new Dictionary<string, PoligonoDTO>();

            foreach (var poligonoKvp in poligonos)
            {
                var poligonoDTO = new PoligonoDTO
                {
                    Puntos = poligonoKvp.Value.Puntos.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new PuntoDTO
                        {
                            Posicion = new float[] { kvp.Value.Posicion.X, kvp.Value.Posicion.Y, kvp.Value.Posicion.Z },
                            Color = new float[] { kvp.Value.Color.X, kvp.Value.Color.Y, kvp.Value.Color.Z }
                        }),
                    Indices = poligonoKvp.Value.Indices
                };

                poligonosDTO.Add(poligonoKvp.Key, poligonoDTO);
            }

            return poligonosDTO;
        }

        // Reconstruir el Objeto 3D desde un ObjetoDTO
        private Objeto ReconstruirObjetoDesdeDTO(ObjetoDTO objetoDTO)
        {
            var objeto = new Objeto(
                new Vector3(objetoDTO.Posicion[0], objetoDTO.Posicion[1], objetoDTO.Posicion[2]),
                new Vector3(objetoDTO.Rotacion[0], objetoDTO.Rotacion[1], objetoDTO.Rotacion[2]),
                new Vector3(objetoDTO.Escala[0], objetoDTO.Escala[1], objetoDTO.Escala[2])
            );

            foreach (var parteEntry in objetoDTO.Partes)
            {
                var poligonos = parteEntry.Value.Poligonos.ToDictionary(
                    p => p.Key,
                    p => new Poligono(
                        p.Value.Puntos.ToDictionary(
                            punto => punto.Key,
                            punto => new Punto(
                                new Vector3(punto.Value.Posicion[0], punto.Value.Posicion[1], punto.Value.Posicion[2]),
                                new Vector3(punto.Value.Color[0], punto.Value.Color[1], punto.Value.Color[2])
                            )
                        ),
                        p.Value.Indices
                    )
                );

                var parte = new Parte(poligonos);
                objeto.AgregarParte(parteEntry.Key, parte);
            }

            return objeto;
        }
    }
}