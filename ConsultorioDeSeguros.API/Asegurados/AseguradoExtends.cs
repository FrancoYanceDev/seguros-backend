using ConsultorioDeSeguros.Domain.Asegurados.Models;
using ConsultorioDeSeguros.Domain.Asegurados.Repository;
using ConsultorioDeSeguros.Domain.Seguros.Models;
using ConsultorioDeSeguros.Domain.Seguros.Repository;
using ConsultorioDeSeguros.Infrastructure.Asegurados.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsultorioDeSeguros.API.Asegurados
{
    public static class AseguradoExtends
    {
        public static WebApplicationBuilder AseguradoConfiguration(this WebApplicationBuilder builder)
        {

            return builder;
        }

        public static IServiceCollection AseguradoService(this IServiceCollection services)
        {
            services.AddScoped<IAseguradoRepository, AseguradoRepository>();
            return services;
        }

        public static IEndpointRouteBuilder AseguradoEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(
                "/asegurado"
                , async (
                     HttpContext context
                     , [FromServices] IAseguradoRepository AseguradoRepository) =>
                {
                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };
                    return JsonSerializer.Serialize(await AseguradoRepository.GetAll(), options);
                });

            endpoints.MapGet(
                "/asegurado/{cedula}"
                , async (
                       HttpContext context
                     , string cedula
                     , [FromServices] IAseguradoRepository AseguradoRepository
                     , [FromServices] ISeguroRepository SeguroRepository) =>
                {
                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };

                    AseguradoModel? asegurado = AseguradoRepository.GetByCedula(cedula);
                    if (asegurado != null)
                    {
                        asegurado.Seguros = await SeguroRepository.GetByCedula(cedula);
                    }
                   


                    return JsonSerializer.Serialize(asegurado, options);
                });

            endpoints.MapPost(
                "/asegurado"
                , async (
                     HttpContext context
                     , [FromBody] AseguradoModel AseguradoModel
                     , [FromServices] IAseguradoRepository AseguradoRepository) =>
                {
                    AseguradoModel asegurado = await AseguradoRepository.Create(AseguradoModel);
                    await AseguradoRepository.Save();
 

                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };
                    return JsonSerializer.Serialize(asegurado, options);
                });

            endpoints.MapPut(
                "/asegurado"
                , async (
                     HttpContext context
                     , [FromBody] AseguradoModel AseguradoModel
                     , [FromServices] IAseguradoRepository AseguradoRepository) =>
                {
                    await AseguradoRepository.RemoveSeguros(AseguradoModel.Cedula);
                    List<SeguroAseguradoModel> seguroAsegurados = new List<SeguroAseguradoModel>();
                    AseguradoModel.Seguros.ForEach(seg => {
                        seguroAsegurados.Add(seg);
                    });
                    AseguradoModel.Seguros = new List<SeguroAseguradoModel>();

                    AseguradoModel asegurado = AseguradoRepository.Update(AseguradoModel);
                    await AseguradoRepository.Save();

                    List<SeguroAseguradoModel> asegurados = await AseguradoRepository.AddSeguros(seguroAsegurados);
                    await AseguradoRepository.Save();


                    asegurado.Seguros = seguroAsegurados;
                    var options = new JsonSerializerOptions()
                    {
                        MaxDepth = 0,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        IgnoreReadOnlyProperties = true
                    };
                    return JsonSerializer.Serialize(asegurado, options);
                });


            endpoints.MapDelete(
                "/asegurado/{cedula}"
                , async (
                     HttpContext context
                     ,string cedula
                     , [FromServices] IAseguradoRepository AseguradoRepository) =>
                {
                    await AseguradoRepository.RemoveSeguros(cedula);
                    await AseguradoRepository.Save();

                    AseguradoRepository.Remove(cedula);
                    await AseguradoRepository.Save();

                    await context.Response.WriteAsJsonAsync(cedula);
                });


            return endpoints;
        }

    }
}
