import * as React from "react";

export const ButtonGroup: React.FunctionComponent = (props) => {
  return (
    <div className="lob-button-group">
      {props.children}
    </div>
  );
};
