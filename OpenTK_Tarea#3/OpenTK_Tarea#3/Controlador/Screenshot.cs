using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png; // Para soporte de PNG
using System;
using System.IO;

namespace OpenTK_Tarea_3.Controlador
{
    internal static class Screenshot
    {
        public static void SaveScreenshot(GameWindow window)
        {
            // Obtén las dimensiones de la ventana
            int width = window.Size.X;
            int height = window.Size.Y;
            byte[] pixels = new byte[width * height * 4]; // RGBA

            // Configura el viewport para asegurarte de capturar la ventana completa
            GL.Viewport(0, 0, width, height);

            // Asegúrate de que el framebuffer esté enlazado al predeterminado
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // Capturar los píxeles de la ventana de OpenGL
            GL.ReadPixels(0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

            // Cargar los datos de píxeles en una imagen usando ImageSharp
            using (var image = Image.LoadPixelData<Bgra32>(pixels, width, height))
            {
                // Invierte la imagen verticalmente
                image.Mutate(x => x.Flip(FlipMode.Vertical));

                // Define la ruta donde se guardará la imagen
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "OpenTK", "Figura T");
                Directory.CreateDirectory(folderPath); // Crea el directorio si no existe
                string filePath = Path.Combine(folderPath, $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                // Guarda la imagen en formato PNG (mejor calidad que JPEG)
                image.Save(filePath, new PngEncoder { CompressionLevel = PngCompressionLevel.DefaultCompression });

                // Alternativamente, puedes guardarla como JPEG
                // var jpegEncoder = new JpegEncoder { Quality = 100 }; // Calidad al 100%
                // image.Save(filePath.Replace(".png", ".jpg"), jpegEncoder);

                Console.WriteLine($"Screenshot guardado en: {filePath}");
            }
        }
    }
}
