import type { ApiProblemDetails } from '../types';
import { ApiError } from './ApiError';

const API_BASE_URL = import.meta.env.VITE_API_URL;

function converterParaProblemDetails (
    value: unknown,
): ApiProblemDetails | undefined {
        if (typeof value !== 'object' || value === null) {
            return undefined;
        }

        return value as ApiProblemDetails;
    }

function obterMensagemDoProblema(
  problemDetails: ApiProblemDetails | undefined,
  status: number,
): string {
    const mensagensDeValidacao = problemDetails?.errors
        ? Object.values(problemDetails.errors).flat()
        : [];

    return (
        mensagensDeValidacao[0] ??
        problemDetails?.detail ??
        problemDetails?.title ??
        `A requisição falhou com status ${status}.`
    );
}

async function lerCorpo(
  response: Response,
): Promise<unknown> {
  const contentType = response.headers.get('content-type') ?? '';

  if (!contentType.includes('json')) {
    return null;
  }

  try {
    return await response.json();
  } catch {
    return null;
  }
}

export async function httpRequest<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const headers = new Headers(options.headers);

  headers.set('Accept', 'application/json');

  if (options.body && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }

  let response: Response;

  try {
    response = await fetch(
      `${API_BASE_URL}${path}`,
      {
        ...options,
        headers,
      },
    );
  } catch (error) {
    const detalhe = error instanceof Error ? error.message
        : 'Falha de rede desconhecida.';

    throw new ApiError(
      `Não foi possível conectar à API. ${detalhe}`,
      0,
    );
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const body = await lerCorpo(response);

  if (!response.ok) {
    const problemDetails = converterParaProblemDetails(body);

    throw new ApiError(
      obterMensagemDoProblema(
        problemDetails,
        response.status,
      ),
      response.status,
      problemDetails,
    );
  }

  return body as T;
}