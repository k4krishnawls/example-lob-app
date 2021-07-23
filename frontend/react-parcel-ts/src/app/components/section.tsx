import * as React from "react";

export const Section: React.FunctionComponent = (props) => {
  return (
    <div className="lob-section">
      {props.children}
    </div>
  );
};
