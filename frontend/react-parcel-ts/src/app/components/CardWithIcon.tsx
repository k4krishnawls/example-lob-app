import * as React from "react";
import { Link } from "react-router-dom";

interface IProps {
  icon: React.FunctionComponent,
  title: string;
  subtitle: string;
  to: string;
}

export const CardWithIcon: React.FunctionComponent<IProps> = (props: IProps) => {
  const { icon, title, subtitle, to } = props;
  return (
    <Link className="lob-card" to={to}>
      <div className="lob-card-icon">{React.createElement(icon)}</div>
      <div className="lob-card-title">{title}</div>
      <div className="lob-card-subtitle">{subtitle}</div>
    </Link>
  );
};
