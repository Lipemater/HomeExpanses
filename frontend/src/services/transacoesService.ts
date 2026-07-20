import { httpRequest } from '../api/httpClient';
import type {
  CriarTransacaoRequest,
  Transacao,
} from '../types';

export const transacoesService = {
  listar(): Promise<Transacao[]> {
    return httpRequest<Transacao[]>(
      '/transacoes',
    );
  },

  criar(
    request: CriarTransacaoRequest,
  ): Promise<Transacao> {
    return httpRequest<Transacao>(
      '/transacoes',
      {
        method: 'POST',
        body: JSON.stringify(request),
      },
    );
  },
};