import {
  useCallback,
  useEffect,
  useState,
} from 'react';

import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingMessage } from '../components/LoadingMessage';
import { totaisService } from '../services/totaisService';
import type { ConsultaTotais } from '../types';
import { obterMensagemErro } from '../utils/error';
import { formatarMoeda } from '../utils/money';

export function TotaisPage() {
  const [consulta, setConsulta] =
    useState<ConsultaTotais | null>(
      null,
    );

  const [carregando, setCarregando] =
    useState(true);

  const [erro, setErro] =
    useState<string | null>(null);

  const carregarTotais =
    useCallback(async () => {
      setCarregando(true);
      setErro(null);

      try {
        const dados =
          await totaisService.consultar();

        setConsulta(dados);
      } catch (error) {
        setErro(
          obterMensagemErro(error),
        );
      } finally {
        setCarregando(false);
      }
    }, []);

  useEffect(() => {
    const carregamentoId = window.setTimeout(
      () => void carregarTotais(),
      0,
    );

    return () => window.clearTimeout(carregamentoId);
  }, [carregarTotais]);

  return (
    <section className="content-card totals-card">
      <div className="section-title"><span className="section-icon">⌁</span><div><span className="eyebrow">BALANÇO DA CASA</span><h2>Totais</h2><p>Acompanhe receitas, despesas e o saldo de cada pessoa.</p></div></div>

      <button
        type="button"
        onClick={() =>
          void carregarTotais()
        }
        disabled={carregando}
      >
        {carregando
          ? 'Atualizando...'
          : 'Atualizar totais'}
      </button>

      <ErrorMessage message={erro} />

      {carregando && !consulta ? (
        <LoadingMessage
          message="Calculando totais..."
        />
      ) : consulta === null ? (
        <p>
          Não foi possível carregar os
          totais.
        </p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Pessoa</th>
              <th>Receitas</th>
              <th>Despesas</th>
              <th>Saldo</th>
            </tr>
          </thead>

          <tbody>
            {consulta.pessoas.map(
              (pessoa) => (
                <tr key={pessoa.pessoaId}>
                  <td>{pessoa.nome}</td>

                  <td>
                    {formatarMoeda(
                      pessoa.totalReceitas,
                    )}
                  </td>

                  <td>
                    {formatarMoeda(
                      pessoa.totalDespesas,
                    )}
                  </td>

                  <td>
                    {formatarMoeda(
                      pessoa.saldo,
                    )}
                  </td>
                </tr>
              ),
            )}
          </tbody>

          <tfoot>
            <tr>
              <th>Total geral</th>

              <th>
                {formatarMoeda(
                  consulta.totalGeral
                    .totalReceitas,
                )}
              </th>

              <th>
                {formatarMoeda(
                  consulta.totalGeral
                    .totalDespesas,
                )}
              </th>

              <th>
                {formatarMoeda(
                  consulta.totalGeral
                    .saldo,
                )}
              </th>
            </tr>
          </tfoot>
        </table>
      )}
    </section>
  );
}
