import * as React from 'react';
import { BrowserRouter, Switch, Route, NavLink } from 'react-router-dom';
import { DashboardPage } from "./pages/dashboard/Dashboard";
import { NotFoundPage } from "./pages/errors/notFoundPage";

export const App: React.FC = () => (
  <BrowserRouter>
    <div className="app-body">
      <div className="app-header">
        <div className="app-header-toggle"></div>
        <div className="app-header-title">Administration</div>
        <div className="app-header-menu">
          <span>🚧 User Name</span>
        </div>
      </div>
      <div className="app-menu">
        <ul>
          <li><NavLink to="/" className="app-menu-item" exact activeClassName="selected">Home</NavLink></li>
          <li><NavLink to="/administration" className="app-menu-item" exact activeClassName="selected">Administration</NavLink></li>
          <li><a href="/account/logout" className="app-menu-item">Logout</a></li>
        </ul>
      </div>
      <div className="app-content">
        <Switch>
          <Route path="/" exact={true} component={DashboardPage} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    </div>
  </BrowserRouter>
);
