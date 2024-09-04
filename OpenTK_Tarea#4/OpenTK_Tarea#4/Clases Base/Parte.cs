using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_4.Clases_Base
{
    public class Parte : IDisposable
    {
        public Dictionary<string, Poligono> Poligonos { get; } = new Dictionary<string, Poligono>();
        public Vector3 CentroDeMasa { get; private set; }

        public Parte(Dictionary<string, Poligono> poligonos)
        {
            Poligonos = poligonos ?? throw new ArgumentNullException(nameof(poligonos));
            CalcularCentroDeMasa();
        }

        private void CalcularCentroDeMasa()
        {
            Vector3 sumaPosiciones = Vector3.Zero;
            int contadorPuntos = 0;

            foreach (var poligono in Poligonos.Values)
            {
                foreach (var punto in poligono.Puntos.Values)
                {
                    sumaPosiciones += punto.Posicion;
                    contadorPuntos++;
                }
            }

            if (contadorPuntos > 0)
            {
                CentroDeMasa = sumaPosiciones / contadorPuntos;
            }
            else
            {
                CentroDeMasa = Vector3.Zero;  // Si no hay puntos, el centro de masa es el origen
            }
        }
        public void Dispose()
        {
            foreach (var poligono in Poligonos.Values)
            {
                poligono.Dispose();
            }
            Poligonos.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
