import * as React from "react";

interface IState {
  count: number;
}

export class Counter extends React.Component<unknown, IState>{
  state = {
    count: 0
  }
  render() {
    return (
      <input
        type="button"
        onClick={() => this.setState({ count: this.state.count + 1 })}
        value={`Click (${this.state.count})`}
      />
    );
  }
}
