using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK_Tarea_4.Controlador;
using OpenTK_Tarea_4.Clases_Base;
using OpenTK_Tarea_4.Extra;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace OpenTK_Tarea_4
{
    internal class Game : GameWindow
    {
        private camera _camera;
        private Inputs _input;
        private Escenario _escenario;
        private Renderización _renderizacion;
        private Shader _shader;
        private Administrar_modelos _modelos;
        private Administrador_Animaciones _animaciones;
        private bool isLoaded = false;
        public Game(int width, int height, string title)
           : base(GameWindowSettings.Default, new NativeWindowSettings()
           {
               ClientSize = new Vector2i(width, height),
               Title = title,
               WindowBorder = WindowBorder.Resizable,
           })
        {
            VSync = VSyncMode.On;

            _camera = new camera(new Vector3(0, 2, 10));
            _escenario = new Escenario();

            _shader = new Shader();
            _renderizacion = new Renderización(_shader);
            _animaciones = new Administrador_Animaciones();
            _modelos = new Administrar_modelos(_renderizacion, _escenario, _animaciones);

            _input = new Inputs(_camera, this, _modelos);
            // Crear y configurar objetos
            _modelos.CrearModelos(4);
            isLoaded = true;
        }

        

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(14 / 255.0f, 102 / 255.0f, 85 / 255.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _input.HandleInput(e);
            _animaciones.ActualizarAnimaciones(isLoaded);       
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
            base.OnUnload();
            _escenario.Dispose();  // Liberar recursos de todos los objetos
            _renderizacion.Dispose();  // Liberar recursos de OpenGL en la clase Renderización
            _shader.Dispose();  // Liberar recursos del Shader
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

    }
}
