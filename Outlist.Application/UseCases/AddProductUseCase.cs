using Outlist.Application.Interface;
using Microsoft.Extensions.Logging;
using Outlist.Domain.Entities;

namespace Outlist.Application.UseCases;

/// <summary>
/// Caso de uso responsável por adicionar um produto à Outlist para bloqueio de alterações de preço durante um período definido.
/// </summary>
public class AddProductUseCase
{
    private readonly IOutlistRepository _repository;
    private readonly ILogger<AddProductUseCase> _logger;

    public AddProductUseCase(IOutlistRepository repository, ILogger<AddProductUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Executa a adição de um produto à Outlist com o período de vigência especificado.
    /// </summary>
    /// <param name="productId">Identificador único do produto a ser adicionado.</param>
    /// <param name="startDate">Data de início da vigência do bloqueio.</param>
    /// <param name="endDate">Data de término da vigência do bloqueio.</param>
    /// <returns>Task representando a operação assíncrona.</returns>
    /// <exception cref="ArgumentException">Lançada quando o ProductId é vazio ou a data de término é anterior ou igual à data de início.</exception>
    public async Task ExecuteAsync(Guid productId, DateTime startDate, DateTime endDate)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(productId));

        if (endDate <= startDate)
            throw new ArgumentException("EndDate must be greater than StartDate.");

        var entry = new OutlistEntry
        {
            ProductId = productId,
            StartDate = startDate,
            EndDate = endDate
        };

        _logger.LogInformation("Adding product {ProductId} to Outlist with validity {StartDate} - {EndDate}", productId, startDate, endDate);

        await _repository.AddProductAsync(entry);
    }
}
