using HomeExpanses.Application.Exceptions;
using HomeExpanses.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HomeExpanses.Api.ErrorHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, titulo) = exception switch
            {
                RecursoNaoEncontradoException => 
                    (
                        StatusCodes.Status404NotFound, "Recurso não encontrado"
                    ), 
                
                RegraNegocioException =>
                    (
                        StatusCodes.Status400BadRequest,
                        "Regra de negócio inválida"
                    ),

                _ =>
                    (
                        StatusCodes.Status500InternalServerError,
                        "Erro interno do servidor"
                    )
            };

            if (statusCode >= 500)
            {
                _logger.LogError(exception, "Erro não tratado durante a requisição.");
            }
            else
            {
                _logger.LogWarning(exception, "A requisição foi rejeitada.");
            }

            var detalhe = statusCode >= 500 ? "Ocorreu um erro inesperado" : exception.Message;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = titulo,
                Detail = detalhe,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}