using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK_Tarea_1.Controlador;


namespace OpenTK_Tarea_1
{
    internal class Game: GameWindow
    {
        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _colorBufferObject;
        private int _shaderProgram;
        private float _rotationAngle;
        private camera _camera;
        private Inputs _input;

        // Definición de vértices para la letra T
        private float[] vertices = {
    // Cara frontal
    -0.5f,  0.3f,  0.1f,  // Vértice 0 (izquierda arriba)
     0.5f,  0.3f,  0.1f,  // Vértice 1 (derecha arriba)
     0.5f,  0.1f,  0.1f,  // Vértice 2 (derecha centro)
    -0.5f,  0.1f,  0.1f,  // Vértice 3 (izquierda centro)

    -0.2f,  0.1f,  0.1f,  // Vértice 4 (centro centro)
     0.2f,  0.1f,  0.1f,  // Vértice 5 (derecha centro)
     0.2f, -0.5f,  0.1f,  // Vértice 6 (derecha abajo)
    -0.2f, -0.5f,  0.1f,  // Vértice 7 (izquierda abajo)

    // Cara trasera
    -0.5f,  0.3f, -0.1f,  // Vértice 8
     0.5f,  0.3f, -0.1f,  // Vértice 9
     0.5f,  0.1f, -0.1f,  // Vértice 10
    -0.5f,  0.1f, -0.1f,  // Vértice 11

    -0.2f,  0.1f, -0.1f,  // Vértice 12
     0.2f,  0.1f, -0.1f,  // Vértice 13
     0.2f, -0.5f, -0.1f,  // Vértice 14
    -0.2f, -0.5f, -0.1f,  // Vértice 15
};

        // Definición de colores para cada vértice
        private float[] colors = {
                // Colores para la cara frontal (Rojo para la barra superior y Verde para la barra vertical)
                1.0f, 1.0f, 1.0f, // Vértice 0
                1.0f, 1.0f, 1.0f, // Vértice 1
                1.0f, 1.0f, 1.0f, // Vértice 2
                1.0f, 1.0f, 1.0f, // Vértice 3

                1.0f, 1.0f, 1.0f, // Vértice 4
                1.0f, 1.0f, 1.0f, // Vértice 5
                1.0f, 1.0f, 1.0f, // Vértice 6
                1.0f, 1.0f, 1.0f, // Vértice 7

                // Colores para la cara trasera (Azul para la barra superior y Amarillo para la barra vertical)
                1.0f, 1.0f, 1.0f, // Vértice 8
                1.0f, 1.0f, 1.0f, // Vértice 9
                1.0f, 1.0f, 1.0f, // Vértice 10
                1.0f, 1.0f, 1.0f, // Vértice 11

                1.0f, 1.0f, 1.0f, // Vértice 12
                1.0f, 1.0f, 1.0f, // Vértice 13
                1.0f, 1.0f, 1.0f, // Vértice 14
                1.0f, 1.0f, 1.0f, // Vértice 15
            };
        // Definición de los índices
        private uint[] indices = {
    // Cara frontal
    0, 1, 2, 2, 3, 0,    // Barra superior
    4, 5, 6, 6, 7, 4,    // Barra vertical

    // Cara trasera
    8, 9, 10, 10, 11, 8, // Barra superior
    12, 13, 14, 14, 15, 12, // Barra vertical

    // Lados de la barra superior
    0, 1, 9, 9, 8, 0, // Lado superior
    1, 2, 10, 10, 9, 1, // Lado derecho
    2, 3, 11, 11, 10, 2, // Lado inferior
    3, 0, 8, 8, 11, 3, // Lado izquierdo

    // Lados de la barra vertical
    4, 5, 13, 13, 12, 4, // Lado superior
    5, 6, 14, 14, 13, 5, // Lado derecho
    6, 7, 15, 15, 14, 6, // Lado inferior
    7, 4, 12, 12, 15, 7, // Lado izquierdo
};

        public Game(int width, int height, string title)
             : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
         ClientSize = new OpenTK.Mathematics.Vector2i(width, height),
         Title = title
         })
        {
            // Inicializamos la cámara en la posición (0, 0, 3)
            _camera = new camera(new Vector3(0, 0, 3));

            // Inicializamos el controlador de inputs
            _input = new Inputs(_camera, this);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Enable(EnableCap.DepthTest); // Habilitar test de profundidad
            // Genera y enlaza los buffers
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Buffer de vértices
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Buffer de colores
            _colorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            // Buffer de índices
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Compilar y enlazar shaders
            _shaderProgram = CreateShaderProgram(VertexShaderSource, FragmentShaderSource);
            GL.UseProgram(_shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _input.HandleInput(e); // Maneja el input de la cámara

            // Crear la matriz de vista desde la cámara
            Matrix4 view = _camera.GetViewMatrix();

            // Crear la matriz de proyección (perspectiva)
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);

            // Obtener la ubicación de las variables uniformes en el shader
            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");

            // Pasar las matrices de vista y proyección al shader
            GL.UniformMatrix4(viewLocation, false, ref view);
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            // Pasar la matriz de rotación (model)
            Matrix4 model = Matrix4.CreateRotationY(_rotationAngle); // Usar la rotación actual
            GL.UniformMatrix4(modelLocation, false, ref model);

            // Limpiar la pantalla
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Dibujar el objeto
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_colorBufferObject);
            GL.DeleteBuffer(_elementBufferObject);

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(_vertexArrayObject);

            GL.DeleteProgram(_shaderProgram);

            base.OnUnload();
        }

        private int CreateShaderProgram(string vertexSource, string fragmentSource)
        {
            // Crear y compilar el Vertex Shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileStatus(vertexShader);

            // Crear y compilar el Fragment Shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileStatus(fragmentShader);

            // Linkear ambos shaders en un programa
            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            CheckProgramLinkStatus(shaderProgram);

            // Limpiar shaders que ya no se necesitan
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return shaderProgram;
        }

        private void CheckShaderCompileStatus(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new System.Exception($"Error compiling shader: {infoLog}");
            }
        }

        private void CheckProgramLinkStatus(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new System.Exception($"Error linking program: {infoLog}");
            }
        }

        // Definición de shaders como strings
        private const string VertexShaderSource = @"
    #version 330 core
    layout(location = 0) in vec3 aPosition;
    layout(location = 1) in vec3 aColor;

    uniform mat4 model;
    uniform mat4 view;
    uniform mat4 projection;

    out vec3 ourColor;

    void main()
    {
        gl_Position = projection * view * model * vec4(aPosition, 1.0);
        ourColor = aColor;
    }";


        private const string FragmentShaderSource = @"
            #version 330 core
            in vec3 ourColor;

            out vec4 FragColor;

            void main()
                {
                    FragColor = vec4(ourColor, 1.0); // Aplicar el color
                }";
        }
    }

