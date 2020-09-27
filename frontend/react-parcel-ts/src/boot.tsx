import * as React from 'react';
import { render } from 'react-dom';
import { hot } from 'react-hot-loader';
import { App } from './app/App';

const HotApp = hot(module)(App);

render(
  <HotApp />,
  document.getElementById('root')
);
