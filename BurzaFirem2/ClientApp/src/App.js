import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { FrontLayout } from "./pages/Front"
import Home from "./pages/Front/Home"
import Detail from "./pages/Front/Detail"
import { AccountLayout } from "./pages/Account"
import Profile from "./pages/Account/Profile"
import SignIn from "./pages/Account/SignIn"
import { AdminLayout } from "./pages/Admin"
import Dashboard from "./pages/Admin/Dashboard"
import CompaniesList from "./pages/Admin/Companies/List"
import CompaniesDetail from "./pages/Admin/Companies/Detail"
import CompaniesCreate from "./pages/Admin/Companies/Create"
import NotFound from "./pages/Special/NotFound"
import Unauthorized from "./pages/Special/Unauthorized"
import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <Routes>
          <Route path="/unauthorized" element={<Unauthorized />} />
          <Route path='/admin' element={<AdminLayout />}>
            <Route index element={<Dashboard />} />
            <Route path="/admin/companies" element={<CompaniesList />} />
            <Route path="/admin/companies/create" element={<CompaniesCreate />} />
            <Route path="/admin/companies/:id" element={<CompaniesDetail />} />
          </Route>
          <Route path='/account' element={<AccountLayout />}>
            <Route index element={<Profile />} />
            <Route path="/account/sign-in" element={<SignIn />} />
          </Route>
          <Route path='/' element={<FrontLayout />}>
            <Route index element={<Home />} />
            <Route path="/:id" element={<Detail />} />
          </Route>
          <Route path="*" element={<NotFound />} />
        </Routes>
    );
  }
}
