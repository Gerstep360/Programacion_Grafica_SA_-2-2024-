using System.Collections.Generic;

namespace OpenTK_Tarea_5.Clases_Base
{
    public class Poligono : IDisposable
    {
        public Dictionary<string, Punto> Puntos { get; } = new Dictionary<string, Punto>();
        public uint[] Indices { get; }

        public Poligono(Dictionary<string, Punto> puntos, uint[] indices)
        {
            Puntos = puntos ?? throw new ArgumentNullException(nameof(puntos));
            Indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
