import React from 'react';


function formatDate(date: Date) {
  return date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

type Props = {
  date: Date;
};

export const UserDateSpan: React.FunctionComponent<Props> = (props: Props) =>
  <span className="lob-span-quantity">{formatDate(props.date)}</span>;
