using OpenTK.Mathematics;

namespace OpenTK_Tarea_5.Clases_Base
{
    public class Punto
    {
        public Vector3 Posicion { get; set; }
        public Vector3 Color { get; set; }

        public Punto(Vector3 posicion, Vector3 color)
        {
            Posicion = posicion;
            Color = color;
        }
    }
}
