export const tryRequest =
  <TResult, TData>(request: (data?: TData) => Promise<TResult>, onError: (error: any) => void) =>
  async (data?: TData) => {
    try {
      return await request(data);
    } catch (error) {
      onError(error);
    }
  };
