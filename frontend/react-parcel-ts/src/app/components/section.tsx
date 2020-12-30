import * as React from "react";

export const Section: React.FunctionComponent = (props) => {
  return (
    <div className="app-section">
      {props.children}
    </div>
  );
};
