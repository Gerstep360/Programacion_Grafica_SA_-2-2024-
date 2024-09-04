// Crear una instancia de la clase Game y ejecutar el juego
using OpenTK_Tarea_4;

using (Game game = new Game(1920, 1080, "Letra T 3D"))
{
    Console.WriteLine("Bienvenido" +
        "\n" +
        "Atajos del teclado:" +
        "\n" +
        "Q: Bloquear el mouse dentro de la ventana" +
        "\n" +
        "E: Desbloquear el mouse" +
        "\n" +
        "G: Guardar Modelo 3D" +
        "\n" +
        "C: Cargar Modelo 3D" +
        "\n" +
        "M: Modificar 3D"+
        "\n" +
        "F: Captura de Pantalla"
        );
    game.Run();
    
}