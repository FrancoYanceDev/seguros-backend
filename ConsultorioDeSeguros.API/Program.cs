
using ConsultorioDeSeguros.API.Asegurados;
using ConsultorioDeSeguros.API.Compartido;
using ConsultorioDeSeguros.API.Seguros;

var builder = WebApplication.CreateBuilder(args);
builder.CompartidoConfiguration();
builder.SeguroConfiguration();
builder.AseguradoConfiguration();

builder.Services.CompartidoService();
builder.Services.SeguroService();
builder.Services.AseguradoService();

var app = builder.Build();
app.UseCors(CompartidoExtends.CorsPolicy);

app.CompartidoEndpoints();
app.SeguroEndpoints();
app.AseguradoEndpoints();

app.Run();
