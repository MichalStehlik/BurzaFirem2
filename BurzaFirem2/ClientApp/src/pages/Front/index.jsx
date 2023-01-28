import {useState} from "react"
import { Outlet, Link } from "react-router-dom"
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Container } from 'reactstrap';

export const FrontLayout = () => {
    const [navbarOpen, setNavbarOpen] = useState(false);
    return (
      <div>
        <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
            <NavbarBrand tag={Link} to="/">BurzaFirem2</NavbarBrand>
            <NavbarToggler onClick={e => setNavbarOpen(prev => !prev)} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={navbarOpen} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
              </NavItem>
            </ul>
            </Collapse>
        </Navbar>
        </header>
        <Container>
            <Outlet />
        </Container>
      </div>
    );
}

export default FrontLayout;