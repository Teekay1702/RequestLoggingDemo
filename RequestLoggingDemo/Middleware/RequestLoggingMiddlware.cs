using System.Diagnostics;
using System.Text.Json;
using RequestLoggingDemo.Data;
using RequestLoggingDemo.Models;


namespace RequestLoggingDemo.Middleware
{
    public class RequestLoggingMiddlware
    {
        private readonly RequestDelegate _next;

        private readonly IServiceScopeFactory _scopeFactory;

        public RequestLoggingMiddlware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        private async Task LogToDatabaseAsync(LoggingDbContext dbContext, string logType, RequestLog logData)
        {
            if (logType == "Request")
            {
                await dbContext.RequestLogs.AddAsync(logData);
            }
            else if (logType == "Response")
            {
                dbContext.RequestLogs.Update(logData);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Start a stopwatch to measure response time
            var stopwatch = Stopwatch.StartNew();

            //Log the request details
            var requestLog = new RequestLog
            {
                HttpMethod = context.Request.Method,
                Url = context.Request.Path,
                Headers = JsonSerializer.Serialize(context.Request.Headers),
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                ResponseTime = "",
                Timestamp = DateTime.UtcNow,
            };

            await LogToFileAsync("Request", requestLog);

            //Log request to database
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
                await LogToDatabaseAsync(dbContext, "Request", requestLog);
            }

            //Pass Control to the next middleware
            await _next(context);

            //Stop the stopwatch and log response details
            stopwatch.Stop();
            var responseLog = new
            {
                StatusCode = context.Response.StatusCode,
                ResponseTime = stopwatch.ElapsedMilliseconds,
            };
            await LogToFileAsync("Response", responseLog);

            //Log response to database
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
                requestLog.ResponseTime = $"{stopwatch.ElapsedMilliseconds} ms";
                await LogToDatabaseAsync(dbContext, "Response", requestLog);
            }
        }

        private async Task LogToFileAsync(string logType, object logData)
        {
            var logMessage = $"{logType} : {JsonSerializer.Serialize(logData)}\n";
            await File.AppendAllTextAsync("logs.txt", logMessage);
        }

    }
}
