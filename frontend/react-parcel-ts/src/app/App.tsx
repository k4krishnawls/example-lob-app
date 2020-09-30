import * as React from 'react';
import { BrowserRouter, Switch, Route, Link } from 'react-router-dom';
import { AdministrationPage } from "./pages/administration/administrationPage";
import { HomePage } from "./pages/home/homePage";
import { NotFoundPage } from "./pages/errors/notFoundPage";

export const App: React.SFC = () => (
  <BrowserRouter>
    <div className="app-body">
      <div className="app-menu">
        <ul>
          <li><Link to="/">Home</Link></li>
          <li><Link to="/administration">Administration</Link></li>
          <li><a href="/account/logout">Logout</a></li>
        </ul>
      </div>
      <div className="app-content">
        <Switch>
          <Route path="/" exact={true} component={HomePage} />
          <Route path="/administration" exact={true} component={AdministrationPage} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    </div>
  </BrowserRouter>
);
