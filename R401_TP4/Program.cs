
using Microsoft.EntityFrameworkCore;
using R401_TP4.Models.DataManager;
using R401_TP4.Models.EntityFramework;
using R401_TP4.Models.Repository;

namespace R401_TP4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();

            builder.Services.AddDbContext<FilmRatingsDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FilmRatingsDBContext")));

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
        }
    }
}
