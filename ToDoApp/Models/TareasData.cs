using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToDoAppRazor.Models
{
    public class TareasData
    {
        public List<Tarea> Tareas { get; set; }

        public static TareasData CargarDatos(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    var datosIniciales = new TareasData
                    {
                        Tareas = new List<Tarea>
                        {
                            new Tarea
                            {
                                Id = 1,
                                Nombre = "Diseñar interfaz de usuario para página de inicio",
                                FechaExpiracion = DateTime.Now.AddDays(5),
                                Estado = "Pendiente"
                            },
                            new Tarea
                            {
                                Id = 2,
                                Nombre = "Implementar autenticación de usuarios",
                                FechaExpiracion = DateTime.Now.AddDays(10),
                                Estado = "Pendiente"
                            },
                            new Tarea
                            {
                                Id = 3,
                                Nombre = "Optimizar consultas de base de datos",
                                FechaExpiracion = DateTime.Now.AddDays(2),
                                Estado = "Pendiente"
                            }
                        }
                    };

                    string directorio = Path.GetDirectoryName(rutaArchivo);
                    if (!Directory.Exists(directorio))
                    {
                        Directory.CreateDirectory(directorio);
                    }

                    string jsonString = JsonSerializer.Serialize(datosIniciales, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    File.WriteAllText(rutaArchivo, jsonString);

                    return datosIniciales;
                }

                string jsonData = File.ReadAllText(rutaArchivo);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                try
                {
                    return JsonSerializer.Deserialize<TareasData>(jsonData, options);
                }
                catch
                {
                    var alternativoFormat = JsonSerializer.Deserialize<TareasWrapper>(jsonData, options);
                    return new TareasData { Tareas = alternativoFormat.Tareas };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar los datos: {ex.Message}");
         
                return new TareasData { Tareas = new List<Tarea>() };
            }
        }

        public List<Tarea> ObtenerTareasPendientesPaginadas(PaginacionInfo paginacion)
        {
            var tareasPendientes = Tareas?.Where(t => t.Estado == "Pendiente")
                .OrderBy(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();

            paginacion.TotalTareas = tareasPendientes.Count;

            return tareasPendientes
                .Skip(paginacion.IndiceInicial)
                .Take(paginacion.TareasPorPagina)
                .ToList();
        }

        public List<Tarea> ObtenerTareasTerminadasPaginadas(PaginacionInfo paginacion)
        {
            var tareasTerminadas = Tareas?.Where(t => t.Estado == "Terminada")
                .OrderByDescending(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();

            paginacion.TotalTareas = tareasTerminadas.Count;

            return tareasTerminadas
                .Skip(paginacion.IndiceInicial)
                .Take(paginacion.TareasPorPagina)
                .ToList();
        }

        public List<Tarea> ObtenerTareasCanceladasPaginadas(PaginacionInfo paginacion)
        {
            var tareasCanceladas = Tareas?.Where(t => t.Estado == "Cancelada")
                .OrderByDescending(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();

            paginacion.TotalTareas = tareasCanceladas.Count;

            return tareasCanceladas
                .Skip(paginacion.IndiceInicial)
                .Take(paginacion.TareasPorPagina)
                .ToList();
        }

        public List<Tarea> ObtenerTareasPendientes()
        {
            return Tareas?.Where(t => t.Estado == "Pendiente")
                .OrderBy(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();
        }

        public List<Tarea> ObtenerTareasTerminadas()
        {
            return Tareas?.Where(t => t.Estado == "Terminada")
                .OrderByDescending(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();
        }

        public List<Tarea> ObtenerTareasCanceladas()
        {
            return Tareas?.Where(t => t.Estado == "Cancelada")
                .OrderByDescending(t => t.FechaExpiracion)
                .ToList() ?? new List<Tarea>();
        }
    }

    public class TareasWrapper
    {
        public List<Tarea> Tareas { get; set; }
    }
}