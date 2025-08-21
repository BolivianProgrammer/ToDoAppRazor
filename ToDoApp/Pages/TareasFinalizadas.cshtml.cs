using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ToDoAppRazor.Models;

public class TareasFinalizadasModel : PageModel
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<TareasFinalizadasModel> _logger;

    public TareasFinalizadasModel(IWebHostEnvironment webHostEnvironment, ILogger<TareasFinalizadasModel> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public List<Tarea> TareasTerminadas { get; set; }
    public string MensajeError { get; set; }
    public PaginacionInfo Paginacion { get; set; } = new PaginacionInfo();

    [BindProperty(SupportsGet = true)]
    public int PaginaActual { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int TareasPorPagina { get; set; } = 5;

    public void OnGet()
    {
        try
        {
            Paginacion.PaginaActual = PaginaActual > 0 ? PaginaActual : 1;
            Paginacion.TareasPorPagina = TareasPorPagina > 0 ? TareasPorPagina : 5;

            string rutaArchivo = Path.Combine(_webHostEnvironment.WebRootPath, "data", "tareas.json");
            var dataTareas = TareasData.CargarDatos(rutaArchivo);

            TareasTerminadas = dataTareas.ObtenerTareasTerminadasPaginadas(Paginacion);

            _logger.LogInformation($"Tareas terminadas cargadas exitosamente. Mostrando página {Paginacion.PaginaActual} de {Paginacion.TotalPaginas} (Total: {Paginacion.TotalTareas})");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al cargar tareas terminadas: {ex.Message}");
            MensajeError = $"Error al cargar las tareas: {ex.Message}";
            TareasTerminadas = new List<Tarea>();
        }
    }
}