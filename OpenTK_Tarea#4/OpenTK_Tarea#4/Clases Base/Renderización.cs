using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK_Tarea_4.Clases_Base;
using System;
using System.Collections.Generic;

public class Renderización : IDisposable
{
    private Dictionary<Parte, int> _vertexArrayObjects = new Dictionary<Parte, int>();
    private Dictionary<Parte, Dictionary<string, int>> _vertexBufferObjects = new Dictionary<Parte, Dictionary<string, int>>();
    private Dictionary<Parte, int> _elementBufferObjects = new Dictionary<Parte, int>();
    private Dictionary<Parte, int> _indicesCounts = new Dictionary<Parte, int>();
    private Shader _shader;

    public Renderización(Shader shader)
    {
        _shader = shader ?? throw new ArgumentNullException(nameof(shader));
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Less);
    }

    public void ConfigurarBuffers(Objeto objeto, List<string> atributos)
    {
        foreach (var parte in objeto.Partes.Values)
        {
            foreach (var poligono in parte.Poligonos.Values)
            {
                int vao = GL.GenVertexArray();
                int ebo = GL.GenBuffer();
                Dictionary<string, int> vbos = new Dictionary<string, int>();

                _vertexArrayObjects[parte] = vao;
                _elementBufferObjects[parte] = ebo;
                _indicesCounts[parte] = poligono.Indices.Length;

                GL.BindVertexArray(vao);

                int index = 0;

                foreach (var atributo in atributos)
                {
                    int vbo = GL.GenBuffer();
                    vbos[atributo] = vbo;

                    float[] datos = ObtenerDatosAtributo(poligono, atributo);

                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                    GL.BufferData(BufferTarget.ArrayBuffer, datos.Length * sizeof(float), datos, BufferUsageHint.StaticDraw);

                    GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(index);

                    index++;
                }

                _vertexBufferObjects[parte] = vbos;

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesCounts[parte] * sizeof(uint), poligono.Indices, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(0);
            }
        }
    }

    private float[] ObtenerDatosAtributo(Poligono poligono, string atributo)
    {
        float[] datos = new float[poligono.Puntos.Count * 3];
        int index = 0;

        foreach (var punto in poligono.Puntos.Values)
        {
            if (atributo == "posicion")
            {
                datos[index++] = punto.Posicion.X;
                datos[index++] = punto.Posicion.Y;
                datos[index++] = punto.Posicion.Z;
            }
            else if (atributo == "color")
            {
                datos[index++] = punto.Color.X;
                datos[index++] = punto.Color.Y;
                datos[index++] = punto.Color.Z;
            }
        }

        return datos;
    }

    public void RenderizarObjeto(Objeto objeto, Matrix4 view, Matrix4 projection)
    {
        _shader.Use();
        _shader.SetMatrix4("model", objeto.ObtenerMatrizModelo());
        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("projection", projection);

        foreach (var parte in objeto.Partes.Values)
        {
            GL.BindVertexArray(_vertexArrayObjects[parte]);
            GL.DrawElements(PrimitiveType.Triangles, _indicesCounts[parte], DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }

    public void Dispose()
    {
        foreach (var vao in _vertexArrayObjects.Values)
        {
            GL.DeleteVertexArray(vao);
        }

        foreach (var vbos in _vertexBufferObjects.Values)
        {
            foreach (var vbo in vbos.Values)
            {
                GL.DeleteBuffer(vbo);
            }
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
