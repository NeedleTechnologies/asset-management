import * as React from "react";
import { connect } from "react-redux";
import logo from "../Assets/img/logo.png";
import { Link } from "react-router-dom";

export const Login = () => (
  <div>
    <div className="container-fluid form-con">
      <div className="row align-items-center">
        <div className="col"></div>
        <div className="col align-self-center mt-5">
          <div>
            <div className="card">
              <div className="card-body">
                <div className=" text-center my-5">
                  <div>
                    <img src={logo} width={240} height={70} />
                  </div>
                </div>
                <form>
                  <div className="mb-4">
                    <label className="form-label">
                      {" "}
                      <strong>Email address</strong>
                    </label>
                    <input type="email" className="form-control" />
                  </div>

                  <div className="mb-5">
                    <label className="form-label">
                      {" "}
                      <strong>Password</strong>{" "}
                    </label>

                    <input type="password" className="form-control" />
                  </div>
                  <div className=" text-center my-5 ">
                    <Link to="/home">
                      <button className="btn btn-success btn-lg" type="submit">
                        Login
                      </button>
                    </Link>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
        <div className="col"></div>
      </div>
    </div>
  </div>
);
