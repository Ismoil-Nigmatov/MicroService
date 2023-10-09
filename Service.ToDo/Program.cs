using Microsoft.EntityFrameworkCore;
using Service.ToDo.Data;
using Service.ToDo.Repository;
using Service.ToDo.Repository.Impl;
using Service.ToDo.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository , UserRepository>();
builder.Services.AddScoped<ITaskRepository , TaskRepository>();
builder.Services.AddScoped<EmailProducer>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("databaseConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
