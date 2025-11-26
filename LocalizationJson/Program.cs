using LocalizationJson.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger/OpenAPI - Learn more at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Localization configuration - Multi-language support
builder.Services.AddLocalizationConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable localization - Language middleware
app.UseLocalizationConfiguraton();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
