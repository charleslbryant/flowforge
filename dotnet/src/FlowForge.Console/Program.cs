using FlowForge.Console.Commands;
using FlowForge.Console.Services.HealthChecking;
using FlowForge.Console.Services.ProcessManagement;
using FlowForge.Console.Services.SystemChecking;
using FlowForge.Console.Infrastructure.Http;
using FlowForge.Console.Infrastructure.Process;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace FlowForge.Console;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        // Create service collection
        var services = new ServiceCollection();
        
        // Configure services
        services.AddLogging();
        services.AddHttpClient();
        
        // Register infrastructure services
        services.AddScoped<IN8nHttpClient, N8nHttpClient>();
        services.AddScoped<IProcessExecutor, ProcessExecutor>();
        
        // Register business logic services
        services.AddScoped<IHealthChecker, HealthChecker>();
        services.AddScoped<ISystemChecker, SystemChecker>();
        services.AddScoped<IProcessManager, ProcessManager>();
        
        // Create the command app
        var app = new CommandApp(new TypeRegistrar(services));
        
        app.Configure(config =>
        {
            config.SetApplicationName("forge-dotnet");
            
            config.AddCommand<HealthCommand>("health")
                .WithDescription("Check n8n status");
                
            config.AddCommand<DoctorCommand>("doctor")
                .WithDescription("Full system health check");
                
            config.AddCommand<StartCommand>("start")
                .WithDescription("Start n8n in background");
                
            config.AddCommand<StopCommand>("stop")
                .WithDescription("Stop n8n process");
                
            config.AddCommand<RestartCommand>("restart")
                .WithDescription("Restart n8n process");
        });

        return await app.RunAsync(args);
    }
}

// Simple type registrar for dependency injection
public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _services;

    public TypeRegistrar(IServiceCollection services)
    {
        _services = services;
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(_services.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        _services.AddScoped(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services.AddScoped(service, _ => factory());
    }
}

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        return _provider.GetService(type);
    }

    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}