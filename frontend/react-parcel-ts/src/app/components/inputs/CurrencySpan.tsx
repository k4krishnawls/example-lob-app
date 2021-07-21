import React from 'react';

const formatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

type Props = {
  number: number;
};

export const CurrencySpan: React.FunctionComponent<Props> = (props: Props) =>
  <span className="lob-span-currency">{formatter.format(props.number)}</span>;
