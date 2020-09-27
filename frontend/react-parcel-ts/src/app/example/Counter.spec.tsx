import * as React from 'react';
import { render, fireEvent, screen } from '@testing-library/react';
import { Counter } from "./Counter";

describe("Example", () => {
  it("increments when you press the button", () => {
    render(<Counter />);

    fireEvent.click(screen.getByRole('button'));
  });
});
