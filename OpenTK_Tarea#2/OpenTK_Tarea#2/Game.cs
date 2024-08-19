using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK_Tarea_1.Clases_Base;
using OpenTK_Tarea_1.Controlador;

namespace OpenTK_Tarea_1
{
    internal class Game : GameWindow
    {
        private camera _camera;
        private Inputs _input;
        private Objeto _objeto;
        private Renderización _renderizacion;

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

            _objeto = new Objeto(new Vector3(0, 0, 0), Vector3.Zero, Vector3.One);
            _renderizacion = new Renderización();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(199 / 255.0f, 0 / 255.0f, 57 / 255.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _renderizacion.ConfigurarBuffers(_objeto);

            int shaderProgram = CreateShaderProgram(VertexShaderSource, FragmentShaderSource);
            _renderizacion.UsarProgramaShader(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _input.HandleInput(e);

            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _renderizacion.RenderizarObjeto(_objeto, view, projection);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            _renderizacion.LimpiarBuffers();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
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
                FragColor = vec4(ourColor, 1.0);
            }";
    }
}
