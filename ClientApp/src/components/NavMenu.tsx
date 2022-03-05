import * as React from "react";
import {
  Collapse,
  Container,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link, useHistory } from "react-router-dom";
import "./NavMenu.css";
import logo from "../Assets/img/logo.png";

const NavMenu = () => {
  const history = useHistory();

  //   { isOpen: boolean }
  // > {
  //   public state = {
  //     isOpen: false,
  //   };

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3"
        light
      >
        <Container>
          <NavbarBrand tag={Link} to="/home">
            <img height={60} src={logo} />
          </NavbarBrand>
          <NavbarToggler className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem className="p-3">
                <NavLink tag={Link} className="text-dark" to="/home">
                  Home
                </NavLink>
              </NavItem>
              <NavItem className="p-3">
                <NavLink tag={Link} className="text-dark" to="/view-all">
                  View
                </NavLink>
              </NavItem>
              <NavItem className="p-3">
                <NavLink tag={Link} className="text-dark" to="/upload-file">
                  Upload
                </NavLink>
              </NavItem>
              <NavItem className="p-3">
                <NavLink
                  tag={Link}
                  className="text-danger"
                  onClick={() => {
                    localStorage.removeItem("token");
                    history.push("/");
                  }}
                >
                  <strong>Logout</strong>
                </NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Container>
      </Navbar>
    </header>
  );
};

export default NavMenu;

//   private toggle = () => {
//     this.setState({
//       isOpen: !this.state.isOpen,
//     });
//   };
// }
