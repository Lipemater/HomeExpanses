using HomeExpanses.Application.Abstractions.Persistence;
using HomeExpanses.Application.Abstractions.Services;
using HomeExpanses.Application.DTOs.Pessoas;
using HomeExpanses.Application.Exceptions;
using HomeExpanses.Domain.Entities;

namespace HomeExpanses.Application.Services
{
    public sealed class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PessoaService(IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<PessoaResponse>> ListarAsync(CancellationToken cancellationToken = default)
        {
            var pessoas = await _pessoaRepository.ListarAsync(cancellationToken);
            return pessoas.Select(Mapear).ToList();    
        }

        public async Task<PessoaResponse> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id, cancellationToken);
            if (pessoa is null)
                throw new RecursoNaoEncontradoException($"Pessoa com id {id} não encontrada.");

            return Mapear(pessoa);
        }

        public async Task<PessoaResponse> CriarAsync(CriarPessoaRequest request, CancellationToken cancellationToken = default)
        {
            var pessoa = new Pessoa(request.Nome, request.Idade);
            await _pessoaRepository.AdicionarAsync(pessoa, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Mapear(pessoa);
        }

        public async Task ExcluirAsync(int id, CancellationToken cancellationToken = default)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(id, cancellationToken);
            if (pessoa is null)
                throw new RecursoNaoEncontradoException($"Pessoa com id {id} não encontrada.");

            /*
            * A remoção das transações é executada pelo banco por meio
            * da configuração de exclusão em cascata da Infrastructure.
            */

            _pessoaRepository.Remover(pessoa);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _pessoaRepository.ReordenarIdsAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static PessoaResponse Mapear(Pessoa pessoa)
        {
            return new PessoaResponse(
                pessoa.Id,
                pessoa.Nome,
                pessoa.Idade);
        }
    }
}