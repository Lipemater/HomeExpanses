import { httpRequest } from '../api/httpClient';
import type {
  CriarPessoaRequest,
  Pessoa,
} from '../types';

export const pessoasService = {
  listar(): Promise<Pessoa[]> {
    return httpRequest<Pessoa[]>('/pessoas');
  },

  criar(
    request: CriarPessoaRequest,
  ): Promise<Pessoa> {
    return httpRequest<Pessoa>(
      '/pessoas',
      {
        method: 'POST',
        body: JSON.stringify(request),
      },
    );
  },

  excluir(id: number): Promise<void> {
    return httpRequest<void>(
      `/pessoas/${id}`,
      {
        method: 'DELETE',
      },
    );
  },
};