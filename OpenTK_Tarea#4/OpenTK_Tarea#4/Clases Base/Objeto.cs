using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Tarea_4.Clases_Base
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

        public void AgregarParte(string key, Parte parte)
        {
            if (parte == null) throw new ArgumentNullException(nameof(parte));
            Partes[key] = parte;
        }

        public Matrix4 ObtenerMatrizModelo()
        {
            return Matrix4.CreateScale(Escala) *
                   Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
                   Matrix4.CreateTranslation(Posicion);
        }

        public void ConfigurarVertices_T()
        {
            var puntos = new Dictionary<string, Punto>
            {
                { "P0", new Punto(new Vector3(-0.5f,  0.3f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P1", new Punto(new Vector3(0.5f,  0.3f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P2", new Punto(new Vector3(0.5f,  0.1f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P3", new Punto(new Vector3(-0.5f,  0.1f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P4", new Punto(new Vector3(-0.2f,  0.1f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P5", new Punto(new Vector3(0.2f,  0.1f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P6", new Punto(new Vector3(0.2f, -0.5f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P7", new Punto(new Vector3(-0.2f, -0.5f,  0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P8", new Punto(new Vector3(-0.5f,  0.3f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P9", new Punto(new Vector3(0.5f,  0.3f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P10", new Punto(new Vector3(0.5f,  0.1f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P11", new Punto(new Vector3(-0.5f,  0.1f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P12", new Punto(new Vector3(-0.2f,  0.1f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P13", new Punto(new Vector3(0.2f,  0.1f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P14", new Punto(new Vector3(0.2f, -0.5f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) },
                { "P15", new Punto(new Vector3(-0.2f, -0.5f, -0.1f), new Vector3(87 / 255.0f, 24 / 255.0f, 69 / 255.0f)) }
            };

            uint[] indices = new uint[]
            {
                0, 1, 2, 2, 3, 0,
                4, 5, 6, 6, 7, 4,
                8, 9, 10, 10, 11, 8,
                12, 13, 14, 14, 15, 12,
                0, 1, 9, 9, 8, 0,
                1, 2, 10, 10, 9, 1,
                2, 3, 11, 11, 10, 2,
                3, 0, 8, 8, 11, 3,
                4, 5, 13, 13, 12, 4,
                5, 6, 14, 14, 13, 5,
                6, 7, 15, 15, 14, 6,
                7, 4, 12, 12, 15, 7,
            };

            var poligono = new Poligono(puntos, indices);
            var parte = new Parte(new Dictionary<string, Poligono> { { "PoligonoT", poligono } });

            AgregarParte("ParteT", parte);
        }
        public void ConfigurarVertices_LogoNVIDIA_Inclinado()
        {
            var puntos = new Dictionary<string, Punto>
    {
        // Parte Verde (lado izquierdo inclinado)
        { "P0", new Punto(new Vector3(-0.7f,  0.3f,  0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) }, // Verde RGB(118, 185, 0)
        { "P1", new Punto(new Vector3(-0.25f,  0.3f,  0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },
        { "P2", new Punto(new Vector3(-0.4f, -0.3f,  0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },
        { "P3", new Punto(new Vector3(-0.7f, -0.3f,  0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },

        { "P4", new Punto(new Vector3(-0.7f,  0.3f, -0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },
        { "P5", new Punto(new Vector3(-0.25f,  0.3f, -0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },
        { "P6", new Punto(new Vector3(-0.4f, -0.3f, -0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },
        { "P7", new Punto(new Vector3(-0.7f, -0.3f, -0.1f), new Vector3(118 / 255.0f, 185 / 255.0f, 0 / 255.0f)) },

        // Parte Negra (lado derecho inclinado)
        { "P8",  new Punto(new Vector3(-0.25f,  0.3f,  0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) }, // Negro RGB(0, 0, 0)
        { "P9",  new Punto(new Vector3( 0.7f,  0.3f,  0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) },
        { "P10", new Punto(new Vector3( 0.55f, -0.3f,  0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) }, // Ajuste para la inclinación más pronunciada
        { "P11", new Punto(new Vector3(-0.4f, -0.3f,  0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) },

        { "P12", new Punto(new Vector3(-0.25f,  0.3f, -0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) },
        { "P13", new Punto(new Vector3( 0.7f,  0.3f, -0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) },
        { "P14", new Punto(new Vector3( 0.55f, -0.3f, -0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) },
        { "P15", new Punto(new Vector3(-0.4f, -0.3f, -0.1f), new Vector3(0 / 255.0f, 0 / 255.0f, 0 / 255.0f)) }
    };

            uint[] indices = new uint[]
            {
        // Cara Frontal Verde
        0, 1, 2, 2, 3, 0,

        // Cara Trasera Verde
        4, 5, 6, 6, 7, 4,

        // Conectar lados verdes
        0, 1, 5, 5, 4, 0,
        1, 2, 6, 6, 5, 1,
        2, 3, 7, 7, 6, 2,
        3, 0, 4, 4, 7, 3,

        // Cara Frontal Negra
        8, 9, 10, 10, 11, 8,

        // Cara Trasera Negra
        12, 13, 14, 14, 15, 12,

        // Conectar lados negros
        8, 9, 13, 13, 12, 8,
        9, 10, 14, 14, 13, 9,
        10, 11, 15, 15, 14, 10,
        11, 8, 12, 12, 15, 11
            };

            var poligonoVerde = new Poligono(puntos, indices);
            var parteVerde = new Parte(new Dictionary<string, Poligono> { { "PoligonoVerde", poligonoVerde } });

            var poligonoNegro = new Poligono(puntos, indices);
            var parteNegra = new Parte(new Dictionary<string, Poligono> { { "PoligonoNegro", poligonoNegro } });

            AgregarParte("ParteVerde", parteVerde);
            AgregarParte("ParteNegra", parteNegra);
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
