using OpenTK.Mathematics;
using OpenTK_Tarea_3.Clases_Base;
using System.Collections.Generic;

namespace OpenTK_Tarea_3.Clases_Base
{
    public class Objeto
    {
        public Vector3 Posicion { get; set; }
        public Vector3 Rotacion { get; set; }
        public Vector3 Escala { get; set; }
        public List<Parte> Partes { get; } = new List<Parte>();

        public Objeto(Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            Posicion = posicion;
            Rotacion = rotacion;
            Escala = escala;
            ConfigurarVertices_T();
        }

        public void AgregarParte(Parte parte)
        {
            if (parte == null) throw new ArgumentNullException(nameof(parte));
            Partes.Add(parte);
        }


        public void ConfigurarVertices_T()
        {
            float[] vertices = new float[]
            {
                // Cara frontal
                -0.5f,  0.3f,  0.1f,
                 0.5f,  0.3f,  0.1f,
                 0.5f,  0.1f,  0.1f,
                -0.5f,  0.1f,  0.1f,

                -0.2f,  0.1f,  0.1f,
                 0.2f,  0.1f,  0.1f,
                 0.2f, -0.5f,  0.1f,
                -0.2f, -0.5f,  0.1f,  

                // Cara trasera
                -0.5f,  0.3f, -0.1f,
                 0.5f,  0.3f, -0.1f,
                 0.5f,  0.1f, -0.1f,
                -0.5f,  0.1f, -0.1f,

                -0.2f,  0.1f, -0.1f,
                 0.2f,  0.1f, -0.1f,
                 0.2f, -0.5f, -0.1f,
                -0.2f, -0.5f, -0.1f
            };

            uint[] indices = new uint[]
            {
                // Cara frontal
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,    

                // Cara trasera
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12, 

                // Lados de la barra superior
                0, 1, 9, 9, 8, 0,
                1, 2, 10, 10, 9, 1,
                2, 3, 11, 11, 10, 2,
                3, 0, 8, 8, 11, 3, 

                // Lados de la barra vertical
                4, 5, 13, 13, 12, 4,
                5, 6, 14, 14, 13, 5,
                6, 7, 15, 15, 14, 6,
                7, 4, 12, 12, 15, 7,
            };

            float[] colors = new float[]
            {
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,

                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,

                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,

                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
                87 / 255.0f, 24 / 255.0f, 69 / 255.0f,
            };
            var poligono = new Poligono(vertices, colors, indices);
            var parte = new Parte();
            parte.AgregarPoligono(poligono);
            AgregarParte(parte);
        }

        public Matrix4 ObtenerMatrizModelo()
        {
            return Matrix4.CreateScale(Escala) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                   Matrix4.CreateTranslation(Posicion);
        }
    }
}
