using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Outlist.Application.DTOs;
using TechTalk.SpecFlow;

namespace OutlistTests.BDD.Steps
{
    [Binding]
    public class OutlistSteps : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private HttpResponseMessage? _response;
        private Guid _productId;

        public OutlistSteps(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
        }

        [Given(@"que eu tenho um produto válido para adicionar")]
        public void GivenQueEuTenhoUmProdutoValidoParaAdicionar()
        {
            _productId = Guid.NewGuid();
        }

        [When(@"eu envio a requisição de adição")]
        public async Task WhenEuEnvioARequisicaoDeAdicao()
        {
            var dto = new AddOutlistRequest
            {
                ProductId = _productId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7)
            };

            _response = await _client.PostAsJsonAsync("/api/outlist", dto);
        }

        [Then(@"o produto deve ser adicionado com sucesso")]
        public void ThenOProdutoDeveSerAdicionadoComSucesso()
        {
            _response!.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Given(@"que existe um produto adicionado")]
        public async Task GivenQueExisteUmProdutoAdicionado()
        {
            _productId = Guid.NewGuid();
            var dto = new AddOutlistRequest
            {
                ProductId = _productId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7)
            };
            await _client.PostAsJsonAsync("/api/outlist", dto);
        }

        [When(@"eu envio a requisição de remoção")]
        public async Task WhenEuEnvioARequisicaoDeRemocao()
        {
            _response = await _client.DeleteAsync($"/api/outlist/{_productId}");
        }

        [Then(@"o produto deve ser removido com sucesso")]
        public void ThenOProdutoDeveSerRemovidoComSucesso()
        {
            _response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [When(@"eu envio a requisição de atualização de vigência")]
        public async Task WhenEuEnvioARequisicaoDeAtualizacaoDeVigencia()
        {
            var updateDto = new UpdateValidityRequest
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            _response = await _client.PutAsJsonAsync($"/api/outlist/{_productId}", updateDto);
        }

        [Then(@"a vigência deve ser atualizada com sucesso")]
        public void ThenAVigenciaDeveSerAtualizadaComSucesso()
        {
            _response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [When(@"eu envio a requisição de listagem")]
        public async Task WhenEuEnvioARequisicaoDeListagem()
        {
            _response = await _client.GetAsync("/api/outlist?page=1&pageSize=20");
        }

        [Then(@"eu devo receber a lista de produtos bloqueados")]
        public async Task ThenEuDevoReceberAListaDeProdutosBloqueados()
        {
            _response!.StatusCode.Should().Be(HttpStatusCode.OK);
            var list = await _response.Content.ReadFromJsonAsync<List<OutlistProductDto>>();
            list.Should().NotBeNull();
        }

        [When(@"eu envio a requisição de verificação")]
        public async Task WhenEuEnvioARequisicaoDeVerificacao()
        {
            _response = await _client.GetAsync($"/api/outlist/check/{_productId}");
        }

        [Then(@"eu devo receber a confirmação de bloqueio")]
        public async Task ThenEuDevoReceberAConfirmacaoDeBloqueio()
        {
            _response!.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await _response.Content.ReadFromJsonAsync<bool>();
            result.Should().BeTrue();
        }
    }
}
