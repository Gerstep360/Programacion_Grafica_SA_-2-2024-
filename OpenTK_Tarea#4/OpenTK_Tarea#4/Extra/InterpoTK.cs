using OpenTK.Mathematics;
using System;

namespace OpenTK_Tarea_4.Extra
{
    internal class InterpoTK
    {
        // Tipos de interpolación que soporta la clase
        public enum TipoInterpolacion
        {
            Lineal,
            Cuadratica,
            Cubica,
            Senoidal,
            Exponencial,
            Logaritmica,
            Bezier,
            Hermite,
            BSpline,
            Elastica,
            Rebote,
            Sigmoidal,
            Circular,
            Cuartica,
            Quintica,
            Paso
        }

        // Método principal para interpolar entre dos valores (inicio y fin) basado en el tiempo 't' (0 a 1)
        public static Vector3 Interpolar(Vector3 inicio, Vector3 fin, float t, TipoInterpolacion tipo)
        {
            switch (tipo)
            {
                case TipoInterpolacion.Lineal:
                    return InterpolacionLineal(inicio, fin, t);
                case TipoInterpolacion.Cuadratica:
                    return InterpolacionCuadratica(inicio, fin, t);
                case TipoInterpolacion.Cubica:
                    return InterpolacionCubica(inicio, fin, t);
                case TipoInterpolacion.Senoidal:
                    return InterpolacionSenoidal(inicio, fin, t);
                case TipoInterpolacion.Exponencial:
                    return InterpolacionExponencial(inicio, fin, t);
                case TipoInterpolacion.Logaritmica:
                    return InterpolacionLogaritmica(inicio, fin, t);
                case TipoInterpolacion.Bezier:
                    return InterpolacionBezier(inicio, fin, t);
                case TipoInterpolacion.Hermite:
                    return InterpolacionHermite(inicio, fin, t);
                case TipoInterpolacion.BSpline:
                    return InterpolacionBSpline(inicio, fin, t);
                case TipoInterpolacion.Elastica:
                    return InterpolacionElastica(inicio, fin, t);
                case TipoInterpolacion.Rebote:
                    return InterpolacionRebote(inicio, fin, t);
                case TipoInterpolacion.Sigmoidal:
                    return InterpolacionSigmoidal(inicio, fin, t);
                case TipoInterpolacion.Circular:
                    return InterpolacionCircular(inicio, fin, t);
                case TipoInterpolacion.Cuartica:
                    return InterpolacionCuartica(inicio, fin, t);
                case TipoInterpolacion.Quintica:
                    return InterpolacionQuintica(inicio, fin, t);
                case TipoInterpolacion.Paso:
                    return InterpolacionPaso(inicio, fin, t);
                default:
                    throw new ArgumentException("Tipo de interpolación no soportado");
            }
        }

        // Interpolación lineal
        private static Vector3 InterpolacionLineal(Vector3 inicio, Vector3 fin, float t)
        {
            return Vector3.Lerp(inicio, fin, t);
        }

        // Interpolación cuadrática
        private static Vector3 InterpolacionCuadratica(Vector3 inicio, Vector3 fin, float t)
        {
            return Vector3.Lerp(inicio, fin, t * t);
        }

        // Interpolación cúbica
        private static Vector3 InterpolacionCubica(Vector3 inicio, Vector3 fin, float t)
        {
            return Vector3.Lerp(inicio, fin, t * t * t);
        }

        // Interpolación senoidal
        private static Vector3 InterpolacionSenoidal(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = (float)Math.Sin(t * Math.PI / 2);  // Suave al final
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación exponencial
        private static Vector3 InterpolacionExponencial(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = (float)Math.Pow(2, 10 * (t - 1));  // Aceleración rápida
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación logarítmica
        private static Vector3 InterpolacionLogaritmica(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = (float)(Math.Log(1 + 9 * t) / Math.Log(10));  // Aceleración inversa
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación Bézier (simplificada)
        private static Vector3 InterpolacionBezier(Vector3 inicio, Vector3 fin, float t)
        {
            // Bézier de 3 puntos con control intermedio en (0.5, 0.5)
            Vector3 puntoControl = Vector3.Lerp(inicio, fin, 0.5f);
            return Vector3.Lerp(Vector3.Lerp(inicio, puntoControl, t), Vector3.Lerp(puntoControl, fin, t), t);
        }

        // Interpolación Hermite (simplificada)
        private static Vector3 InterpolacionHermite(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = t * t * (3 - 2 * t);  // Curva suavizada Hermite
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación B-Spline (simplificada)
        private static Vector3 InterpolacionBSpline(Vector3 inicio, Vector3 fin, float t)
        {
            // Una curva B-Spline básica para suavidad
            float tModificado = (3 * t * t - 2 * t * t * t);  // Suaviza la curva
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación elástica (Elastic)
        private static Vector3 InterpolacionElastica(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = (float)Math.Sin(13 * Math.PI / 2 * t) * (float)Math.Pow(2, 10 * (t - 1));
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación de rebote (Bounce)
        private static Vector3 InterpolacionRebote(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = 1 - Math.Abs((float)Math.Sin(6 * t));  // Efecto de rebote
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación sigmoidal
        private static Vector3 InterpolacionSigmoidal(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = 1 / (1 + (float)Math.Exp(-10 * (t - 0.5)));  // Sigmoide para suavidad
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación circular
        private static Vector3 InterpolacionCircular(Vector3 inicio, Vector3 fin, float t)
        {
            float tModificado = (float)(1 - Math.Cos(t * Math.PI / 2));  // Curva circular
            return Vector3.Lerp(inicio, fin, tModificado);
        }

        // Interpolación cuártica
        private static Vector3 InterpolacionCuartica(Vector3 inicio, Vector3 fin, float t)
        {
            return Vector3.Lerp(inicio, fin, t * t * t * t);
        }

        // Interpolación quíntica
        private static Vector3 InterpolacionQuintica(Vector3 inicio, Vector3 fin, float t)
        {
            return Vector3.Lerp(inicio, fin, t * t * t * t * t);
        }

        // Interpolación por pasos (Step)
        private static Vector3 InterpolacionPaso(Vector3 inicio, Vector3 fin, float t)
        {
            return t < 1 ? inicio : fin;  // Salta directamente al valor final
        }

        // Función para actualizar la interpolación (animación continua) basada en el tiempo delta
        public static void ActualizarInterpolacion(ref Vector3 valorActual, Vector3 destino, float duracion, float tiempoTranscurrido, TipoInterpolacion tipo)
        {
            // Calcular el progreso en función del tiempo
            float t = tiempoTranscurrido / duracion;
            t = Math.Clamp(t, 0f, 1f);  // Limitar t entre 0 y 1

            // Interpolar hacia el destino usando el tipo de interpolación elegido
            valorActual = Interpolar(valorActual, destino, t, tipo);
        }
    }
}
