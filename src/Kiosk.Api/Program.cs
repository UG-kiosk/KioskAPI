using System.Text.Json.Serialization;
using Kiosk.Repositories;
using Kiosk.Repositories.Interfaces;
using KioskAPI.Filters;
using KioskAPI.Services;
using KioskAPI.Services.Interfaces;
using Microsoft.AspNetCore.Rewrite;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var pack = new ConventionPack();
pack.Add(new CamelCaseElementNameConvention());
ConventionRegistry.Register("Camel case convention", pack, t => true);

var connectionString = builder.Configuration.GetSection("KioskDatabase:ConnectionString").Value;
var databaseName = builder.Configuration.GetSection("KioskDatabase:DatabaseName").Value;

var database = new MongoClient(connectionString).GetDatabase(databaseName);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSingleton(database);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<ValidateTokenFilter>()
    .AddScoped<IStaffRepository, StaffRepository>()
    .AddScoped<IStaffService, StaffService>()
    .AddScoped<IEctsSubjectRepository, EctsSubjectRepository>()
    .AddScoped<IEctsSubjectService, EctsSubjectService>()
    .AddScoped<IEventsRepository, EventsRepository>()
    .AddScoped<IEventsService, EventsService>()
    .AddScoped<IMajorsRepository, MajorsRepository>()
    .AddScoped<IMajorsService, MajorsService>()
    .AddScoped<INewsRepository, NewsRepository>()
    .AddScoped<INewsService, NewsService>()
    .AddScoped<ILessonPlanRepository, LessonPlanRepository>()
    .AddScoped<ILessonPlanService, LessonPlanService>()
    .AddScoped<ITranslatorService, TranslatorService>();


builder.Services.AddCors((options =>
{
    options.AddPolicy("corspolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
}));

builder.Services
    .AddMvc()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options =>
{
    options.AddPolicy("corspolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
}));

var app = builder.Build();


app.UseMiddleware<GlobalRoutePrefixMiddleware>("/kiosk-api");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(option => option.RouteTemplate = "kiosk-api/{documentName}/swagger.json");
    app.UseSwaggerUI(option=>
    {
        option.SwaggerEndpoint("/kiosk-api/v1/swagger.json", "CodeMaze API");
        option.RoutePrefix = "kiosk-api/swagger";
    });
    var option = new RewriteOptions();
    option.AddRedirect("^$", "swagger");
    app.UseRewriter(option);
}
app.UsePathBase(new PathString("/kiosk-api"));

app.UseHttpsRedirection();


app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.UseCors("corspolicy");



app.Run();