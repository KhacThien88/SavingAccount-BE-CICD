using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;
using SavingAccount_BE.Seeders;
using SavingAccount_BE.Service.Accounts;
using SavingAccount_BE.Service.Admin.AddUserCard;
using SavingAccount_BE.Service.Users.Cards;
using SavingAccount_BE.Service.Users.Deposits;
using SavingAccount_BE.Service.Users.Histories;
using SavingAccount_BE.Service.Users.Profile;
using SavingAccount_BE.Service.Users.SavingAccounts;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SavingAccount API", Version = "v1" });

    // C?u h�nh x�c th?c JWT cho Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nh?p 'Bearer' [kho?ng tr?ng] v� sau ?� d�n token JWT c?a b?n."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddTransient<IUserCard, UserCardService>();
builder.Services.AddTransient<IUserHistory, UserHistory>();
builder.Services.AddTransient<IUserSavingAccountService, UserSavingAccountService>();
builder.Services.AddTransient<IUserProfile ,UserProfile>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddTransient<IAddUserCard, AddUserCard>();
builder.Services.AddTransient<IDepositsAndWithdraws ,DepositsAndWithdraws>();

builder.Services.AddDbContext<SavingAccountDbContext>(
    option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("UsersDatabase"));
    }
);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SavingAccountDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var serviceScope = app.Services.CreateScope();
    using var dbContext = serviceScope.ServiceProvider.GetService<SavingAccountDbContext>();
    dbContext?.Database.Migrate();
}
await RoleSeeder.InitializeRoles(app.Services);
await UserSeeder.InitializeUsers(app.Services);

app.UseCors(
    builder =>
    {
        builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    }
);


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
