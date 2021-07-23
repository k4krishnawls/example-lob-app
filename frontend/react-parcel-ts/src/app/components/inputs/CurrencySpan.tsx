import React from 'react';

const formatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

interface IProps {
  number: number;
}

export const CurrencySpan: React.FunctionComponent<IProps> = (props: IProps) =>
  <span className="lob-span-currency">{formatter.format(props.number)}</span>;
