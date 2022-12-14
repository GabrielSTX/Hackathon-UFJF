using hackathon.Controllers;

namespace hackaton
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            InicializacaoController.Iniciar();

            builder.Services.AddCors(config =>
            {
                config.AddPolicy("libera_acesso", options =>
                {
                    options.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
            });                

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("libera_acesso");

            app.MapControllers();

            app.Run();
        }
    }
}