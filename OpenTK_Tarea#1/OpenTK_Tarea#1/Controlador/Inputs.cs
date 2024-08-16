﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
namespace OpenTK_Tarea_1.Controlador
{
    internal class Inputs
    {
        private camera _camera;
        private GameWindow _window;

        public Inputs(camera camera, GameWindow window)
        {
            _camera = camera;
            _window = window;
            _window.CursorState = CursorState.Grabbed; // Bloquear el cursor dentro de la ventana
        }
        public void HandleInput(FrameEventArgs e)
        {
            // Movimiento de la cámara con teclas
            Vector3 direction = Vector3.Zero;
            if (_window.KeyboardState.IsKeyDown(Keys.W))
                direction.Z += 1;
            if (_window.KeyboardState.IsKeyDown(Keys.S))
                direction.Z -= 1;
            if (_window.KeyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (_window.KeyboardState.IsKeyDown(Keys.D))
                direction.X += 1;

            _camera.ProcessKeyboardInput(direction, (float)e.Time);

            // Movimiento de la cámara con el mouse
            var mouseState = _window.MouseState;
            _camera.ProcessMouseMovement(mouseState.Delta.X, mouseState.Delta.Y);
        }
    }
}
