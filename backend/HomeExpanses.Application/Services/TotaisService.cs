using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Totais;

namespace HomeExpanses.Application.Services
{
    public sealed class TotaisService : ITotaisService
    {
        private readonly ITotaisRepository _totaisRepository;

        public TotaisService(ITotaisRepository totaisRepository)
        {
            _totaisRepository = totaisRepository;
        }

        public async Task<ConsultaTotaisResponse> ConsultarAsync(CancellationToken cancellationToken = default)
        {
            var totais = await _totaisRepository.ConsultarAsync(cancellationToken);
            
            var pessoas = totais.Select(item =>
            {
                var receitas = ConverterParaDecimal(item.TotalReceitasEmCentavos);
                var despesas = ConverterParaDecimal(item.TotalDespesasEmCentavos);

                return new TotalPessoaResponse(item.PessoaId, item.Nome, receitas, despesas, receitas - despesas);

            }).ToList();

            var totalReceitas = pessoas.Sum(pessoa => pessoa.TotalReceitas);

            var totalDespesas = pessoas.Sum(pessoa => pessoa.TotalDespesas);

            var totalGeral = new TotalGeralResponse(totalReceitas, totalDespesas, totalReceitas - totalDespesas);

            return new ConsultaTotaisResponse(pessoas, totalGeral);

        }

        private static decimal ConverterParaDecimal(long valorEmCentavos)
        {
            return valorEmCentavos / 100m;
        }
    }
}