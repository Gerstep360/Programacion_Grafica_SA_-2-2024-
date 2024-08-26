using OpenTK.Mathematics;

namespace OpenTK_Tarea_2.Clases_Base
{
    public class Poligono
    {
        public float[] Vertices { get; private set; }
        public float[] Colors { get; private set; }
        public uint[] Indices { get; private set; }

        public Poligono(float[] vertices, float[] colors, uint[] indices)
        {
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            Colors = colors ?? throw new ArgumentNullException(nameof(colors));
            Indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }
    }
}