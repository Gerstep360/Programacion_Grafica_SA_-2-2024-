using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_5.Clases_Base
{
    public class Parte : IDisposable
    {
        public Dictionary<string, Poligono> Poligonos { get; } = new Dictionary<string, Poligono>();
        public Vector3 CentroDeMasa { get; private set; }
        // Transformaciones propias de cada parte
        public Vector3 Posicion { get; set; } = Vector3.Zero;
        public Vector3 Rotacion { get; set; } = Vector3.Zero;
        public Vector3 Escala { get; set; } = Vector3.One;
        public Parte(Dictionary<string, Poligono> poligonos)
        {
            Poligonos = poligonos ?? throw new ArgumentNullException(nameof(poligonos));
            CalcularCentroDeMasa();
        }
        // Calcula la matriz de transformación para la parte
        public Matrix4 ObtenerMatrizTransformacion()
        {
            return Matrix4.CreateScale(Escala) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                   Matrix4.CreateTranslation(Posicion);
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
