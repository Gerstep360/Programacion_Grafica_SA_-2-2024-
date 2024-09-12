using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Tarea_5.Clases_Base
{
    public class Objeto : IDisposable
    {
        public Vector3 Posicion { get; set; }
        public Vector3 Rotacion { get; set; }
        public Vector3 Escala { get; set; }
        public Dictionary<string, Parte> Partes { get; } = new Dictionary<string, Parte>();

        public Objeto(Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            Posicion = posicion;
            Rotacion = rotacion;
            Escala = escala;
        }

        public Matrix4 ObtenerMatrizModelo()
        {
            return Matrix4.CreateScale(Escala) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                   Matrix4.CreateTranslation(Posicion);
        }
        // Obtiene la matriz de transformación combinada para cada parte
        public Matrix4 ObtenerMatrizParte(Parte parte)
        {
            return parte.ObtenerMatrizTransformacion() * ObtenerMatrizModelo();
        }
        public void AgregarParte(string key, Parte parte)
        {
            if (parte == null) throw new ArgumentNullException(nameof(parte));
            Partes.Add(key, parte);
        }

        public void Dispose()
        {
            foreach (var parte in Partes.Values)
            {
                parte.Dispose();  // Liberar recursos gráficos de cada parte
            }
            Partes.Clear();
        }
    }
}
