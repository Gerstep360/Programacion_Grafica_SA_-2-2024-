using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK_Tarea_3.Controlador;
using OpenTK_Tarea_3.Clases_Base;

namespace OpenTK_Tarea_1
{
    internal class Game : GameWindow
    {
        private camera _camera;
        private Inputs _input;
        private Escenario _escenario;
        private Renderización _renderizacion;
        private Shader _shader;

        public Game(int width, int height, string title)
           : base(GameWindowSettings.Default, new NativeWindowSettings()
           {
               ClientSize = new Vector2i(width, height),
               Title = title,
               WindowBorder = WindowBorder.Resizable,

           })
        {
            VSync = VSyncMode.On;
            _renderizacion = new Renderización();
            _camera = new camera(new Vector3(0, 0, 3));
            _input = new Inputs(_camera, this);
            _escenario = new Escenario();
            _shader = new Shader(VertexShaderSource, FragmentShaderSource);

            // Crear algunos objetos
            CrearObjetosT(10);
        }

        private void CrearObjetosT(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Vector3 posicion = new Vector3(i * 2.0f, i * 2.0f, i * 2.0f);
                Objeto objetoT = new Objeto(posicion, Vector3.Zero, Vector3.One);
                _escenario.AgregarObjeto(objetoT);
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(14 / 255.0f, 102 / 255.0f, 85 / 255.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Cargar y usar el shader
            
            _renderizacion.UsarProgramaShader(_shader.Handle);

            // Configurar buffers para todos los objetos en el escenario
            foreach (var objeto in _escenario.Objetos)
            {
                foreach (var parte in objeto.Partes)
                {
                    foreach (var poligono in parte.Poligonos)
                    {
                        _renderizacion.ConfigurarBuffers(poligono);
                    }
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _input.HandleInput(e);

            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            _escenario.DibujarEscenario(_renderizacion, view, projection);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            _renderizacion.LimpiarBuffers();
            _shader.Dispose();
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
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
            precision highp float;
            in vec3 ourColor;
            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(ourColor, 1.0);
            }";
    }
}
