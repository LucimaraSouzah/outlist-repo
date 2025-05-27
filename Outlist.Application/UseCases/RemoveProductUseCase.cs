using Outlist.Application.Interface;
using Microsoft.Extensions.Logging;

namespace Outlist.Application.UseCases;

/// <summary>
/// Caso de uso responsável por remover um produto da Outlist, identificando-o pelo ProductId.
/// </summary>
public class RemoveProductUseCase
{
    private readonly IOutlistRepository _repository;
    private readonly ILogger<RemoveProductUseCase> _logger;

    public RemoveProductUseCase(IOutlistRepository repository, ILogger<RemoveProductUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Executa a remoção do produto da Outlist com o identificador informado.
    /// </summary>
    /// <param name="productId">Identificador único do produto a ser removido.</param>
    /// <returns>Task representando a operação assíncrona.</returns>
    /// <exception cref="Exception">Lança exceção em caso de falha na remoção do produto.</exception>
    public async Task ExecuteAsync(Guid productId)
    {
        _logger.LogInformation("Starting removal of product from Outlist. ProductId: {ProductId}", productId);

        try
        {
            await _repository.RemoveProductAsync(productId);
            _logger.LogInformation("Product successfully removed. ProductId: {ProductId}", productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove product from Outlist. ProductId: {ProductId}", productId);
            throw;
        }
    }
}
