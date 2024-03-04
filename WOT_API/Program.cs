using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using WOT_Models;
using Microsoft.EntityFrameworkCore;
using WOT_API;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#region Configure DBCore Connection

var _GetConnectionString = builder.Configuration.GetConnectionString("connCORE");
builder.Services.AddDbContext<WOTCOREContext>(options => options.UseSqlServer(_GetConnectionString));

#endregion

#region Configure SwaggerGen

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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

#endregion

#region Configure JWTSettings

var _jwtsetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JWTSettings>(_jwtsetting);

#endregion

#region Configure Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

#endregion

#region Configure Serilog

string logpath = builder.Configuration.GetSection("Logging:LogPath").Value!;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logpath)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

#endregion

#region Configure CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

#endregion

var app = builder.Build();

#region Configure NestedDB Connection

var _GetDynamicConnectionString = builder.Configuration.GetConnectionString("connDYNAMIC")!.ToString();
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var WOTservices = services.GetService<WOTCOREContext>();
    var connLIST = WOTservices!.WotConnections.Where(x => x.Active == true)
        .Select(x => new WOTCTName
        {
            ContextName = x.ConnId,
            ConnectString = _GetDynamicConnectionString.Replace("{0}", x.ServerName)
                                                        .Replace("{1}", x.DbName)
                                                        .Replace("{2}", x.UserId)
                                                        .Replace("{3}", x.Passcode)
        }).ToList();

    WOTCSModel.ConnectionStrings = connLIST;
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

