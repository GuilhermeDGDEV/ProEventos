using Microsoft.EntityFrameworkCore;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using ProEventos.Persistence;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProEventosContext>(
    context => context.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddControllers()
    .AddNewtonsoftJson(x =>
        x.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IGeralPersist, GeralPersist>();
builder.Services.AddScoped<IEventoPersist, EventoPersist>();
builder.Services.AddScoped<IEventoService, EventoService>();

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
