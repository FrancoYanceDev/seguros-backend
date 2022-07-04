using ConsultorioDeSeguros.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ConsultorioDeSeguros.API.Compartido
{
    public static class CompartidoExtends
    {
        public static readonly string CorsPolicy = "CorsPolicy";
        public static WebApplicationBuilder CompartidoConfiguration(this WebApplicationBuilder builder)
        {
            /*Base de datos:  conexión*/
            builder.Services.AddDbContext<SegurosContexto>(
                options => options.UseSqlServer(
                    "name=ConnectionStrings:DefaultConnection"
                    , b => b.MigrationsAssembly("ConsultorioDeSeguros.Infrastructure")));


            builder.Services.AddCors(
                options => options.AddPolicy(
                                CorsPolicy
                                ,builder => builder
                                                   .AllowAnyOrigin()
                                                   .AllowAnyMethod()
                                                   .AllowAnyHeader()
                            ));



            return builder;
        }

        public static IServiceCollection CompartidoService(this IServiceCollection services)
        {

      
            return services;
        }
        public static IEndpointRouteBuilder CompartidoEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/", () => "Consultorio de Seguros");

            return endpoints;
        }
    }
}
