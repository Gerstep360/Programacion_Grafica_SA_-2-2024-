using OpenTK.Mathematics;
using OpenTK_Tarea_5.Clases_Base;
using System;
using System.Collections.Generic;

namespace OpenTK_Tarea_5.Extra
{
    internal class Administrar_modelos
    {
        private Renderización _renderizacion;
        private Escenario _escenario;
        private Serializador _serializador;

        private int _index;

        public Administrar_modelos(Renderización render, Escenario escena)
        {
            _renderizacion = render;
            _escenario = escena;
            _serializador = new Serializador();
            _index = 0;
        }

        public void CrearModelos(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 posicion = new Vector3(i + 1f, i + 1f, i + 1f);
                Vector3 rotacion = Vector3.Zero;
                Vector3 escala = Vector3.One;
                Objeto objetoT = ConstruirModeloT(posicion, rotacion, escala);

                string nombreObjeto = $"T_{i}";
                _escenario.AgregarObjeto(nombreObjeto, objetoT);
                Console.WriteLine($"Se creó en memoria: {nombreObjeto}");
                _escenario.actualizar_objeto(_renderizacion, objetoT);
            }
        }

        public static Objeto ConstruirModeloT(Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            Objeto objeto = new Objeto(posicion, rotacion, escala);

            // Parte horizontal (la parte superior de la T)
            var puntosHorizontal = new Dictionary<string, Punto>
    {
        { "P0", new Punto(new Vector3(-0.5f,  0.3f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P1", new Punto(new Vector3(0.5f,  0.3f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P2", new Punto(new Vector3(0.5f,  0.1f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P3", new Punto(new Vector3(-0.5f,  0.1f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P4", new Punto(new Vector3(-0.5f,  0.3f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P5", new Punto(new Vector3(0.5f,  0.3f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P6", new Punto(new Vector3(0.5f,  0.1f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P7", new Punto(new Vector3(-0.5f,  0.1f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) }
    };

            // ÍNDICES PARA CERRAR TODAS LAS CARAS DEL POLÍGONO
            uint[] indicesHorizontal = {
        // Frente y atrás
        0, 1, 2, 2, 3, 0,  // Frente
        4, 5, 6, 6, 7, 4,  // Atrás

        // Lados laterales
        0, 3, 7, 7, 4, 0,  // Lado izquierdo
        1, 2, 6, 6, 5, 1,  // Lado derecho

        // Lados superior e inferior
        0, 1, 5, 5, 4, 0,  // Superior
        3, 2, 6, 6, 7, 3   // Inferior
    };

            var poligonoHorizontal = new Poligono(puntosHorizontal, indicesHorizontal);
            var parteHorizontal = new Parte(new Dictionary<string, Poligono> { { "PoligonoHorizontal", poligonoHorizontal } });
            // Transformaciones específicas para la parte horizontal
            parteHorizontal.Posicion = Vector3.Zero;  // Posiciona más arriba
            parteHorizontal.Rotacion = Vector3.Zero;
            parteHorizontal.Escala = Vector3.One;  // Escala normal
            objeto.AgregarParte("ParteHorizontal", parteHorizontal);

            // Parte vertical (el "palo" de la T)
            var puntosVertical = new Dictionary<string, Punto>
    {
        { "P0", new Punto(new Vector3(-0.2f,  0.1f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P1", new Punto(new Vector3(0.2f,  0.1f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P2", new Punto(new Vector3(0.2f, -0.5f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P3", new Punto(new Vector3(-0.2f, -0.5f,  0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P4", new Punto(new Vector3(-0.2f,  0.1f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P5", new Punto(new Vector3(0.2f,  0.1f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P6", new Punto(new Vector3(0.2f, -0.5f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) },
        { "P7", new Punto(new Vector3(-0.2f, -0.5f, -0.1f), new Vector3(0.34f, 0.09f, 0.27f)) }
    };

            // ÍNDICES PARA CERRAR TODAS LAS CARAS DEL POLÍGONO VERTICAL
            uint[] indicesVertical = {
        // Frente y atrás
        0, 1, 2, 2, 3, 0,  // Frente
        4, 5, 6, 6, 7, 4,  // Atrás

        // Lados laterales
        0, 3, 7, 7, 4, 0,  // Lado izquierdo
        1, 2, 6, 6, 5, 1,  // Lado derecho

        // Lados superior e inferior
        0, 1, 5, 5, 4, 0,  // Superior
        3, 2, 6, 6, 7, 3   // Inferior
    };

            var poligonoVertical = new Poligono(puntosVertical, indicesVertical);
            var parteVertical = new Parte(new Dictionary<string, Poligono> { { "PoligonoVertical", poligonoVertical } });
            // Transformaciones específicas para la parte vertical
            parteVertical.Posicion = new Vector3(0, 0, 0);  // Posiciona más abajo para formar la T
            parteVertical.Rotacion = Vector3.Zero;  // Sin rotación
            parteVertical.Escala = Vector3.One;  // Escala normal
            objeto.AgregarParte("ParteVertical", parteVertical);

            return objeto;
        }
        private string SeleccionarModeloPorIndice()
        {
            // Mostrar los modelos disponibles con índices
            var modelos = _escenario.ObtenerTodosLosHashes().ToList();
            if (modelos.Count == 0)
            {
                Console.WriteLine("No hay modelos disponibles.");
                return string.Empty;
            }

            Console.WriteLine("\nModelos disponibles:");
            for (int i = 0; i < modelos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {modelos[i]}");
            }

            Console.WriteLine("\nIngrese el número del modelo que desea seleccionar:");
            string inputModelo = Console.ReadLine();

            if (int.TryParse(inputModelo, out int modeloIndex) && modeloIndex > 0 && modeloIndex <= modelos.Count)
            {
                return modelos[modeloIndex - 1]; // Devolver el nombre del modelo seleccionado
            }
            else
            {
                Console.WriteLine("Índice de modelo no válido. Intente de nuevo.");
                return string.Empty;
            }
        }

        public void CargarModelo()
        {
            var archivosDisponibles = _serializador.FiltrarArchivos(); // Obtener los archivos disponibles

            if (archivosDisponibles.Length == 0)
            {
                Console.WriteLine("No hay archivos disponibles para cargar.");
                return;
            }

            Console.WriteLine("\nIngrese el número del archivo que desea cargar:");
            string input = Console.ReadLine();

            // Validar la entrada del usuario
            if (int.TryParse(input, out int archivoIndex) && archivoIndex > 0 && archivoIndex <= archivosDisponibles.Length)
            {
                // Obtener el nombre del archivo con base en el índice
                string nombreArchivo = Path.GetFileNameWithoutExtension(archivosDisponibles[archivoIndex - 1]);

                // Cargar el modelo desde el archivo seleccionado
                var objetoCargado = _serializador.Cargar3D(nombreArchivo);
                if (objetoCargado != null)
                {
                    string nuevoNombreObjeto = GenerarNombreUnico(nombreArchivo);
                    _escenario.AgregarObjeto(nuevoNombreObjeto, objetoCargado);
                    _escenario.actualizar_objeto(_renderizacion, objetoCargado);
                    EditarObjeto(nuevoNombreObjeto, objetoCargado); // Permitir la edición del objeto cargado
                }
                else
                {
                    Console.WriteLine($"No se pudo cargar el objeto desde '{nombreArchivo}'.");
                }
            }
            else
            {
                Console.WriteLine("Entrada no válida. Por favor, ingrese un número dentro del rango.");
            }
        }


        public void GuardarModelo()
        {
            var modeloSeleccionado = SeleccionarModeloPorIndice();
            if (!string.IsNullOrEmpty(modeloSeleccionado))
            {
                _serializador.Guardar3D(modeloSeleccionado, _escenario);
                Console.WriteLine($"Modelo '{modeloSeleccionado}' guardado con éxito.");
            }
        }


        public void ModificarModelo()
        {
            var modeloSeleccionado = SeleccionarModeloPorIndice();
            if (!string.IsNullOrEmpty(modeloSeleccionado))
            {
                var objetoCargado = _escenario.ObtenerObjeto(modeloSeleccionado);
                if (objetoCargado != null)
                {
                    EditarObjeto(modeloSeleccionado, objetoCargado, true);
                }
                else
                {
                    Console.WriteLine("No se pudo cargar el objeto.");
                }
            }
        }


        // Métodos Auxiliares

        private void EditarObjeto(string nombreObjeto, Objeto objeto, bool permitirEliminar = false)
        {
            while (true)
            {
                int opcion = MostrarMenuObjeto(permitirEliminar);

                if (opcion >= 1 && opcion <= 5)
                {
                    // Modificar transformaciones del objeto
                    _escenario.Modificar_objeto(nombreObjeto, objeto, _renderizacion, opcion);
                    _escenario.actualizar_objeto(_renderizacion, objeto);
                    Console.WriteLine($"El objeto '{nombreObjeto}' ha sido actualizado en la escena.");
                }
                else if (opcion == 6 && permitirEliminar) // Eliminar objeto
                {
                    // Eliminar el objeto del escenario
                    _escenario.EliminarObjeto(nombreObjeto);
                    Console.WriteLine($"El objeto '{nombreObjeto}' ha sido eliminado.");
                    break;
                }
                else if (opcion == 7 || (!permitirEliminar && opcion == 6)) // Salir
                {
                    Console.WriteLine($"Saliendo de la edición de '{nombreObjeto}'.");
                    break;
                }

                // Pregunta si quiere continuar modificando
                if (!ConfirmarContinuacion()) break;
            }
        }

        private int MostrarMenuObjeto(bool permitirEliminar = false)
        {
            Console.WriteLine("\n¿Qué deseas hacer con el objeto cargado?");
            Console.WriteLine("1. Modificar posición");
            Console.WriteLine("2. Modificar rotación");
            Console.WriteLine("3. Modificar escala");
            Console.WriteLine("4. Modificar color de los puntos");
            Console.WriteLine("5. Modificar Partes");
            if (permitirEliminar)
            {
                Console.WriteLine("6. Eliminar objeto");
            }
            Console.WriteLine("7. Salir del menú");

            return int.Parse(Console.ReadLine() ?? "0");
        }

        private bool ConfirmarContinuacion()
        {
            string respuesta = SolicitarEntradaUsuario("\n¿Quieres seguir configurando el modelo? (s/n): ");
            return respuesta.ToLower() == "s";
        }

        private string ObtenerNombreArchivo()
        {
            _serializador.FiltrarArchivos();
            return SolicitarEntradaUsuario("Por favor, ingresa el nombre del archivo (o escribe 'exit' para salir): ").ToLower();
        }

        private string SolicitarEntradaUsuario(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? string.Empty;
        }

        private void MostrarModelos()
        {
            _serializador.FiltrarHashes(_escenario);
        }

        private string GenerarNombreUnico(string nombreBase)
        {
            string nuevoNombre;
            int intentos = 0;
            do
            {
                nuevoNombre = $"{nombreBase}[Objeto#{_index + intentos}]";
                intentos++;
            } while (_escenario.ObtenerObjeto(nuevoNombre) != null);

            _index += intentos;
            return nuevoNombre;
        }
    }
}
