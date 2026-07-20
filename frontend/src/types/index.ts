export type Pagina = 'pessoas' | 'transacoes' | 'totais';
export type TipoTransacao = 'Despesa' | 'Receita';

export interface Pessoa {
    id: number;
    nome: string;
    idade: number;
}

export interface CriarPessoaRequest {
    nome: string;
    idade: number;
}

export interface Transacao {
    id: number;
    descricao: string;
    valor: number;
    tipo: TipoTransacao;
    pessoaId: number;
    pessoaNome: string;
}

export interface CriarTransacaoRequest {
    descricao: string;
    valor: number;
    tipo: TipoTransacao;
    pessoaId: number;
}

export interface TotalPessoa {
    pessoaId: number;
    nome: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface TotalGeral {
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface ConsultaTotais {
    pessoas: TotalPessoa[];
    totalGeral: TotalGeral;
}

export interface ApiProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    traceId?: string;
    errors?: Record<string, string[]>;
}