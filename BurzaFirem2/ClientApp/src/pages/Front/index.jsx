import {useState} from "react"
import { Outlet, Link } from "react-router-dom"
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Container } from 'reactstrap'
import { useAppContext } from "../../providers/AppProvider"
import { useAuthContext } from "../../providers/AuthProvider"

export const FrontLayout = () => {
    const [{config}] = useAppContext();
    const [{accessToken, profile}] = useAuthContext();
    const [navbarOpen, setNavbarOpen] = useState(false);
    return (
      <div>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container color="light" light fixed="top" full={true} tag="header">
            <NavbarBrand tag={Link} to="/">{config.applicationName}</NavbarBrand>
            <NavbarToggler onClick={e => setNavbarOpen(prev => !prev)} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={navbarOpen} navbar>
            {(profile && (profile.admin === "1" || profile.editor === "1")) 
            ? 
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/admin">Administrace</NavLink>
              </NavItem>
            </ul>
            : 
            null}
            <ul className="navbar-nav flex-grow">
              <NavItem>
                {accessToken 
                ? 
                <NavLink tag={Link} className="text-dark" to="/account">Uživatel</NavLink>
                : 
                <NavLink tag={Link} className="text-dark" to="/account/sign-in">Přihlášení</NavLink>
                }
              </NavItem>
            </ul>
            </Collapse>
        </Navbar>
        <Container style={{paddingTop: 76}}>
            <Outlet />
        </Container>
      </div>
    );
}

export default FrontLayout;