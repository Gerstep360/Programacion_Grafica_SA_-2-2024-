using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK_Tarea_3.Clases_Base;

namespace OpenTK_Tarea_3.Clases_Base
{
    public class Renderización
    {
        private int _vertexArrayObject;
        private int _shaderProgram;
        private int _vertexBufferObject;
        private int _colorBufferObject;
        private int _elementBufferObject;
        private int _indicesCount;

        public Renderización()
        {
            GL.LoadBindings(new GLFWBindingsContext());
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
        }

        public void ConfigurarBuffers(Poligono poligono)
        {
            _indicesCount = poligono.Indices.Length;

            // Combina los datos de vértices y colores en un solo VBO para minimizar las vinculaciones.
            var vertexDataSize = poligono.Vertices.Length * sizeof(float);
            var colorDataSize = poligono.Colors.Length * sizeof(float);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexDataSize + colorDataSize, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertexDataSize, poligono.Vertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(vertexDataSize), colorDataSize, poligono.Colors);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), vertexDataSize);
            GL.EnableVertexAttribArray(1);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesCount * sizeof(uint), poligono.Indices, BufferUsageHint.StaticDraw);
        }

        public void RenderizarObjeto(Objeto objeto, Matrix4 view, Matrix4 projection)
        {
            // Usa el programa de shaders si no está ya en uso
            if (GL.GetInteger(GetPName.CurrentProgram) != _shaderProgram)
            {
                GL.UseProgram(_shaderProgram);
            }

            // Envía las matrices solo si hay cambios significativos para evitar enviar datos innecesarios.
            var model = objeto.ObtenerMatrizModelo();
            EnviarMatrizUniforme("view", ref view);
            EnviarMatrizUniforme("projection", ref projection);
            EnviarMatrizUniforme("model", ref model);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indicesCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private void EnviarMatrizUniforme(string nombreUniforme, ref Matrix4 matriz)
        {
            int location = GL.GetUniformLocation(_shaderProgram, nombreUniforme);
            GL.UniformMatrix4(location, false, ref matriz);
        }

        public void UsarProgramaShader(int shaderProgram)
        {
            if (_shaderProgram != shaderProgram)
            {
                _shaderProgram = shaderProgram;
                GL.UseProgram(_shaderProgram);
            }
        }

        public void LimpiarBuffers()
        {
            if (_vertexBufferObject != 0)
            {
                GL.DeleteBuffer(_vertexBufferObject);
                _vertexBufferObject = 0;
            }
            if (_colorBufferObject != 0)
            {
                GL.DeleteBuffer(_colorBufferObject);
                _colorBufferObject = 0;
            }
            if (_elementBufferObject != 0)
            {
                GL.DeleteBuffer(_elementBufferObject);
                _elementBufferObject = 0;
            }
            if (_vertexArrayObject != 0)
            {
                GL.DeleteVertexArray(_vertexArrayObject);
                _vertexArrayObject = 0;
            }
            if (_shaderProgram != 0)
            {
                GL.DeleteProgram(_shaderProgram);
                _shaderProgram = 0;
            }
        }
    }
}
