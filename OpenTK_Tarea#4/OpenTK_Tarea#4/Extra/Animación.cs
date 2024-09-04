using OpenTK_Tarea_4.Clases_Base;
using OpenTK.Mathematics;
using OpenTK_Tarea_4.Extra;

namespace OpenTK_Tarea_4.Extra
{
    internal class Animación
    {
        private Objeto _objeto;
        private Vector3 _posicionInicial;
        private Vector3 _posicionFinal;
        private float _progresoInterpolacion;
        private float _velocidadInterpolacion;
        private InterpoTK.TipoInterpolacion _tipoInterpolacion;
        public Animación(Objeto objeto, Vector3 posicionFinal, float velocidadInterpolacion, InterpoTK.TipoInterpolacion tipoInterpolacion)
        {
            _objeto = objeto;
            _posicionInicial = objeto.Posicion;  // Obtener la posición inicial del objeto
            _posicionFinal = posicionFinal;
            _progresoInterpolacion = 0f;
            _velocidadInterpolacion = velocidadInterpolacion;
            _tipoInterpolacion = tipoInterpolacion;
        }

        public bool ActualizarInterpolacion()
        {
            // Actualizar progreso de interpolación
            _progresoInterpolacion += _velocidadInterpolacion;

            if (_progresoInterpolacion > 1f)  // Si ya alcanzó la posición final
            {
                _progresoInterpolacion = 1f;
            }

            // Interpolamos la posición del objeto
            Vector3 posicionActual = InterpoTK.Interpolar(_posicionInicial, _posicionFinal, _progresoInterpolacion, _tipoInterpolacion);

            // Actualizamos la posición del objeto
            _objeto.Posicion = posicionActual;

            // Retornar true si la interpolación terminó
            return _progresoInterpolacion >= 1f;
        }
    }
}
