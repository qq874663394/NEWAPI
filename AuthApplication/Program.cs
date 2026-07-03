using AuthApplication.Extensions;
using DependencyInjection;
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

            // ========== 替换内置 OpenAPI，使用 Swagger ==========
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AuthApplication API",
                    Version = "v1",
                    Description = "新框架认证授权服务"
                });

                // 包含 XML 注释（如果启用了 GenerateDocumentationFile）
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // 定义 Bearer 安全方案
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入 JWT Token（无需添加 Bearer 前缀）",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // 全局应用安全要求（新版 API 需要传入 Func 委托）
                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            // ========== Swagger 中间件 ==========
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
                c.RoutePrefix = string.Empty;   // 直接访问根路径打开 Swagger UI
            });

            // 你原有的中间件
            if (app.Environment.IsDevelopment())
            {
                // 不再需要 app.MapOpenApi(); 可以删除
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