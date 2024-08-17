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
        private camera _camera;
        private Inputs _input;

        // Definición de vértices para la letra T
        private float[] vertices = {
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
            -0.2f, -0.5f, -0.1f,  
            };

        // Definición de colores para cada vértice
        private float[] colors = {
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
        // Definición de los índices
        private uint[] indices = {
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

        public Game(int width, int height, string title)
           : base(GameWindowSettings.Default, new NativeWindowSettings()
           {
               ClientSize = new Vector2i(width, height),
               Title = title,

               WindowBorder = WindowBorder.Resizable
           })
        {
            _camera = new camera(new Vector3(0, 0, 3));

            _input = new Inputs(_camera, this);

        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(199 / 255.0f, 0 / 255.0f, 57 / 255.0f, 1.0f);

            GL.Enable(EnableCap.DepthTest); 
            
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            
            
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            
            _colorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            
            _shaderProgram = CreateShaderProgram(VertexShaderSource, FragmentShaderSource);
            GL.UseProgram(_shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _input.HandleInput(e); 
            
            Matrix4 view = _camera.GetViewMatrix();

            
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);


            
            int viewLocation = GL.GetUniformLocation(_shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(_shaderProgram, "projection");
            int modelLocation = GL.GetUniformLocation(_shaderProgram, "model");

            
            GL.UniformMatrix4(viewLocation, false, ref view);
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            
            Matrix4 model = Matrix4.Identity; 
            GL.UniformMatrix4(modelLocation, false, ref model);

            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            
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
            
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileStatus(vertexShader);

            
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileStatus(fragmentShader);

            
            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            CheckProgramLinkStatus(shaderProgram);

            
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
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e); 
            GL.Viewport(0, 0, Size.X, Size.Y); 
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

