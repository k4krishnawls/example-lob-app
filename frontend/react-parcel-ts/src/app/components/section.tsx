import * as React from "react";

export const Section: React.SFC = (props) => {
  return (
    <div className="app-section">
      {props.children}
    </div>
  );
};
