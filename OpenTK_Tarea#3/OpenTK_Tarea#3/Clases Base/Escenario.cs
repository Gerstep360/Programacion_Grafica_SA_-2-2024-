using System.Collections.Generic;
using OpenTK.Mathematics;

namespace OpenTK_Tarea_3.Clases_Base
{
    public class Escenario
    {
        private readonly List<Objeto> _objetos = new List<Objeto>();

        public IReadOnlyList<Objeto> Objetos => _objetos.AsReadOnly();

        public void AgregarObjeto(Objeto objeto)
        {
            if (objeto == null) throw new ArgumentNullException(nameof(objeto));
            _objetos.Add(objeto);
        }

        public void DibujarEscenario(Renderización renderizacion, Matrix4 view, Matrix4 projection)
        {
            for (int i = 0; i < _objetos.Count; i++)
            {
                renderizacion.RenderizarObjeto(_objetos[i], view, projection);
            }
        }
    }
}
