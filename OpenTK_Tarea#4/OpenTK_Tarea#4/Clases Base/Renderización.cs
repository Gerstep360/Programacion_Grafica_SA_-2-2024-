using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenTK_Tarea_4.Clases_Base
{
    public class Renderización : IDisposable
    {
        private Dictionary<Objeto, int> _vertexArrayObjects = new Dictionary<Objeto, int>();
        private Dictionary<Objeto, int> _vertexBufferObjects = new Dictionary<Objeto, int>();
        private Dictionary<Objeto, int> _elementBufferObjects = new Dictionary<Objeto, int>();
        private Dictionary<Objeto, int> _indicesCounts = new Dictionary<Objeto, int>();

        private Shader _shader;

        public Renderización(Shader shader)
        {
            _shader = shader ?? throw new ArgumentNullException(nameof(shader));
        }

        public void ConfigurarBuffers(Objeto objeto, Poligono poligono)
        {
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            _vertexArrayObjects[objeto] = vao;
            _vertexBufferObjects[objeto] = vbo;
            _elementBufferObjects[objeto] = ebo;
            _indicesCounts[objeto] = poligono.Indices.Length;

            GL.BindVertexArray(vao);

            var vertexDataSize = poligono.Puntos.Count * 3 * sizeof(float);
            var colorDataSize = poligono.Puntos.Count * 3 * sizeof(float);

            float[] vertices = new float[poligono.Puntos.Count * 3];
            float[] colors = new float[poligono.Puntos.Count * 3];

            int index = 0;
            foreach (var punto in poligono.Puntos.Values)
            {
                vertices[index] = punto.Posicion.X;
                vertices[index + 1] = punto.Posicion.Y;
                vertices[index + 2] = punto.Posicion.Z;

                colors[index] = punto.Color.X;
                colors[index + 1] = punto.Color.Y;
                colors[index + 2] = punto.Color.Z;

                index += 3;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexDataSize + colorDataSize, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertexDataSize, vertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(vertexDataSize), colorDataSize, colors);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), vertexDataSize);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesCounts[objeto] * sizeof(uint), poligono.Indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0); // Desenlazar VAO para evitar cambios accidentales
        }

        public void RenderizarObjeto(Objeto objeto, Matrix4 view, Matrix4 projection)
        {
            _shader.Use();

            // Enviar las matrices al shader
            _shader.SetMatrix4("model", objeto.ObtenerMatrizModelo());
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(_vertexArrayObjects[objeto]);
            GL.DrawElements(PrimitiveType.Triangles, _indicesCounts[objeto], DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public void Dispose()
        {
            foreach (var vao in _vertexArrayObjects.Values)
            {
                GL.DeleteVertexArray(vao);
            }
            foreach (var vbo in _vertexBufferObjects.Values)
            {
                GL.DeleteBuffer(vbo);
            }
            foreach (var ebo in _elementBufferObjects.Values)
            {
                GL.DeleteBuffer(ebo);
            }

            _vertexArrayObjects.Clear();
            _vertexBufferObjects.Clear();
            _elementBufferObjects.Clear();
            _indicesCounts.Clear();
        }
    }
}