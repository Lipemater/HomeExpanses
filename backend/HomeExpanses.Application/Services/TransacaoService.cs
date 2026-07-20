using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Transacoes;
using HomeExpanses.Application.Exceptions;
using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.Services
{
    public sealed class TransacaoService : ITransacaoService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransacaoService(
            IPessoaRepository pessoaRepository,
            ITransacaoRepository transacaoRepository,
            IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _transacaoRepository = transacaoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TransacaoResponse>> ListarAsync(CancellationToken cancellationToken = default)
        {
            var transacoes = await _transacaoRepository.ListarAsync(cancellationToken);
            return transacoes.Select(Mapear).ToList();
        }

        public async Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request, CancellationToken cancellationToken = default)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(request.PessoaId, cancellationToken);
            if (pessoa is null)
                throw new RecursoNaoEncontradoException($"Pessoa com id {request.PessoaId} não encontrada.");

            var transacao = Transacao.Criar (
                pessoa,
                request.Descricao,
                request.Valor,
                request.Tipo);

            await _transacaoRepository.AdicionarAsync(transacao, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Mapear(transacao);
        }

        private static TransacaoResponse Mapear(Transacao transacao)
        {
            return new TransacaoResponse(
                transacao.Id,
                transacao.Descricao,
                transacao.Valor,
                transacao.Tipo,
                transacao.PessoaId,
                transacao.Pessoa.Nome
            );
        }
    }
}