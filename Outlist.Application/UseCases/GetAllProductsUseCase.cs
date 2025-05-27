using Outlist.Application.Interface;
using Outlist.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Outlist.Application.UseCases;

/// <summary>
/// Caso de uso responsável por recuperar uma lista paginada de produtos
/// cadastrados na Outlist, respeitando os parâmetros de paginação.
/// </summary>
public class GetAllProductsUseCase
{
    private readonly IOutlistRepository _repository;
    private readonly ILogger<GetAllProductsUseCase> _logger;

    public GetAllProductsUseCase(IOutlistRepository repository, ILogger<GetAllProductsUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Executa a busca paginada dos produtos cadastrados na Outlist.
    /// </summary>
    /// <param name="page">Número da página a ser recuperada.</param>
    /// <param name="pageSize">Quantidade máxima de produtos por página.</param>
    /// <returns>Lista de produtos da Outlist para a página solicitada.</returns>
    /// <exception cref="Exception">Lança exceção em caso de erro durante o acesso aos dados.</exception>
    public async Task<List<OutlistEntry>> ExecuteAsync(int page, int pageSize)
    {
        _logger.LogInformation("Fetching Outlist products - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        try
        {
            var products = await _repository.GetAllProductsAsync(page, pageSize);
            _logger.LogInformation("{Count} products retrieved.", products.Count);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching Outlist products.");
            throw;
        }
    }
}
