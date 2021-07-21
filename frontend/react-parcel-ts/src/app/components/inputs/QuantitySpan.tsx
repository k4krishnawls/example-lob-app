import React from 'react';

const formatter = new Intl.NumberFormat("en-US", {
  style: "decimal"
});

type Props = {
  number: number;
};

export const QuantitySpan: React.FunctionComponent<Props> = (props: Props) =>
  <span className="lob-span-quantity">{formatter.format(props.number)}</span>;
