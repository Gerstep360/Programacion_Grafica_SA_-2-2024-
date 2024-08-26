using OpenTK.Mathematics;

namespace OpenTK_Tarea_3.Clases_Base
{
    public class Poligono
    {
        public float[] Vertices { get; }
        public float[] Colors { get; }
        public uint[] Indices { get; }

        public Poligono(float[] vertices, float[] colors, uint[] indices)
        {
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            Colors = colors ?? throw new ArgumentNullException(nameof(colors));
            Indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }
    }
}
