using OpenTK.Mathematics;
using OpenTK_Tarea_4.Clases_Base;
using System;
using System.Collections.Generic;

namespace OpenTK_Tarea_4.Extra
{
    internal class Administrar_modelos
    {
        private Renderización _renderizacion;
        private Escenario _escenario;
        private Serializador _serializador;
        private Administrador_Animaciones _adminAnimaciones;
        private int _index;

        public Administrar_modelos(Renderización render, Escenario escena, Administrador_Animaciones anim)
        {
            _renderizacion = render;
            _escenario = escena;
            _adminAnimaciones = anim;
            _serializador = new Serializador();
            _index = 0;
        }

        public void CrearModelos(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 posicion = new Vector3(i + 1f, i + 1f, i + 1f);
                Objeto objetoT = new Objeto(posicion, Vector3.Zero, Vector3.One);
                objetoT.ConfigurarVertices_LogoNVIDIA_Inclinado();

                string nombreObjeto = $"Nvidia_RTX_{i}";
                _escenario.AgregarObjeto(nombreObjeto, objetoT);
                Console.WriteLine($"Se creó en memoria: {nombreObjeto}");

                _adminAnimaciones.AgregarAnimacion(objetoT, new Vector3(i + 0, i + 0, i / -5), 0.01f, InterpoTK.TipoInterpolacion.Cuartica);
                _escenario.actualizar_objeto(_renderizacion, objetoT);
            }
        }

        public void CargarModelo()
        {
            while (true)
            {
                string nombreArchivo = ObtenerNombreArchivo();
                if (nombreArchivo == "exit")
                    break;

                var objetoCargado = _serializador.Cargar3D(nombreArchivo);
                if (objetoCargado != null)
                {
                    string nuevoNombreObjeto = GenerarNombreUnico(nombreArchivo);
                    _escenario.AgregarObjeto(nuevoNombreObjeto, objetoCargado);
                    _escenario.actualizar_objeto(_renderizacion, objetoCargado);
                    EditarObjeto(nuevoNombreObjeto, objetoCargado);
                }
                else
                {
                    Console.WriteLine($"No se pudo cargar el objeto desde '{nombreArchivo}'.");
                }
            }
        }

        public void GuardarModelo()
        {
            MostrarModelos();
            string nombreArchivo = SolicitarEntradaUsuario("Por favor, ingresa el nombre del modelo: ");
            _serializador.Guardar3D(nombreArchivo, _escenario);
        }

        public void ModificarModelo()
        {
            MostrarModelos();
            string nombreArchivo = SolicitarEntradaUsuario("Por favor, ingresa el nombre del modelo: ");
            var objetoCargado = _escenario.ObtenerObjeto(nombreArchivo);
            EditarObjeto(nombreArchivo, objetoCargado, true);
        }

        // Métodos Auxiliares

        private void EditarObjeto(string nombreObjeto, Objeto objeto, bool permitirEliminar = false)
        {
            while (true)
            {
                int opcion = MostrarMenuObjeto(permitirEliminar);

                if (opcion >= 1 && opcion <= 4)
                {
                    _escenario.Modificar_objeto(nombreObjeto, objeto, _renderizacion, opcion);
                    _escenario.actualizar_objeto(_renderizacion, objeto);
                    Console.WriteLine($"El objeto '{nombreObjeto}' ha sido actualizado en la escena.");
                }
                else if (opcion == 5 && permitirEliminar)
                {
                    _escenario.EliminarObjeto(nombreObjeto);
                    Console.WriteLine($"El objeto '{nombreObjeto}' ha sido eliminado.");
                    break;
                }
                else if (opcion == 5 || (opcion == 6 && permitirEliminar))
                {
                    Console.WriteLine($"Saliendo de la edición de '{nombreObjeto}'.");
                    break;
                }

                if (!ConfirmarContinuacion())
                    break;
            }
        }

        private int MostrarMenuObjeto(bool permitirEliminar = false)
        {
            Console.WriteLine("\n¿Qué deseas hacer con el objeto cargado?");
            Console.WriteLine("1. Modificar posición");
            Console.WriteLine("2. Modificar rotación");
            Console.WriteLine("3. Modificar escala");
            Console.WriteLine("4. Modificar color de los puntos");
            if (permitirEliminar)
            {
                Console.WriteLine("5. Eliminar objeto");
                Console.WriteLine("6. Salir del menú");
            }
            else
            {
                Console.WriteLine("5. Salir del menú");
            }

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
