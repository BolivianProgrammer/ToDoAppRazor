using System;

namespace ToDoAppRazor.Models
{
    public class PaginacionInfo
    {
        public int PaginaActual { get; set; }
        public int TareasPorPagina { get; set; }
        public int TotalTareas { get; set; }

        public PaginacionInfo()
        {
            PaginaActual = 1;
            TareasPorPagina = 5; 
        }

        public int TotalPaginas => (int)Math.Ceiling((decimal)TotalTareas / TareasPorPagina);

        public bool TienePaginaAnterior => PaginaActual > 1;

        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
        public int PaginaAnterior => Math.Max(PaginaActual - 1, 1);

        public int PaginaSiguiente => Math.Min(PaginaActual + 1, TotalPaginas);

        public int IndiceInicial => (PaginaActual - 1) * TareasPorPagina;
    }
}