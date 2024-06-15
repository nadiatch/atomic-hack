import React from 'react';
import {BrowserRouter as Router, useRoutes} from 'react-router-dom';
import {Chat} from "./components/Chat";

const App = () => {
  return useRoutes([
      {path: "/", element: <Chat/>}
  ]);
}

const AppWrapper = () => {
  return (
          <Router>
            <App />
          </Router>
  );
};

export default AppWrapper;
