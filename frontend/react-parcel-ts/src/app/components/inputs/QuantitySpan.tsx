import React from 'react';

const formatter = new Intl.NumberFormat("en-US", {
  style: "decimal"
});

interface IProps {
  number: number;
}

export const QuantitySpan: React.FunctionComponent<IProps> = (props: IProps) =>
  <span className="lob-span-quantity">{formatter.format(props.number)}</span>;
