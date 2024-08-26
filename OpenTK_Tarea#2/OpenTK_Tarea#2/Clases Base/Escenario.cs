using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK_Tarea_1.Clases_Base;

namespace OpenTK_Tarea_2.Clases_Base
{
    public class Escenario
    {
        public List<Objeto> Objetos { get; private set; }

        public Escenario()
        {
            Objetos = new List<Objeto>();
        }

        public void AgregarObjeto(Objeto objeto)
        {
            if (objeto == null) throw new ArgumentNullException(nameof(objeto));
            Objetos.Add(objeto);
        }

        public void DibujarEscenario(Renderización renderizacion, Matrix4 view, Matrix4 projection)
        {
            foreach (var objeto in Objetos)
            {
                renderizacion.RenderizarObjeto(objeto, view, projection);
            }
        }
    }
}
