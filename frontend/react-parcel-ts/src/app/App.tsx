import * as React from 'react';
import { BrowserRouter, Switch, Route, NavLink } from 'react-router-dom';
import { AdministrationPage } from "./pages/administration/administrationPage";
import { UserPage } from "./pages/administration/userPage";
import { HomePage } from "./pages/home/homePage";
import { NotFoundPage } from "./pages/errors/notFoundPage";

export const App: React.SFC = () => (
  <BrowserRouter>
    <div className="app-body">
      <div className="app-header"></div>
      <div className="app-menu">
        <ul>
          <li><NavLink to="/" className="app-menu-item" exact activeClassName="selected">Home</NavLink></li>
          <li><NavLink to="/administration" className="app-menu-item" exact activeClassName="selected">Administration</NavLink></li>
          <li><a href="/account/logout" className="app-menu-item">Logout</a></li>
        </ul>
      </div>
      <div className="app-content">
        <Switch>
          <Route path="/" exact={true} component={HomePage} />
          <Route path="/administration" exact={true} component={AdministrationPage} />
          <Route path="/administration/users/:id" exact={true} component={UserPage} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    </div>
  </BrowserRouter>
);
