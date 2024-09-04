using OpenTK_Tarea_4.Clases_Base;
using OpenTK.Mathematics;
namespace OpenTK_Tarea_4.Extra
{
    internal class Administrador_Animaciones
    {
        private List<Animación> _animaciones = new List<Animación>();

        // Método para agregar animaciones a un objeto
        public void AgregarAnimacion(Objeto objeto, Vector3 posicionFinal, float velocidad, InterpoTK.TipoInterpolacion tipo)
        {
            var animacion = new Animación(objeto, posicionFinal, velocidad, tipo);
            _animaciones.Add(animacion);
        }

        // Método para actualizar todas las animaciones en cada frame
        public void ActualizarAnimaciones(bool loaded)
        {
            if (loaded)
            {
                for (int i = _animaciones.Count - 1; i >= 0; i--)
                {
                    if (_animaciones[i].ActualizarInterpolacion()) // Si la animación ha finalizado
                    {
                        _animaciones.RemoveAt(i);
                    }
                }
            }

        }

    }
}
