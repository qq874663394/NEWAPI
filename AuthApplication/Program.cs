using AuthApplication.Extensions;
using DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;

namespace AuthApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var configuration = builder.Configuration;
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddApplicationModules(builder.Configuration);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                        .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            // ========== �滻���� OpenAPI��ʹ�� Swagger ==========
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AuthApplication API",
                    Version = "v1",
                    Description = "�¿����֤��Ȩ����"
                });

                // ���� XML ע�ͣ���������� GenerateDocumentationFile��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // ���� Bearer ��ȫ����
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "������ JWT Token��������� Bearer ǰ׺��",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // ȫ��Ӧ�ð�ȫҪ���°� API ��Ҫ���� Func ί�У�
                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                });
            });

             // =========================
             // JWT Bearer 认证中间件配置
             // =========================
             var jwtSection = configuration.GetSection("Jwt");
             var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("未配置 Jwt:Secret");
             var issuer = jwtSection["Issuer"] ?? "AuthApplication";
             var audience = jwtSection["Audience"] ?? "AuthApplication";
             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

             builder.Services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,
                     ValidateLifetime = true,
                     ValidIssuer = issuer,
                     ValidAudience = audience,
                     IssuerSigningKey = key,
                     ClockSkew = TimeSpan.Zero
                 };
             });

            var app = builder.Build();

            // ========== Swagger �м�� ==========
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
                c.RoutePrefix = string.Empty;   // ֱ�ӷ��ʸ�·���� Swagger UI
            });

            // ��ԭ�е��м��
            if (app.Environment.IsDevelopment())
            {
                // ������Ҫ app.MapOpenApi(); ����ɾ��
            }
            app.UseHttpsRedirection();
            app.UseGlobalException();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}