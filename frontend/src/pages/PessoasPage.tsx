import {
  useEffect,
  useState,
  type SubmitEvent,
} from 'react';

import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingMessage } from '../components/LoadingMessage';
import { pessoasService } from '../services/pessoasService';
import type { Pessoa } from '../types';
import { obterMensagemErro } from '../utils/error';

export function PessoasPage() {
  const [pessoas, setPessoas] =
    useState<Pessoa[]>([]);

  const [nome, setNome] =
    useState('');

  const [idade, setIdade] =
    useState('');

  const [carregando, setCarregando] =
    useState(true);

  const [salvando, setSalvando] =
    useState(false);

  const [excluindoId, setExcluindoId] =
    useState<number | null>(null);

  const [erro, setErro] =
    useState<string | null>(null);

  const [mensagem, setMensagem] =
    useState<string | null>(null);

  useEffect(() => {
    let componenteAtivo = true;

    async function carregarPessoas() {
      try {
        const dados = await pessoasService.listar();

        if (componenteAtivo) {
          const dadosOrdenados = [...dados].sort((a, b) => a.id - b.id);
          setPessoas(dadosOrdenados);
        }
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

    void carregarPessoas();

    return () => {
      componenteAtivo = false;
    };
  }, []);

  async function handleSubmit(
    event: SubmitEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setErro(null);
    setMensagem(null);

    const nomeNormalizado = nome.trim();
    const idadeNumerica = Number(idade);

    if (nomeNormalizado.length < 2) {
      setErro(
        'O nome deve possuir pelo menos 2 caracteres.',
      );
      return;
    }

    if (
      idade.trim() === '' ||
      !Number.isInteger(idadeNumerica) ||
      idadeNumerica < 0 ||
      idadeNumerica > 120
    ) {
      setErro(
        'Informe uma idade inteira entre 0 e 120.',
      );
      return;
    }

    setSalvando(true);

    try {
      const pessoaCriada =
        await pessoasService.criar({
          nome: nomeNormalizado,
          idade: idadeNumerica,
        });

      setPessoas((pessoasAtuais) => {
        const listaAtualizada = [...pessoasAtuais, pessoaCriada];
        return listaAtualizada.sort((a, b) => a.id - b.id);
      });

      setNome('');
      setIdade('');

      setMensagem(
        'Pessoa cadastrada com sucesso.',
      );
    } catch (error) {
      setErro(
        obterMensagemErro(error),
      );
    } finally {
      setSalvando(false);
    }
  }

  async function handleExcluir(
    pessoa: Pessoa,
  ) {
    const confirmou = window.confirm(
      `Excluir ${pessoa.nome} e todas as suas transações?`,
    );

    if (!confirmou) {
      return;
    }

    setErro(null);
    setMensagem(null);
    setExcluindoId(pessoa.id);

    try {
      await pessoasService.excluir(
        pessoa.id,
      );

      const pessoasAtualizadas =
        await pessoasService.listar();

      const pessoasAtualizadasOrdenadas = [...pessoasAtualizadas].sort((a, b) => a.id - b.id);
      setPessoas(pessoasAtualizadasOrdenadas);

      setMensagem(
        'Pessoa e suas transações foram excluídas.',
      );
    } catch (error) {
      setErro(
        obterMensagemErro(error),
      );
    } finally {
      setExcluindoId(null);
    }
  }

  return (
    <section className="content-card">
      <div className="section-title"><span className="section-icon">+</span><div><span className="eyebrow">MORADORES</span><h2>Pessoas</h2><p>Cadastre e gerencie quem participa das finanças da casa.</p></div></div>

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="nome">
            Nome
          </label>

          <input
            id="nome"
            name="nome"
            type="text"
            value={nome}
            onChange={(event) =>
              setNome(event.target.value)
            }
            minLength={2}
            maxLength={100}
            required
            disabled={salvando}
          />
        </div>

        <div>
          <label htmlFor="idade">
            Idade
          </label>

          <input
            id="idade"
            name="idade"
            type="number"
            value={idade}
            onChange={(event) =>
              setIdade(event.target.value)
            }
            min={0}
            max={120}
            step={1}
            required
            disabled={salvando}
          />
        </div>

        <button
          type="submit"
          disabled={salvando}
        >
          {salvando
            ? 'Cadastrando...'
            : 'Cadastrar pessoa'}
        </button>
      </form>

      <ErrorMessage message={erro} />

      {mensagem && (
        <p role="status">
          {mensagem}
        </p>
      )}

      <h3><span>Pessoas cadastradas</span><small>{pessoas.length} no total</small></h3>

      {carregando ? (
        <LoadingMessage
          message="Carregando pessoas..."
        />
      ) : pessoas.length === 0 ? (
        <p>
          Nenhuma pessoa cadastrada.
        </p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Idade</th>
              <th>Ações</th>
            </tr>
          </thead>

          <tbody>
            {pessoas.map((pessoa) => (
              <tr key={pessoa.id}>
                <td>{pessoa.id}</td>
                <td>{pessoa.nome}</td>
                <td>{pessoa.idade}</td>
                <td>
                  <button
                    type="button"
                    onClick={() =>
                      handleExcluir(pessoa)
                    }
                    disabled={
                      excluindoId !== null
                    }
                  >
                    {excluindoId === pessoa.id
                      ? 'Excluindo...'
                      : 'Excluir'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
