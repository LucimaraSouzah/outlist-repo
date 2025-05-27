using Outlist.Application.Interface;
using Microsoft.Extensions.Logging;

namespace Outlist.Application.UseCases;

/// <summary>
/// Caso de uso responsável por verificar se um produto está bloqueado na Outlist,
/// impedindo alterações de preço durante o período de vigência.
/// </summary>
public class CheckProductBlockedUseCase
{
    private readonly IOutlistRepository _repository;
    private readonly ILogger<CheckProductBlockedUseCase> _logger;

    public CheckProductBlockedUseCase(IOutlistRepository repository, ILogger<CheckProductBlockedUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Executa a verificação se o produto informado está bloqueado na Outlist.
    /// </summary>
    /// <param name="productId">Identificador único do produto.</param>
    /// <returns>Retorna <c>true</c> se o produto estiver bloqueado, <c>false</c> caso contrário.</returns>
    public async Task<bool> ExecuteAsync(Guid productId)
    {
        _logger.LogInformation("Checking if product {ProductId} is blocked.", productId);

        bool isBlocked = await _repository.IsProductBlockedAsync(productId);

        return isBlocked;
    }
}
