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
import { Link } from "react-router-dom";
import "./NavMenu.css";
import logo from "../Assets/img/logo.png";

export default class NavMenu extends React.PureComponent<
  {},
  { isOpen: boolean }
> {
  public state = {
    isOpen: false,
  };

  public render() {
    return (
      <header>
        <Navbar
          className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3"
          light
        >
          <Container>
            <NavbarBrand tag={Link} to="/">
              <img height={60} src={logo} />
            </NavbarBrand>
            <NavbarToggler onClick={this.toggle} className="mr-2" />
            <Collapse
              className="d-sm-inline-flex flex-sm-row-reverse"
              isOpen={this.state.isOpen}
              navbar
            >
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
                  <NavLink tag={Link} className="text-danger" to="/">
                    <strong>Logout</strong>
                  </NavLink>
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }

  private toggle = () => {
    this.setState({
      isOpen: !this.state.isOpen,
    });
  };
}
