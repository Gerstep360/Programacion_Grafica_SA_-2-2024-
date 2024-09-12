using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_Tarea_4.Extra;

namespace OpenTK_Tarea_4.Clases_Base
{
    internal class Escenario : IDisposable
    {
        private readonly Dictionary<string, Objeto> _objetos = new Dictionary<string, Objeto>();
        public IReadOnlyDictionary<string, Objeto> Objetos => _objetos;
        private Administrador_Animaciones _adminAnimaciones;

        public Escenario(Administrador_Animaciones anim)
        {
            _adminAnimaciones = anim;
        }

        public void AgregarObjeto(string key, Objeto objeto)
        {
            if (objeto == null) throw new ArgumentNullException(nameof(objeto));
            _objetos.Add(key, objeto);
        }
        public IEnumerable<string> ObtenerTodosLosHashes()
        {
            return _objetos.Keys;
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
                // Libera los recursos del objeto antes de eliminarlo
                objeto.Dispose();
                _objetos.Remove(key);
                return true;
            }
            return false;
        }
        public void Modificar_objeto(string key, Objeto objeto, Renderización renderizacion, int n)
        {
            bool modificado=false;
            if ((n>=1) && (n<=3))
            {
                //Modificar_Tranformaciones(n, objeto);
                Modificar_Animaciones(objeto, n);
                modificado = true;
            }
            else if (n==4)
            {
                Modificar_color_puntos(objeto);
                modificado = true;
            }
            else
            {
                Console.WriteLine("\n¡¡¡Ingrese un numero valido!!!");
            }
            if (modificado)
            {
                actualizar_objeto(renderizacion, objeto);
            }
            
        }
        public void Modificar_Tranformaciones(int n, Objeto obj)
        {
            switch (n)
            {
                case 1:
                    Console.WriteLine("Modificar Posición (dejar en blanco para mantener los valores predeterminados):");
                    obj.Posicion = ModificarVector(obj.Posicion);
                    break;
                case 2:
                    Console.WriteLine("Modificar Rotación (dejar en blanco para mantener los valores predeterminados):");
                    obj.Rotacion = ModificarVector(obj.Rotacion);
                    break;
                case 3:
                    Console.WriteLine("Modificar Escala (dejar en blanco para mantener los valores predeterminados):");
                    obj.Escala = ModificarVector(obj.Escala);
                    break;
            }

        }
        private void Modificar_Animaciones(Objeto objeto, int tipoTransformacion)
        {
            Console.WriteLine("Ingrese los valores finales para la animación:");

            Vector3 inicio;
            Vector3 final;

            // Selecciona la transformación según el tipo (posición, rotación, escala)
            switch (tipoTransformacion)
            {
                case 1: // Modificar Posición
                    inicio = objeto.Posicion;  // Usa la posición actual como inicio
                    Console.WriteLine("Posición final:");
                    final = ModificarVector(inicio);  // Solicita los valores finales
                    break;

                case 2: // Modificar Rotación
                    inicio = objeto.Rotacion;  // Usa la rotación actual como inicio
                    Console.WriteLine("Rotación final:");
                    final = ModificarVector(inicio);  // Solicita los valores finales
                    break;

                case 3: // Modificar Escala
                    inicio = objeto.Escala;  // Usa la escala actual como inicio
                    Console.WriteLine("Escala final:");
                    final = ModificarVector(inicio);  // Solicita los valores finales
                    break;

                default:
                    throw new InvalidOperationException("Transformación no válida");
            }

            Console.Write("Velocidad de animación: ");
            float velocidad = float.Parse(Console.ReadLine());

            // Tipo de animación según el valor de 'tipoTransformacion'
            Animación.Tipo_Animacion tipoAnimacion = tipoTransformacion switch
            {
                1 => Animación.Tipo_Animacion.Posicion,
                2 => Animación.Tipo_Animacion.Rotacion,
                3 => Animación.Tipo_Animacion.Escala,
                _ => throw new InvalidOperationException("Transformación no válida")
            };

            // Agregar la animación al objeto
            _adminAnimaciones.AgregarAnimacion(objeto, inicio, final, velocidad, InterpoTK.TipoInterpolacion.Cubica, tipoAnimacion);
        }
        public void Modificar_color_puntos(Objeto obj)
        {
            foreach (var parte in obj.Partes.Values)
            {
                foreach (var poligono in parte.Poligonos.Values)
                {
                    foreach (var puntoEntry in poligono.Puntos)
                    {
                        string hash = puntoEntry.Key;  // Identificador del punto
                        Punto punto = puntoEntry.Value;

                        Console.WriteLine($"Modificar color del punto {hash} (dejar en blanco para mantener el valor predeterminado):");
                        punto.Color = ModificarVector(punto.Color);
                    }
                }
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
        public void DibujarEscenario(Renderización renderizacion, Matrix4 view, Matrix4 projection)
        {
            foreach (var objeto in _objetos.Values)
            {
                renderizacion.RenderizarObjeto(objeto, view, projection);
            }
        }

        public void actualizar_objeto(Renderización renderizacion, Objeto objeto)
        {
            List<string> atributos = new List<string> { "posicion", "color" };
            renderizacion.ConfigurarBuffers(objeto, atributos);
        }

        public void Dispose()
        {
            // Liberar recursos de todos los objetos en el escenario
            foreach (var objeto in _objetos.Values)
            {
                objeto.Dispose();
            }
            _objetos.Clear();
        }
    }
}
