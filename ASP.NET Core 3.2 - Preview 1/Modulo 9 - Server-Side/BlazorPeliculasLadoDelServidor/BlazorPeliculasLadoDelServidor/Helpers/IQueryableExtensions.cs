using BlazorPeliculasLadoDelServidor.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Helpers
{
    public static class IQueryableExtensions
    {
        public async static Task<int> CalcularTotalPaginas<T>(this IQueryable<T> queryable,
            int cantidadRegistrosAMostrar)
        {
            double conteo = await queryable.CountAsync();
            int totalPaginas = (int)Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            return totalPaginas;
        }

        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Paginacion paginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
                .Take(paginacion.CantidadRegistros);
        }
    }
}
