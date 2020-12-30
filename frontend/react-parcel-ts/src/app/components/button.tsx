import * as React from "react";

interface IProps {
  flavor?: "primary" | "secondary"
  onClick: () => void;
  enable?: boolean;
}

export const Button: React.FunctionComponent<IProps> = (props) => {
  const className = `gdb-button ${props.flavor == "secondary" ? "gdb-bs-secondary" : "gdb-bs-primary"}`;
  const disabled = props.enable === false;
  return (
    <button
      className={className}
      disabled={disabled}
      onClick={() => {
        if (!disabled) props.onClick();
      }}>
      {props.children}
    </button>
  );
};
