using Carter;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using POCAssessment.Application.Products;
using POCAssessment.Domain;
using POCAssessment.Infrastructure;
using POCAssessment.Presentation;
using System.Diagnostics.CodeAnalysis;

namespace POCAssessment;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Configuration
        builder.Services.AddOptions<DataSettings>()
                    .Bind(builder.Configuration.GetSection(nameof(DataSettings)))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<DataSettings>>().Value);


        //Mediatr library
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProductEndpoints).Assembly));

        //Add Carter endpoints
        builder.Services.AddCarter();

        builder.Services.AddHttpClient("products");

        //Problem Details
        builder.Services.AddProblemDetails();

        // add validators
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<ProductFilterQueryValidation>();

        // Add services to the container.
        builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IDataContext, DataContext>();

        //Add swagger
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
        //Carter
        app.MapCarter();
        await app.RunAsync();
    }
}