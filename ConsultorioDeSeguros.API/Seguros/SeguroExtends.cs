using ConsultorioDeSeguros.Domain.Asegurados.Models;
using ConsultorioDeSeguros.Domain.Asegurados.Repository;
using ConsultorioDeSeguros.Domain.Seguros.Models;
using ConsultorioDeSeguros.Domain.Seguros.Repository;
using ConsultorioDeSeguros.Infrastructure.Seguros.Repository;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsultorioDeSeguros.API.Seguros
{
    public static class SeguroExtends
    {
        public static WebApplicationBuilder SeguroConfiguration(this WebApplicationBuilder builder)
        {

            return builder;
        }

        public static IServiceCollection SeguroService(this IServiceCollection services)
        {
            services.AddScoped<ISeguroRepository, SeguroRepository>();
            return services;
        }
        public static IEndpointRouteBuilder SeguroEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(
                "/seguro"
                ,async (
                    HttpContext context
                    ,[FromServices] ISeguroRepository SeguroRepository) =>
            {
                var options = new JsonSerializerOptions()
                {
                    MaxDepth = 0,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    IgnoreReadOnlyProperties = true
                };
                List<SeguroModel> seguros = await SeguroRepository.GetAll();

                return JsonSerializer.Serialize(seguros, options);
            });

            endpoints.MapGet(
                "/seguro/{codigo}"
                , async (
                       HttpContext context
                     , int codigo
                     , [FromServices] ISeguroRepository SeguroRepository) =>
                {
                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };
                    SeguroModel? seguro = SeguroRepository.GetByCode(codigo);
                    if (seguro != null)
                    {
                        seguro.Asegurados = await SeguroRepository.GetAsegurados(codigo);
                    }
                    return JsonSerializer.Serialize(seguro, options);
                });

            endpoints.MapPost(
                "/seguro"
                , async (
                     HttpContext context
                     , [FromBody] SeguroModel SeguroModel
                     , [FromServices] ISeguroRepository SeguroRepository) =>
                {
                    SeguroModel seguro =  await SeguroRepository.Create(SeguroModel);
                    await SeguroRepository.Save();

                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };

                    return JsonSerializer.Serialize(seguro, options);
                });
            
            endpoints.MapPost(
                "/asegurado/import"
                , async (
                       HttpContext context
                     , [FromServices] IAseguradoRepository AseguradoRepository) => {
                         try
                         {
                             IFormFile file = context.Request.Form.Files[0];
                             Stream inputstream = file.OpenReadStream();
                             XSSFWorkbook workbook = new XSSFWorkbook(inputstream);
                             ISheet sheet = workbook.GetSheetAt(0);

                             bool rowIsValid = true;
                             List<AseguradoModel> asegurados = new List<AseguradoModel>();

                             //Recorrect Fila
                             for (int rowIdx = 2; rowIdx <= sheet.LastRowNum; rowIdx++)
                             {
                                 IRow currentRow = sheet.GetRow(rowIdx);
                                 int index = 0;

                                 //Recorrer campos
                                 AseguradoModel asegurado = new AseguradoModel();
                                 foreach (var celda in currentRow.Cells)
                                 {

                                     if (index == 0 && celda.CellType != CellType.String)
                                     {
                                         rowIsValid = false;
                                     }
                                     if (index == 1 && celda.CellType != CellType.String)
                                     {

                                         rowIsValid = false;
                                     }
                                     if (index == 2 && celda.CellType != CellType.String)
                                     {

                                         rowIsValid = false;
                                     }
                                     if (index == 4 && celda.CellType != CellType.Numeric)
                                     {

                                         rowIsValid = false;
                                     }

                                     if (rowIsValid == false)
                                     {
                                         break;
                                     }

                                     if (index == 0)
                                     {
                                         asegurado.Cedula = celda.StringCellValue;
                                     }
                                     if (index == 1)
                                     {
                                         asegurado.Nombre = celda.StringCellValue;
                                     }
                                     if (index == 2)
                                     {
                                         asegurado.Telefono = celda.StringCellValue;
                                     }
                                     if (index == 3)
                                     {
                                         asegurado.Edad = (byte)celda.NumericCellValue;
                                     }

                                     index++;
                                 };
                                 asegurados.Add(asegurado);
                             }

                             if (rowIsValid == false)
                                 return JsonSerializer.Serialize(new { code = 500, message = "El formato no es válido" });




                             var options = new JsonSerializerOptions()
                             {
                                 MaxDepth = 0,
                                 ReferenceHandler = ReferenceHandler.IgnoreCycles,
                                 IgnoreReadOnlyProperties = true
                             };
                             List<AseguradoModel> newAsegurados = await AseguradoRepository.CreateMany(asegurados);
                             await AseguradoRepository.Save();
                             return JsonSerializer.Serialize(newAsegurados, options);
                         }
                         catch (Exception ex)
                         {
                             if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                             {
                                 return JsonSerializer.Serialize(new { code = 500 , message = "Ciertos datos del excel estan duplicados."});
                             };
                             return JsonSerializer.Serialize(new {code = 500, message = "Error no corregido"});
                         }
                });

            endpoints.MapPut(
                "/seguro"
                , async (
                     HttpContext context
                     , [FromBody] SeguroModel SeguroModel
                     , [FromServices] ISeguroRepository SeguroRepository) =>
                {
                    SeguroModel seguro = SeguroRepository.Update(SeguroModel);
                    await SeguroRepository.Save();

                    await context.Response.WriteAsJsonAsync(seguro);
                });
            endpoints.MapDelete(
                "/seguro/{codigo}"
                , async (
                     HttpContext context
                     , int codigo
                     , [FromServices] ISeguroRepository SeguroRepository) =>
                {
                    SeguroRepository.Remove(codigo);
                    await SeguroRepository.Save();

                    await context.Response.WriteAsJsonAsync(codigo);
                });

            return endpoints;
        }
    }
}
