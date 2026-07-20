interface LoadingMessageProps {
  message?: string;
}

export function LoadingMessage({
  message = 'Carregando...',
}: LoadingMessageProps) {
  return (
    <p role="status">
      {message}
    </p>
  );
}