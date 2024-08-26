using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK_Tarea_2.Clases_Base;

namespace OpenTK_Tarea_1.Clases_Base
{
    public class Renderización
    {
        private int _vertexArrayObject;
        private int _shaderProgram;
        private int _vertexBufferObject;
        private int _colorBufferObject;
        private int _elementBufferObject;

        public Renderización()
        {
            GL.LoadBindings(new GLFWBindingsContext());    // Asegúrate de cargar los bindings de OpenGL antes de cualquier otra operación.
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
        }

        public void ConfigurarBuffers(Poligono poligono)
        {
            // VBO para los vértices
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, poligono.Vertices.Length * sizeof(float), poligono.Vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // VBO para los colores
            _colorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, poligono.Colors.Length * sizeof(float), poligono.Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            // EBO para los índices
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, poligono.Indices.Length * sizeof(uint), poligono.Indices, BufferUsageHint.StaticDraw);
        }

        public void RenderizarObjeto(Objeto objeto, Matrix4 view, Matrix4 projection)
        {
            Matrix4 model = objeto.ObtenerMatrizModelo();

            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");

            GL.UniformMatrix4(viewLocation, false, ref view);
            GL.UniformMatrix4(projectionLocation, false, ref projection);
            GL.UniformMatrix4(modelLocation, false, ref model);

            foreach (var parte in objeto.Partes)
            {
                foreach (var poligono in parte.Poligonos)
                {
                    ConfigurarBuffers(poligono);
                    GL.BindVertexArray(_vertexArrayObject);
                    GL.DrawElements(PrimitiveType.Triangles, poligono.Indices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }
        }

        public void UsarProgramaShader(int shaderProgram)
        {
            _shaderProgram = shaderProgram;
            GL.UseProgram(_shaderProgram);
        }

        public void LimpiarBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_colorBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shaderProgram);
        }
    }
}
