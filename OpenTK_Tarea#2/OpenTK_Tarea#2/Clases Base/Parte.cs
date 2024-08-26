using OpenTK_Tarea_2.Clases_Base;
using System.Collections.Generic;

namespace OpenTK_Tarea_2.Clases_Base
{
    public class Parte
    {
        public List<Poligono> Poligonos { get; private set; }

        public Parte()
        {
            Poligonos = new List<Poligono>();
        }

        public void AgregarPoligono(Poligono poligono)
        {
            if (poligono == null) throw new ArgumentNullException(nameof(poligono));
            Poligonos.Add(poligono);
        }
    }
}
