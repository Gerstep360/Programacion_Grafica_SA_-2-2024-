using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK_Tarea_4.Clases_Base;

namespace OpenTK_Tarea_4.Extra
{
    internal class DTO
    {
    }
    public class ObjetoDTO
    {
        public float[] Posicion { get; set; }
        public float[] Rotacion { get; set; }
        public float[] Escala { get; set; }
        public Dictionary<string, ParteDTO> Partes { get; set; }

        // Constructor vacío necesario para la deserialización
        public ObjetoDTO() { }

        public ObjetoDTO(Objeto objeto)
        {
            Posicion = new float[] { objeto.Posicion.X, objeto.Posicion.Y, objeto.Posicion.Z };
            Rotacion = new float[] { objeto.Rotacion.X, objeto.Rotacion.Y, objeto.Rotacion.Z };
            Escala = new float[] { objeto.Escala.X, objeto.Escala.Y, objeto.Escala.Z };
            Partes = objeto.Partes.ToDictionary(p => p.Key, p => new ParteDTO(p.Value));
        }
    }

    public class ParteDTO
    {
        public Dictionary<string, PuntoDTO> Puntos { get; set; }
        public uint[] Indices { get; set; }

        // Agregamos esta propiedad para contener los polígonos
        public Dictionary<string, PoligonoDTO> Poligonos { get; set; }

        // Constructor vacío necesario para la deserialización
        public ParteDTO() { }

        public ParteDTO(Parte parte)
        {
            Puntos = new Dictionary<string, PuntoDTO>();

            // Inicializar la lista de polígonos
            Poligonos = new Dictionary<string, PoligonoDTO>();

            // Recorrer cada polígono y mapear sus puntos a PuntoDTO y PoligonoDTO
            foreach (var poligonoEntry in parte.Poligonos)
            {
                var poligono = poligonoEntry.Value;
                var poligonoDTO = new PoligonoDTO(poligono);
                Poligonos[poligonoEntry.Key] = poligonoDTO;
            }

            Indices = parte.Poligonos.Values.SelectMany(p => p.Indices).ToArray();
        }
    }
    public class PoligonoDTO
    {
        public Dictionary<string, PuntoDTO> Puntos { get; set; }
        public uint[] Indices { get; set; }

        // Constructor vacío necesario para la deserialización
        public PoligonoDTO() { }

        public PoligonoDTO(Poligono poligono)
        {
            Puntos = poligono.Puntos.ToDictionary(p => p.Key, p => new PuntoDTO(p.Value));
            Indices = poligono.Indices;
        }
    }
    public class PuntoDTO
    {
        public float[] Posicion { get; set; }
        public float[] Color { get; set; }

        // Constructor vacío necesario para la deserialización
        public PuntoDTO() { }

        public PuntoDTO(Punto punto)
        {
            Posicion = new float[] { punto.Posicion.X, punto.Posicion.Y, punto.Posicion.Z };
            Color = new float[] { punto.Color.X, punto.Color.Y, punto.Color.Z };
        }
    }

}
