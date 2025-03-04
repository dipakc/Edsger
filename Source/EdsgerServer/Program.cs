﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;


namespace EdsgerServer
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var server = await LanguageServer.From(options =>
        options
          .WithInput(Console.OpenStandardInput())
          .WithOutput(Console.OpenStandardOutput())
          .WithLoggerFactory(new LoggerFactory())
          .AddDefaultLoggingProvider()
          // .WithMinimumLogLevel(LogLevel.Trace)
          .WithServices(ConfigureServices)
          .WithHandler<TextDocumentSyncHandler>()
          // .WithHandler<CompletionHandler>()
          
      );

      await server.WaitForExit;
    }
    static void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<BufferManager>();
    }

  }
}