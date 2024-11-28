using Aplicacion;
using Persistencia;
using WebApi.Extensions;
using WebApi.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicacion();
builder.Services.AddPersistencia(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPoliciesServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddCors(o => o.AddPolicy("corsapp", builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.useSwaggerDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();
await app.SeedDataAuthentication();
app.UseCors("corsapp");
app.MapControllers();
app.Run();


