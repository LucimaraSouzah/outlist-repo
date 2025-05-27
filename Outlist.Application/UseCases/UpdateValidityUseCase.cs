using Outlist.Application.Interface;
using Microsoft.Extensions.Logging;

namespace Outlist.Application.UseCases;

/// <summary>
/// Caso de uso responsável por atualizar o período de vigência de um produto na Outlist.
/// </summary>
public class UpdateValidityUseCase
{
    private readonly IOutlistRepository _repository;
    private readonly ILogger<UpdateValidityUseCase> _logger;

    public UpdateValidityUseCase(IOutlistRepository repository, ILogger<UpdateValidityUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Executa a atualização da vigência de um produto na Outlist.
    /// </summary>
    /// <param name="productId">Identificador único do produto que terá a vigência atualizada.</param>
    /// <param name="startDate">Data de início da nova vigência.</param>
    /// <param name="endDate">Data de término da nova vigência.</param>
    /// <returns>Task representando a operação assíncrona.</returns>
    /// <exception cref="Exception">Lança exceção caso ocorra erro na atualização.</exception>
    public async Task ExecuteAsync(Guid productId, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Updating product validity in Outlist. ProductId: {ProductId}, StartDate: {StartDate}, EndDate: {EndDate}", productId, startDate, endDate);

        try
        {
            await _repository.UpdateValidityAsync(productId, startDate, endDate);
            _logger.LogInformation("Product validity successfully updated. ProductId: {ProductId}", productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product validity in Outlist. ProductId: {ProductId}", productId);
            throw;
        }
    }
}
