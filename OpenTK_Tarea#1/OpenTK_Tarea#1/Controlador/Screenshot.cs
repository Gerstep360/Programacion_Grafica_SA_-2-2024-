using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;

namespace OpenTK_Tarea_1.Controlador
{
    internal static class Screenshot
    {
        public static void SaveScreenshot(GameWindow window)
        {
            int width = window.Size.X;
            int height = window.Size.Y;
            byte[] pixels = new byte[width * height * 4]; // RGBA

            // Capturar los píxeles de la ventana de OpenGL
            GL.ReadPixels(0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

            // Cargar los datos de píxeles en una imagen usando ImageSharp
            using (var image = Image.LoadPixelData<Bgra32>(pixels, width, height))
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));

                // Guardar la imagen en formato JPEG
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "OpenTK", "Figura T");
                Directory.CreateDirectory(folderPath); // Crea el directorio si no existe
                string filePath = Path.Combine(folderPath, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.jpg");

                image.Save(filePath, new JpegEncoder());
            }
        }
    }
}