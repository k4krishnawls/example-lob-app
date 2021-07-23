import * as React from "react";

interface IProps {
  children: React.ReactNode;
}

export const ButtonGroup: React.FC<IProps> = (props: IProps) => {
  return (
    <div className="lob-button-group">
      {props.children}
    </div>
  );
};
