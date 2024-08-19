using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK_Tarea_1.Clases_Base;
namespace OpenTK_Tarea_1.Clases_Base
{
    internal class Objeto
    {
        public Vector3 Posicion { get; set; }
        public Vector3 Rotacion { get; set; }
        public Vector3 Escala { get; set; }

        private float[] vertices = new float[0];
        private float[] colors = new float[0];
        private uint[] indices = new uint[0];


        public Objeto(Vector3 posicion, Vector3 rotacion, Vector3 escala)
        {
            Posicion = posicion;
            Rotacion = rotacion;
            Escala = escala;

            ConfigurarVertices(); // Inicializa los vértices con posiciones relativas
        }

        private void ConfigurarVertices()
        {
            vertices = new float[]
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

            indices = new uint[]
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

            colors = new float[]
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
        }

        public float[] GetVertices() => vertices;
        public uint[] GetIndices() => indices;
        public float[] GetColors() => colors;
    }
}

internal class Renderización
    {
    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _colorBufferObject;
    private int _shaderProgram;

    public Renderización()
    {
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
    }

    public void ConfigurarBuffers(Objeto objeto)
    {
        // VBO para vértices
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, objeto.GetVertices().Length * sizeof(float), objeto.GetVertices(), BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // VBO para colores
        _colorBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, objeto.GetColors().Length * sizeof(float), objeto.GetColors(), BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);

        // EBO para índices
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, objeto.GetIndices().Length * sizeof(uint), objeto.GetIndices(), BufferUsageHint.StaticDraw);
    }

    public void RenderizarObjeto(Objeto objeto, Matrix4 view, Matrix4 projection)
    {
        Matrix4 model = Matrix4.CreateScale(objeto.Escala) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(objeto.Rotacion.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(objeto.Rotacion.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(objeto.Rotacion.Z)) *
                        Matrix4.CreateTranslation(objeto.Posicion);

        int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
        int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
        int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");

        GL.UniformMatrix4(viewLocation, false, ref view);
        GL.UniformMatrix4(projectionLocation, false, ref projection);
        GL.UniformMatrix4(modelLocation, false, ref model);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, objeto.GetIndices().Length, DrawElementsType.UnsignedInt, 0);
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

