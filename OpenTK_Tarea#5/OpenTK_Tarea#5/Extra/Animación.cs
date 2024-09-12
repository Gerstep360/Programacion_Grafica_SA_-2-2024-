using OpenTK_Tarea_5.Clases_Base;
using OpenTK.Mathematics;
using OpenTK_Tarea_5.Extra;
using System;

namespace OpenTK_Tarea_5.Extra
{
    internal class Animación
    {
        private Objeto _objeto;
        private Vector3 _posicionInicial;
        private Vector3 _posicionFinal;
        private float _progresoInterpolacion;
        private float _velocidadInterpolacion;
        private InterpoTK.TipoInterpolacion _tipoInterpolacion;
        private Tipo_Animacion _animacion;
        public enum Tipo_Animacion
        {
            Posicion,
            Rotacion,
            Escala
            
        }
        public Animación(Objeto objeto,Vector3 inicio, Vector3 final, float velocidadInterpolacion, InterpoTK.TipoInterpolacion tipoInterpolacion, Tipo_Animacion _anim)
        {
            _objeto = objeto;
            _animacion = _anim;
            switch (_anim)
            {
                case Tipo_Animacion.Posicion:
                    
                    _posicionInicial = objeto.Posicion = inicio; ;
                    break;
                case Tipo_Animacion.Rotacion:
                    
                    _posicionInicial = objeto.Rotacion = inicio;
                    break;
                case Tipo_Animacion.Escala:
                    
                    _posicionInicial = objeto.Escala = inicio;
                    break;
            }
              // Obtener la posición inicial del objeto
            _posicionFinal = final;
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
            switch (_animacion)
            {
                case Tipo_Animacion.Posicion:

                    _objeto.Posicion = posicionActual; ;
                    break;
                case Tipo_Animacion.Rotacion:

                    _objeto.Rotacion = posicionActual;
                    break;
                case Tipo_Animacion.Escala:

                    _objeto.Escala = posicionActual;
                    break;
            }
            

            // Retornar true si la interpolación terminó
            return _progresoInterpolacion >= 1f;
        }
    }
}
