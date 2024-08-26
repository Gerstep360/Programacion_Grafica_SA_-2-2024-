using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace OpenTK_Tarea_3.Clases_Base
{
    public class Shader : IDisposable
    {
        public int Handle { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            // Compilación de shaders
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexPath);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentPath);

            // Creación del programa de shader
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            // Verificar el estado del enlace del programa
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                throw new Exception($"Error linking shader program: {infoLog}");
            }

            // Los shaders ya no son necesarios después de enlazar el programa, se pueden eliminar
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private int CompileShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            // Verificar el estado de compilación del shader
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                GL.DeleteShader(shader);
                throw new Exception($"Error compiling {type}: {infoLog}");
            }

            return shader;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
