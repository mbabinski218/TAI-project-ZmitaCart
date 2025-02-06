export const stringMask = (type: string, value: string): string => {
  if (value)
    switch (type) {
      case 'Numer telefonu': {
        return `${value.slice(0, 3)}-${value.slice(3, 6)}-${value.slice(6)}`;
      }
      case 'Kod pocztowy': {
        return `${value.slice(0, 2)}-${value.slice(2)}`;
      }
    }
};