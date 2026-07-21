import {
  useEffect,
  useState,
  type ChangeEvent,
  type SubmitEvent,
} from 'react';

import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingMessage } from '../components/LoadingMessage';
import { pessoasService } from '../services/pessoasService';
import { transacoesService } from '../services/transacoesService';
import type {
  Pessoa,
  TipoTransacao,
  Transacao,
} from '../types';
import { obterMensagemErro } from '../utils/error';
import { formatarMoeda } from '../utils/money';

export function TransacoesPage() {
  const [pessoas, setPessoas] =
    useState<Pessoa[]>([]);

  const [transacoes, setTransacoes] =
    useState<Transacao[]>([]);

  const [descricao, setDescricao] =
    useState('');

  const [valor, setValor] =
    useState('');

  const [tipo, setTipo] =
    useState<TipoTransacao>('Despesa');

  const [pessoaId, setPessoaId] =
    useState('');

  const [carregando, setCarregando] =
    useState(true);

  const [salvando, setSalvando] =
    useState(false);

  const [erro, setErro] =
    useState<string | null>(null);

  const [mensagem, setMensagem] =
    useState<string | null>(null);

  const pessoaSelecionada =
    pessoas.find(
      (pessoa) =>
        pessoa.id === Number(pessoaId),
    );

  const pessoaMenor =
    pessoaSelecionada !== undefined &&
    pessoaSelecionada.idade < 18;

  useEffect(() => {
    let componenteAtivo = true;

    async function carregarDados() {
      try {
        const [
          pessoasRecebidas,
          transacoesRecebidas,
        ] = await Promise.all([
          pessoasService.listar(),
          transacoesService.listar(),
        ]);

        if (!componenteAtivo) {
          return;
        }

        setPessoas(pessoasRecebidas);
        setTransacoes(
          transacoesRecebidas,
        );

        
      } catch (error) {
        if (componenteAtivo) {
          setErro(
            obterMensagemErro(error),
          );
        }
      } finally {
        if (componenteAtivo) {
          setCarregando(false);
        }
      }
    }

    void carregarDados();

    return () => {
      componenteAtivo = false;
    };
  }, []);

  function handlePessoaChange(
    event: ChangeEvent<HTMLSelectElement>,
  ) {
    const novoPessoaId =
      event.target.value;

    setPessoaId(novoPessoaId);

    const novaPessoa =
      pessoas.find(
        (pessoa) =>
          pessoa.id === Number(novoPessoaId),
      );

    if (
      novaPessoa !== undefined &&
      novaPessoa.idade < 18
    ) {
      setTipo('Despesa');
    }
  }

  async function handleSubmit(
    event: SubmitEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setErro(null);
    setMensagem(null);

    const descricaoNormalizada =
      descricao.trim();

    const valorNumerico = Number(
      valor.replace(',', '.'),
    );

    if (!pessoaSelecionada) {
      setErro(
        'Selecione uma pessoa válida.',
      );
      return;
    }

    if (
      descricaoNormalizada.length < 2
    ) {
      setErro(
        'A descrição deve possuir pelo menos 2 caracteres.',
      );
      return;
    }

    if (
      !Number.isFinite(valorNumerico) ||
      valorNumerico <= 0
    ) {
      setErro(
        'Informe um valor maior que zero.',
      );
      return;
    }

    if (
      pessoaSelecionada.idade < 18 &&
      tipo === 'Receita'
    ) {
      setErro(
        'Pessoas menores de 18 anos não podem cadastrar receitas.',
      );
      return;
    }

    setSalvando(true);

    try {
      const transacaoCriada =
        await transacoesService.criar({
          descricao:
            descricaoNormalizada,
          valor: valorNumerico,
          tipo,
          pessoaId:
            pessoaSelecionada.id,
        });

      setTransacoes(
        (transacoesAtuais) => [
          transacaoCriada,
          ...transacoesAtuais,
        ],
      );

      setDescricao('');
      setValor('');

      if (pessoaMenor) {
        setTipo('Despesa');
      }

      setMensagem(
        'Transação cadastrada com sucesso.',
      );
    } catch (error) {
      setErro(
        obterMensagemErro(error),
      );
    } finally {
      setSalvando(false);
    }
  }

  return (
    <section className="content-card">
      <div className="section-title"><span className="section-icon">↗</span><div><span className="eyebrow">MOVIMENTAÇÕES</span><h2>Transações</h2><p>Registre cada entrada e saída em poucos segundos.</p></div></div>

      {pessoas.length === 0 &&
        !carregando && (
          <p>
            Cadastre uma pessoa antes de
            registrar transações.
          </p>
        )}

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="descricao">
            Descrição
          </label>

          <input
            id="descricao"
            name="descricao"
            type="text"
            value={descricao}
            onChange={(event) =>
              setDescricao(
                event.target.value,
              )
            }
            minLength={2}
            maxLength={200}
            required
            disabled={
              salvando ||
              pessoas.length === 0
            }
          />
        </div>

        <div>
          <label htmlFor="valor">
            Valor
          </label>

          <input
            id="valor"
            name="valor"
            type="number"
            value={valor}
            onChange={(event) =>
              setValor(event.target.value)
            }
            min={0.01}
            step={0.01}
            required
            disabled={
              salvando ||
              pessoas.length === 0
            }
          />
        </div>

        <div>
          <label htmlFor="pessoa">
            Pessoa
          </label>

          <select
            id="pessoa"
            name="pessoa"

            value={pessoaId}
            onChange={handlePessoaChange}
            required
            disabled={
              salvando ||
              pessoas.length === 0
            }
          >
            <option value="" disabled>
              Selecionar
            </option>
            

            {pessoas.map((pessoa) => (
              <option
                key={pessoa.id}
                value={pessoa.id}
              >
                {pessoa.nome} —{' '}
                {pessoa.idade} anos
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="tipo">
            Tipo
          </label>

          <select
            id="tipo"
            name="tipo"
            value={tipo}
            onChange={(event) =>
              setTipo(
                event.target
                  .value as TipoTransacao,
              )
            }
            disabled={
              salvando ||
              pessoas.length === 0
            }
          >
            <option value="Despesa">
              Despesa
            </option>

            <option
              value="Receita"
              disabled={pessoaMenor}
            >
              Receita
            </option>
          </select>
        </div>

        {pessoaMenor && (
          <p>
            Pessoas menores de 18 anos
            podem cadastrar apenas despesas.
          </p>
        )}

        <button
          type="submit"
          disabled={
            salvando ||
            pessoas.length === 0
          }
        >
          {salvando
            ? 'Cadastrando...'
            : 'Cadastrar transação'}
        </button>
      </form>

      <ErrorMessage message={erro} />

      {mensagem && (
        <p role="status">
          {mensagem}
        </p>
      )}

      <h3><span>Transações cadastradas</span><small>{transacoes.length} no total</small></h3>

      {carregando ? (
        <LoadingMessage
          message="Carregando transações..."
        />
      ) : transacoes.length === 0 ? (
        <p>
          Nenhuma transação cadastrada.
        </p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Pessoa</th>
            </tr>
          </thead>

          <tbody>
            {transacoes.map(
              (transacao) => (
                <tr key={transacao.id}>
                  <td>{transacao.id}</td>

                  <td>
                    {transacao.descricao}
                  </td>

                  <td>
                    {formatarMoeda(
                      transacao.valor,
                    )}
                  </td>

                  <td>
                    {transacao.tipo}
                  </td>

                  <td>
                    {transacao.pessoaNome}
                  </td>
                </tr>
              ),
            )}
          </tbody>
        </table>
      )}
    </section>
  );
}
