using Microsoft.EntityFrameworkCore;
using MusicStore.Persistence;
using MusicStoreRepositories;

var builder = WebApplication.CreateBuilder(args);


//registrar conetexto
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});
// Add services to the container.

builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IConcertRepository, ConcertRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
