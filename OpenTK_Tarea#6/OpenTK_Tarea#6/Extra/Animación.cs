using OpenTK_Tarea_5.Clases_Base;
using OpenTK.Mathematics;
using OpenTK_Tarea_5.Extra;
using System;

namespace OpenTK_Tarea_5.Extra
{
    internal class Animación
    {
        private Objeto _objeto;
        private Parte _parte;
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
        public Animación(Vector3 inicio, Vector3 final, float velocidadInterpolacion, InterpoTK.TipoInterpolacion tipoInterpolacion, Tipo_Animacion tipoAnimacion, Objeto? objeto = null, Parte? parte = null)
        {
            _objeto = objeto;
            _parte = parte;
            _animacion = tipoAnimacion;

            // Asignar valores iniciales dependiendo si es un Objeto o una Parte
            if (objeto != null)
            {
                _posicionInicial = ObtenerTransformacionInicial(objeto, tipoAnimacion, inicio);
            }
            else if (parte != null)
            {
                _posicionInicial = ObtenerTransformacionInicial(parte, tipoAnimacion, inicio);
            }

            // Valores comunes a la animación
            _posicionFinal = final;
            _progresoInterpolacion = 0f;
            _velocidadInterpolacion = velocidadInterpolacion;
            _tipoInterpolacion = tipoInterpolacion;
        }

        // Método para obtener la transformación inicial de un Objeto o Parte basado en el tipo de animación
        private Vector3 ObtenerTransformacionInicial(dynamic target, Tipo_Animacion tipoAnimacion, Vector3 inicio)
        {
            // Ajusta la posición inicial de acuerdo con el tipo de animación
            switch (tipoAnimacion)
            {
                case Tipo_Animacion.Posicion:
                    target.Posicion = inicio;  // Ajustar la posición inicial
                    return target.Posicion;
                case Tipo_Animacion.Rotacion:
                    target.Rotacion = inicio;  // Ajustar la rotación inicial
                    return target.Rotacion;
                case Tipo_Animacion.Escala:
                    target.Escala = inicio;  // Ajustar la escala inicial
                    return target.Escala;
                default:
                    throw new InvalidOperationException("Tipo de animación no válida");
            }
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

            if (_objeto!=null)
            {
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
            }
            else if (_parte != null)
            {
                // Actualizamos la posición de la parte
                switch (_animacion)
                {
                    case Tipo_Animacion.Posicion:

                        _parte.Posicion = posicionActual; ;
                        break;
                    case Tipo_Animacion.Rotacion:

                        _parte.Rotacion = posicionActual;
                        break;
                    case Tipo_Animacion.Escala:

                        _parte.Escala = posicionActual;
                        break;
                }
            }



            // Retornar true si la interpolación terminó
            return _progresoInterpolacion >= 1f;
        }
    }
}
