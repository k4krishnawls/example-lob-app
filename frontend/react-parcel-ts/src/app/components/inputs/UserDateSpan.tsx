import React from 'react';


function formatDate(date: Date) {
  return date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

interface IProps {
  date: Date;
}

export const UserDateSpan: React.FunctionComponent<IProps> = (props: IProps) =>
  <span className="lob-span-quantity">{formatDate(props.date)}</span>;
