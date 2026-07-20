const formatadorBRL = new Intl.NumberFormat(
    'pt-BR',
    {
      style: 'currency',
      currency: 'BRL',
    },
);

export function formatarMoeda(
  valor: number,
): string {
  return formatadorBRL.format(valor);
}