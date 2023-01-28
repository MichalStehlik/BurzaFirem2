import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { FrontLayout } from "./pages/Front"
import Home from "./pages/Front/Home"
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <Routes>
          <Route path='/' element={<FrontLayout />}>
            <Route index element={<Home />} />
          </Route>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
    );
  }
}
