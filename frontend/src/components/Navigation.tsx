import type { Pagina } from '../types';

interface NavigationProps {
  paginaAtual: Pagina;
  onNavigate: (pagina: Pagina) => void;
}

export function Navigation({
  paginaAtual,
  onNavigate,
}: NavigationProps) {
  return (
    <nav aria-label="Navegação principal">
      <button
        type="button"
        aria-pressed={paginaAtual === 'pessoas'}
        onClick={() => onNavigate('pessoas')}
      >
        ◉ &nbsp; Pessoas
      </button>

      <button
        type="button"
        aria-pressed={
          paginaAtual === 'transacoes'
        }
        onClick={() => onNavigate('transacoes')}
      >
        ↗ &nbsp; Transações
      </button>

      <button
        type="button"
        aria-pressed={paginaAtual === 'totais'}
        onClick={() => onNavigate('totais')}
      >
        ⌁ &nbsp; Totais
      </button>
    </nav>
  );
}
