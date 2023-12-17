using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Infrastructure.Repositories;
using PatientManagementServices.Services.Interfaces;
using PatientManagementServices.Services;
using PatientManagementServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PatientManagementServices.Domain.Interfaces;
using PatientManagementServices.Infrastructure.Dapper;
using PatientManagementServices.Application.Core.Background;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Application.Core.Cache;
using PatientManagementServices.Infrastructure.Cache;
using PatientManagementServices.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Load the connection string from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer()
);

//DI

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped(typeof(IDapper<>), typeof(Dapper<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
//builder.Services.AddScoped<IAllergyTypeRepository, AllergyTypeRepository>();
//builder.Services.AddScoped<IAllergyRepository, AllergyRepository>();
//builder.Services.AddScoped<IPatientAllergyRepository, PatientAllergyRepository>();
//builder.Services.AddScoped<IPatientOperationRepository, PatientOperationRepository>();
//builder.Services.AddScoped<IPatientDiseaseRepository, PatientDiseaseRepository>();

//builder.Services.AddScoped<IGenericAttributeRepository, GenericAttributeRepository>();
//builder.Services.AddScoped<IGenericAttributeService, GenericAttributeService>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientAllergyService, PatientAllergyService>();
builder.Services.AddScoped<IPatientOperationService, PatientOperationService>();
builder.Services.AddScoped<IPatientDiseaseService, PatientDiseaseService>();

//builder.Services.AddScoped<ICacheService, CacheService>();
//builder.Services.AddHangfireServer();

//JSON
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Patient API's
app.MapPost("/CreatePatient", async (IPatientService patientService, [FromBody] PatientCreateDto model) =>
{

    return await patientService.CreatePatientAsync(model);
})
.WithName("Create Patient")
.WithOpenApi();

app.MapGet("/Patients", async (IPatientService patientService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await patientService.GetAllPatientsAsync(searchRequest);
})
.WithName("Get Patient")
.WithOpenApi();

app.MapPatch("/UpdatePatients", async (IPatientService patientService, [FromBody] PatientCreateDto model) =>
{
    return await patientService.UpdatePatientAsync(model);
})
.WithName("Update Patients")
.WithOpenApi();

app.MapDelete("/DeletePatients", async (int id, IPatientService patientService) =>
{
    var response = patientService.GetPatientByIdAsync(id);
    if(response == null) return Results.NoContent();
    var dataDelete = await patientService.DeletePatientAsync(id);
    return Results.Ok(dataDelete);
})
.WithName("Delete Patients")
.WithOpenApi();
#endregion

#region Patients Allergies API's
app.MapPost("/CreatePatientAllergy", async (IPatientAllergyService patientAllergyService, [FromBody] PatientAllergyCreateDto model) =>
{

    return await patientAllergyService.CreatePatientAllergyAsync(model);
})
.WithName("Create Patient Allergy")
.WithOpenApi();

app.MapPost("/GetPatientAllergies", async (IPatientAllergyService PatientAllergyService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await PatientAllergyService.GetAllPatientergiesAsync(searchRequest);
})
.WithName("Get Patient Allergies")
.WithOpenApi();

app.MapPatch("/UpdatePatientAllergy", async (IPatientAllergyService PatientAllergyService, [FromBody] PatientAllergyCreateDto model) =>
{
    return await PatientAllergyService.UpdatePatientAllergyAsync(model);
})
.WithName("Update Patient Allergy")
.WithOpenApi();

app.MapDelete("/DeletePatientAllergy", async (int id, IPatientAllergyService PatientAllergyService) =>
{
    var response = PatientAllergyService.GetPatientAllergyByIdAsync(id);
    if (response == null) return Results.NoContent();
    var dataDelete = await PatientAllergyService.DeletePatientAllergyAsync(id);
    return Results.Ok(dataDelete);
})
.WithName("Delete Patient Allergy")
.WithOpenApi();
#endregion

#region Allergy Types API's
app.MapPost("/CreateAllergyType", async (IPatientAllergyService patientAllergyService, [FromBody] AllergyTypeCreateDto model) =>
{

    return await patientAllergyService.CreateAllergyTypeAsync(model);
})
.WithName("Create Allergy Type")
.WithOpenApi();

app.MapPost("/GetAllergyTypes", async (IPatientAllergyService PatientAllergyService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await PatientAllergyService.GetAllAllergyTypeAsync(searchRequest);
})
.WithName("Get Allergy Type")
.WithOpenApi();

app.MapPost("/GetAllergyTypeById", async (IPatientAllergyService PatientAllergyService, long id) =>
{
    return await PatientAllergyService.GetAllergyTypeByIdAsync(id);
})
.WithName("Get Allergy Type by Id")
.WithOpenApi();

app.MapPatch("/UpdateAllergyType", async (IPatientAllergyService PatientAllergyService, [FromBody] AllergyTypeCreateDto model) =>
{
    return await PatientAllergyService.UpdateAllergyTypeAsync(model);
})
.WithName("Update Allergy Type")
.WithOpenApi();

app.MapDelete("/DeleteAllergyType", async (int id, IPatientAllergyService PatientAllergyService) =>
{
    var response = PatientAllergyService.GetAllergyTypeByIdAsync(id);
    if (response == null) return Results.NoContent();
    var dataDelete = await PatientAllergyService.DeleteAllergyTypeAsync(id);
    return Results.Ok(dataDelete);
})
.WithName("Delete Allergy Type")
.WithOpenApi();
#endregion

#region Allergies API's
app.MapPost("/CreateAllergy", async (IPatientAllergyService patientAllergyService, [FromBody] AllergyCreateDto model) =>
{

    return await patientAllergyService.CreateAllergyAsync(model);
})
.WithName("Create Allergy")
.WithOpenApi();

app.MapPost("/GetAllergies", async (IPatientAllergyService PatientAllergyService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await PatientAllergyService.GetAllAllergiesAsync(searchRequest);
})
.WithName("Get Allergies")
.WithOpenApi();

app.MapGet("/GetAllergyById", async (IPatientAllergyService PatientAllergyService, long id) =>
{
    return await PatientAllergyService.GetAllergyByIdAsync(id);
})
.WithName("Get Allergy by Id")
.WithOpenApi();

app.MapPatch("/UpdateAllergy", async (IPatientAllergyService PatientAllergyService, [FromBody] AllergyCreateDto model) =>
{
    return await PatientAllergyService.UpdateAllergyAsync(model);
})
.WithName("Update Allergy")
.WithOpenApi();

app.MapDelete("/DeleteAllergy", async (int id, IPatientAllergyService PatientAllergyService) =>
{
   return await PatientAllergyService.DeleteAllergyAsync(id);
})
.WithName("Delete Allergy")
.WithOpenApi();
#endregion

#region Operations API's
app.MapPost("/CreateOperation", async (IPatientOperationService patientOperationService, [FromBody] OperationCreateDto model) =>
{

    return await patientOperationService.CreateOperationAsync(model);
})
.WithName("Create Operation")
.WithOpenApi();

app.MapPost("/GetOperations", async (IPatientOperationService patientOperationService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await patientOperationService.GetAllOperationsAsync(searchRequest);
})
.WithName("Get Operations")
.WithOpenApi();

app.MapGet("/GetOperationById", async (IPatientOperationService PatientOperationService, long id) =>
{
    return await PatientOperationService.GetOperationByIdAsync(id);
})
.WithName("Get Operation by Id")
.WithOpenApi();

app.MapPatch("/UpdateOperation", async (IPatientOperationService patientOperationService, [FromBody] OperationCreateDto model) =>
{
    return await patientOperationService.UpdateOperationAsync(model);
})
.WithName("Update Operation")
.WithOpenApi();

app.MapDelete("/DeleteOperation", async (int id, IPatientOperationService patientOperationService) =>
{
    var dataDelete = await patientOperationService.DeleteOperationAsync(id);
    return Results.Ok(dataDelete);
})
.WithName("Delete Operation")
.WithOpenApi();
#endregion

#region Patients Operations API's
app.MapPost("/CreatePatientOperation", async (IPatientOperationService patientOperationService, [FromBody] PatientOperationCreateDto model) =>
{

    return await patientOperationService.CreatePatientOperationAsync(model);
})
.WithName("Create Patient Operation")
.WithOpenApi();

app.MapPost("/GetPatientOperations", async (IPatientOperationService PatientOperationService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await PatientOperationService.GetAllPatientOperationsAsync(searchRequest);
})
.WithName("Get Patient Operations")
.WithOpenApi();

app.MapPatch("/UpdatePatientOperation", async (IPatientOperationService PatientOperationService, [FromBody] PatientOperationCreateDto model) =>
{
    return await PatientOperationService.UpdatePatientOperationAsync(model);
})
.WithName("Update Patient Operation")
.WithOpenApi();

app.MapDelete("/DeletePatientOperation", async (int id, IPatientOperationService patientOperationService) =>
{
    return await patientOperationService.DeletePatientOperationAsync(id);
})
.WithName("Delete Patient Operation")
.WithOpenApi();
#endregion

#region Diseases API's
app.MapPost("/CreateDisease", async (IPatientDiseaseService patientDiseaseService, [FromBody] DiseaseCreateDto model) =>
{

    return await patientDiseaseService.CreateDiseaseAsync(model);
})
.WithName("Create Disease")
.WithOpenApi();

app.MapPost("/GetDiseases", async (IPatientDiseaseService patientDiseaseService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await patientDiseaseService.GetAllDiseasesAsync(searchRequest);
})
.WithName("Get Diseases")
.WithOpenApi();

app.MapGet("/GetDiseaseById", async (IPatientDiseaseService PatientDiseaseService, long id) =>
{
    return await PatientDiseaseService.GetDiseaseByIdAsync(id);
})
.WithName("Get Disease by Id")
.WithOpenApi();

app.MapPatch("/UpdateDisease", async (IPatientDiseaseService patientDiseaseService, [FromBody] DiseaseCreateDto model) =>
{
    return await patientDiseaseService.UpdateDiseaseAsync(model);
})
.WithName("Update Disease")
.WithOpenApi();

app.MapDelete("/DeleteDisease", async (int id, IPatientDiseaseService patientDiseaseService) =>
{
    var response = patientDiseaseService.GetDiseaseByIdAsync(id);
    if (response == null) return Results.NoContent();
    var dataDelete = await patientDiseaseService.DeleteDiseaseAsync(id);
    return Results.Ok(dataDelete);
})
.WithName("Delete Disease")
.WithOpenApi();
#endregion

#region Patients Diseases API's
app.MapPost("/CreatePatientDisease", async (IPatientDiseaseService patientDiseaseService, [FromBody] PatientDiseaseCreateDto model) =>
{

    return await patientDiseaseService.CreatePatientDiseaseAsync(model);
})
.WithName("Create Patient Disease")
.WithOpenApi();

app.MapPost("/GetPatientDiseases", async (IPatientDiseaseService PatientDiseaseService, [FromBody] ApiBaseSearchRequest searchRequest) =>
{
    return await PatientDiseaseService.GetAllPatientDiseasesAsync(searchRequest);
})
.WithName("Get Patient Diseases")
.WithOpenApi();

app.MapPatch("/UpdatePatientDisease", async (IPatientDiseaseService PatientDiseaseService, [FromBody] PatientDiseaseCreateDto model) =>
{
    return await PatientDiseaseService.UpdatePatientDiseaseAsync(model);
})
.WithName("Update Patient Disease")
.WithOpenApi();

app.MapDelete("/DeletePatientDisease", async (int id, IPatientDiseaseService patientDiseaseService) =>
{
    return await patientDiseaseService.DeletePatientDiseaseAsync(id);
})
.WithName("Delete Patient Disease")
.WithOpenApi();
#endregion

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));





app.Run();
