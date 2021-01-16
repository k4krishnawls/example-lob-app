import { useState, useEffect } from 'react';

export interface Response<T> {
  data: T | null;
  error: any | null;
  isLoading: boolean;
}

export const useFetch = <T extends unknown>(url: string, translate: (rawData: any) => T): Response<T> => {
  const [data, setData] = useState<T | null>(null);
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  async function fetchData() {
    setIsLoading(true);
    try {
      const response = await fetch(url);
      const json = await response.json();
      const translated = translate(json);
      setData(translated);
      setIsLoading(false);
    }
    catch (error) {
      setIsLoading(false);
      setError(error);
    }
  }

  useEffect(() => {
    fetchData();
  }, [url]);

  return { data, error, isLoading };
};
