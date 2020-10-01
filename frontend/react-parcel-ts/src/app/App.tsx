import * as React from 'react';
import { BrowserRouter, Switch, Route, Link } from 'react-router-dom';
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
          <li><Link to="/" className="app-menu-item">Home</Link></li>
          <li><Link to="/administration" className="app-menu-item">Administration</Link></li>
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
