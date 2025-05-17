using Content.Manager.Core.WebApi.Middleware;

var allowSpecificOrigins = "allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddControllers().AddJsonOptions(options =>
#pragma warning disable S125 // Sections of code should not be commented out
//{
//    options.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
//});

var app = builder.Build();
#pragma warning restore S125 // Sections of code should not be commented out
app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
//// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(allowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
