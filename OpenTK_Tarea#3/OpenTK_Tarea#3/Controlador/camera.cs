using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3.Controlador
{
    internal class camera
    {
        private Vector3 _front = -Vector3.UnitZ; // Dirección inicial de la cámara
        private Vector3 _up = Vector3.UnitY;     // Dirección hacia arriba
        private Vector3 _right = Vector3.UnitX;  // Dirección a la derecha
        public bool bloquear_raton=true;
        public Vector3 Position { get; private set; } // Posición de la cámara
        public float Yaw { get; private set; } = -90.0f; // Rotación horizontal
        public float Pitch { get; private set; } = 0.0f; // Rotación vertical
        public float Speed { get; set; } = 3.5f;        // Velocidad de movimiento
        public float Sensitivity { get; set; } = 0.5f;  // Sensibilidad del mouse
        public camera(Vector3 position)
        {
            Position = position;
        }
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public void ProcessKeyboardInput(Vector3 direction, float deltaTime)
        {
            float velocity = Speed * deltaTime;
            if (direction.Z == 1) // Mover hacia adelante
            {
                Position += _front * velocity;
            }
                
            if (direction.Z == -1) // Mover hacia atrás
            {
                Position -= _front * velocity;
            }
                
            if (direction.X == 1) // Mover hacia la derecha
            {
                Position += _right * velocity;
            }
                
            if (direction.X == -1) // Mover hacia la izquierda
            {
                Position -= _right * velocity;
            }
                
            if(direction.Y == 1)
            {
                Position += _up * velocity;
            }
                
            if (direction.Y == -1)
            {
                Position -= _up * velocity;
            }
                
        }

        public void ProcessMouseMovement(float xOffset, float yOffset, bool constrainPitch = true)
        {
            if (!bloquear_raton)
            {
                xOffset *= Sensitivity;
                yOffset *= Sensitivity;

                Yaw += xOffset;
                Pitch -= yOffset; // Invertir el yOffset porque las coordenadas Y en la ventana aumentan hacia abajo

                if (constrainPitch)
                {
                    Pitch = MathHelper.Clamp(Pitch, -89.0f, 89.0f);
                }

                UpdateCameraVectors();
            }
            
        }

        private void UpdateCameraVectors()
        {
            _front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            _front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            _front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, _up)); // Re-calcular el vector de la derecha
        }
    }
}
