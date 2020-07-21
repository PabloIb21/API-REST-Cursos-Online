using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> devolverPaginacion(string storeProcedure,
            int numeroPagina,
            int cantidadElementos,
            IDictionary<string,object> parametrosFiltro,
            string ordenamientoColumna
        );
    }
}