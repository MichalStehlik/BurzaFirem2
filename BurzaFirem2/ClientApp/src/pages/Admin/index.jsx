import {useState} from "react"
import { Outlet, Link } from "react-router-dom"
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Container } from 'reactstrap'
import { useAppContext } from "../../providers/AppProvider"
import { useAuthContext } from "../../providers/AuthProvider"
import {requireAuth} from "../../hoc/requireAuth"

export const AdminLayout = () => {
    const [{config}] = useAppContext();
    const [{accessToken, profile}] = useAuthContext();
    const [navbarOpen, setNavbarOpen] = useState(false);
    
    return (
      <div>
        <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
            <NavbarBrand tag={Link} to="/">{config.applicationName}</NavbarBrand>
            <NavbarToggler onClick={e => setNavbarOpen(prev => !prev)} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={navbarOpen} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/admin/companies">Firmy</NavLink>
              </NavItem>
              {profile && profile.admin === "1"
              ?
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/admin/users">Uživatelé</NavLink>
              </NavItem>
              :
              null
              }         
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

export default requireAuth(AdminLayout);