using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_Tarea_5.Extra;

namespace OpenTK_Tarea_5.Clases_Base
{
    internal class Escenario : IDisposable
    {
        // Transformaciones del escenario
        public Vector3 Posicion { get; set; } = Vector3.Zero;
        public Vector3 Rotacion { get; set; } = Vector3.Zero;
        public Vector3 Escala { get; set; } = Vector3.One;

        private readonly Dictionary<string, Objeto> _objetos = new Dictionary<string, Objeto>();
        public IReadOnlyDictionary<string, Objeto> Objetos => _objetos;

        private Administrador_Animaciones _adminAnimaciones;

        public Escenario(Administrador_Animaciones anim)
        {
            _adminAnimaciones = anim;
        }

        public Matrix4 ObtenerMatrizEscenario()
        {
            return Matrix4.CreateScale(Escala) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                   Matrix4.CreateTranslation(Posicion);
        }

        public Matrix4 ObtenerMatrizObjeto(Objeto objeto)
        {
            return objeto.ObtenerMatrizModelo() * ObtenerMatrizEscenario();
        }
        public IEnumerable<string> ObtenerTodosLosHashes()
        {
            return _objetos.Keys;
        }

        public void AgregarObjeto(string key, Objeto objeto)
        {
            if (objeto == null) throw new ArgumentNullException(nameof(objeto));
            _objetos.Add(key, objeto);
        }

        public Objeto? ObtenerObjeto(string key)
        {
            if (_objetos.TryGetValue(key, out var objeto))
            {
                return objeto;
            }
            return null;
        }

        public bool EliminarObjeto(string key)
        {
            if (_objetos.TryGetValue(key, out var objeto))
            {
                objeto.Dispose();
                _objetos.Remove(key);
                return true;
            }
            return false;
        }

        public void Modificar_objeto(string key, Objeto objeto, Renderización renderizacion, int n)
        {
            bool modificado = false;
            var partes=objeto.Partes;

            if (n >= 1 && n <= 3)
            {
                Modificar_Animaciones(n, objeto, null);
                modificado = true;
            }
            else if (n == 4)
            {
                Modificar_color_puntos(objeto);
                modificado = true;
            }
            else if (n == 5)
            {
                ModificarParteMenu(objeto, renderizacion);
                modificado = true;
            }

            if (modificado)
            {
                actualizar_objeto(renderizacion, objeto);
            }
        }

        private void ModificarParteMenu(Objeto objeto, Renderización renderizacion)
        {
            Console.WriteLine("\nPartes disponibles del objeto:");
            var partes = objeto.Partes;
            int i = 1;

            // Mostrar lista de partes disponibles
            foreach (var parteKey in partes.Keys)
            {
                Console.WriteLine($"{i}: {parteKey}");
                i++;
            }

            Console.WriteLine("\nIngrese el número de la parte que desea modificar:");
            string inputParte = Console.ReadLine();

            // Convertir la entrada del usuario a un número
            if (int.TryParse(inputParte, out int parteIndex) && parteIndex > 0 && parteIndex <= partes.Count)
            {
                // Obtener la clave de la parte con base en el índice
                var parteKey = partes.Keys.ElementAt(parteIndex - 1);  // Restar 1 porque la lista empieza desde 0
                var parte = partes[parteKey];

                // Menú de opciones de modificación para la parte seleccionada
                Console.WriteLine("\nSeleccione la transformación que desea modificar:");
                Console.WriteLine("1. Modificar Posición");
                Console.WriteLine("2. Modificar Rotación");
                Console.WriteLine("3. Modificar Escala");
                Console.WriteLine("4. Eliminar Parte");

                if (int.TryParse(Console.ReadLine(), out int opcion) && opcion >= 1 && opcion <= 4)
                {
                    bool modificado = false;

                    if (opcion >= 1 && opcion <= 3)
                    {
                        Modificar_Animaciones(opcion,null,parte);
                        modificado = true;
                    }
                    else if (opcion == 4)
                    {
                        EliminarParte(objeto,parteKey);
                        modificado = true;
                    }

                    if (modificado)
                    {
                        actualizar_objeto(renderizacion, objeto);
                    }
                }
                else
                {
                    Console.WriteLine("Opción no válida.");
                }
            }
            else
            {
                Console.WriteLine("Parte no válida. Verifica el número ingresado.");
            }
        }
        private void EliminarParte(Objeto objeto, string parteKey)
        {
            if (objeto.Partes.ContainsKey(parteKey))
            {
                objeto.Partes.Remove(parteKey);
                Console.WriteLine($"Parte {parteKey} eliminada.");
            }
        }

        private Vector3 ModificarVector(Vector3 vectorOriginal)
        {
            Console.Write($"X ({vectorOriginal.X}): ");
            string? inputX = Console.ReadLine();
            Console.Write($"Y ({vectorOriginal.Y}): ");
            string? inputY = Console.ReadLine();
            Console.Write($"Z ({vectorOriginal.Z}): ");
            string? inputZ = Console.ReadLine();

            float x = string.IsNullOrEmpty(inputX) ? vectorOriginal.X : float.Parse(inputX);
            float y = string.IsNullOrEmpty(inputY) ? vectorOriginal.Y : float.Parse(inputY);
            float z = string.IsNullOrEmpty(inputZ) ? vectorOriginal.Z : float.Parse(inputZ);

            return new Vector3(x, y, z);
        }
        // Método que acepta tanto objeto como parte, haciendo que uno de ellos sea opcional
        private void Modificar_Animaciones(int tipoTransformacion,Objeto? objeto = null, Parte? parte = null)
        {
            // Asegúrate de que al menos uno de los dos esté presente
            if (objeto == null && parte == null)
            {
                throw new ArgumentException("Se debe proporcionar un objeto o una parte.");
            }

            Console.WriteLine("Ingrese los valores finales para la animación:");

            Vector3 inicio;
            Vector3 final;

            // Si estamos modificando un Objeto
            if (objeto != null)
            {
                inicio = ObtenerTransformacion(objeto, tipoTransformacion);
            }
            // Si estamos modificando una Parte
            else if (parte != null)
            {
                inicio = ObtenerTransformacion(parte, tipoTransformacion);
            }
            else
            {
                throw new InvalidOperationException("No se puede realizar la transformación.");
            }

            Console.WriteLine("Ingrese los valores finales:");
            final = ModificarVector(inicio);

            Console.Write("Velocidad de animación: ");
            float velocidad = float.Parse(Console.ReadLine());

            Animación.Tipo_Animacion tipoAnimacion = tipoTransformacion switch
            {
                1 => Animación.Tipo_Animacion.Posicion,
                2 => Animación.Tipo_Animacion.Rotacion,
                3 => Animación.Tipo_Animacion.Escala,
                _ => throw new InvalidOperationException("Transformación no válida")
            };

            // Si estamos trabajando con un objeto
            if (objeto != null)
            {
                _adminAnimaciones.AgregarAnimacion(inicio, final, velocidad, InterpoTK.TipoInterpolacion.Cubica, tipoAnimacion, objeto, null);
            }
            // Si estamos trabajando con una parte
            else if (parte != null)
            {
                _adminAnimaciones.AgregarAnimacion(inicio, final, velocidad, InterpoTK.TipoInterpolacion.Cubica, tipoAnimacion, null, parte);
            }
        }

        // Método para obtener la transformación dependiendo del tipo de transformación (posición, rotación o escala)
        private Vector3 ObtenerTransformacion(dynamic target, int tipoTransformacion)
        {
            return tipoTransformacion switch
            {
                1 => target.Posicion,
                2 => target.Rotacion,
                3 => target.Escala,
                _ => throw new InvalidOperationException("Transformación no válida")
            };
        }


        public void Modificar_color_puntos(Objeto obj)
        {
            foreach (var parte in obj.Partes.Values)
            {
                foreach (var poligono in parte.Poligonos.Values)
                {
                    foreach (var puntoEntry in poligono.Puntos)
                    {
                        string hash = puntoEntry.Key;
                        Punto punto = puntoEntry.Value;

                        Console.WriteLine($"Modificar color del punto {hash} (dejar en blanco para mantener el valor predeterminado):");
                        punto.Color = ModificarVector(punto.Color);
                    }
                }
            }
        }

        public void DibujarEscenario(Renderización renderizacion, Matrix4 view, Matrix4 projection)
        {
            var matrizEscenario = ObtenerMatrizEscenario();

            foreach (var objeto in _objetos.Values)
            {
                var matrizObjeto = objeto.ObtenerMatrizModelo() * matrizEscenario;
                renderizacion.RenderizarObjeto(objeto, matrizObjeto, view, projection);
            }
        }

        public void actualizar_objeto(Renderización renderizacion, Objeto objeto)
        {
            List<string> atributos = new List<string> { "posicion", "color" };
            renderizacion.ConfigurarBuffers(objeto, atributos);
        }

        public void Dispose()
        {
            foreach (var objeto in _objetos.Values)
            {
                objeto.Dispose();
            }
            _objetos.Clear();
        }
    }
}
