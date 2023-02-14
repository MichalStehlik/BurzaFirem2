import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import FrontLayout from "./pages/Front"
import Home from "./pages/Front/Home"
import Detail from "./pages/Front/Detail"
import AccountLayout from "./pages/Account"
import Profile from "./pages/Account/Profile"
import SignIn from "./pages/Account/SignIn"
import AdminLayout from "./pages/Admin"
import Dashboard from "./pages/Admin/Dashboard"
import CompaniesLayout from "./pages/Admin/Companies"
import CompaniesList from "./pages/Admin/Companies/List"
import CompaniesDetail from "./pages/Admin/Companies/Detail"
import CompaniesCreate from "./pages/Admin/Companies/Create"
import UsersLayout from "./pages/Admin/Users"
import UsersList from "./pages/Admin/Users/List"
import UsersDetail from "./pages/Admin/Users/Detail"
import UsersCreate from "./pages/Admin/Users/Create"
import ImagesLayout from "./pages/Admin/Images"
import ImagesList from "./pages/Admin/Images/List"
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
            <Route path='/admin/companies' element={<CompaniesLayout />}>
              <Route index element={<CompaniesList />} />
              <Route path="/admin/companies/create" element={<CompaniesCreate />} />
              <Route path="/admin/companies/:id" element={<CompaniesDetail />} />
            </Route> 
            <Route path='/admin/users' element={<UsersLayout />}>
              <Route index element={<UsersList />} />
              <Route path="/admin/users/create" element={<UsersCreate />} />
              <Route path="/admin/users/:id" element={<UsersDetail />} />
            </Route> 
            <Route path='/admin/images' element={<ImagesLayout />}>
              <Route index element={<ImagesList />} />
            </Route> 
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
