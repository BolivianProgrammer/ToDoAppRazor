using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ToDoAppRazor.Models;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IWebHostEnvironment webHostEnvironment, ILogger<IndexModel> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public List<Tarea> TareasPendientes { get; set; }
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
            TareasPendientes = dataTareas.ObtenerTareasPendientesPaginadas(Paginacion);
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al cargar las tareas: {ex.Message}";
            TareasPendientes = new List<Tarea>();
        }
    }

    public IActionResult OnPostCancelar(int id)
    {
        try
        {
            string rutaArchivo = Path.Combine(_webHostEnvironment.WebRootPath, "data", "tareas.json");
            var dataTareas = TareasData.CargarDatos(rutaArchivo);

            var tarea = dataTareas.Tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                tarea.Estado = "Cancelada";
                var wrapper = new { tareas = dataTareas.Tareas };
                System.IO.File.WriteAllText(rutaArchivo, JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true }));
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al cancelar la tarea: {ex.Message}";
        }
        return RedirectToPage();
    }
}