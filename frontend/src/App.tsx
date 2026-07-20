import { useState } from 'react';

import { Navigation } from './components/Navigation';
import { PessoasPage } from './pages/PessoasPage';
import { TotaisPage } from './pages/TotaisPage';
import { TransacoesPage } from './pages/TransacoesPage';
import type { Pagina } from './types';

export default function App() {
  const [paginaAtual, setPaginaAtual] =
    useState<Pagina>('pessoas');

  return (
    <div className="app-shell">
      <div className="ambient ambient-one" />
      <div className="ambient ambient-two" />
      <header className="topbar">
        <div className="brand" aria-label="Home Expenses">
          <span className="brand-mark">H</span>
          <span>Home<span>Expenses</span></span>
        </div>

        <Navigation
          paginaAtual={paginaAtual}
          onNavigate={setPaginaAtual}
        />
        <div className="profile" title="Seu perfil">
          <span>Olá,</span><strong>Visitante</strong><span className="avatar">V</span>
        </div>
      </header>

      <main className="main-content" key={paginaAtual}>
        <div className="page-heading">
          <div><span className="eyebrow">VISÃO GERAL · SUA CASA</span><h1>Controle de gastos<br /><em>residenciais.</em></h1></div>
          <p>Organize as finanças de quem você ama. Simples, visual e sempre sob controle.</p>
        </div>
        {paginaAtual === 'pessoas' && (
          <PessoasPage />
        )}

        {paginaAtual === 'transacoes' && (
          <TransacoesPage />
        )}

        {paginaAtual === 'totais' && (
          <TotaisPage />
        )}
      </main>
      <footer><span>HOME EXPENSES · 2026</span><span>Finanças leves. Casa tranquila.</span></footer>
    </div>
  );
}
