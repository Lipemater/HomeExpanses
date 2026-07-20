import { httpRequest } from '../api/httpClient';
import type { ConsultaTotais } from '../types';

export const totaisService = {
  consultar(): Promise<ConsultaTotais> {
    return httpRequest<ConsultaTotais>(
      '/totais',
    );
  },
};