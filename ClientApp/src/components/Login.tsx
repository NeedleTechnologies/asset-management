import React, { useRef, useState, useEffect, useContext } from "react";
import { connect } from "react-redux";
import logo from "../Assets/img/logo.png";
import { Link, useHistory } from "react-router-dom";
import AuthContext from "../context/authContext";
import { useAlert } from "react-alert";

import axios from "../api/Base";
const LOGIN_URL = "/auth";

const Login = () => {
  const setAuth = useContext(AuthContext);
  const history = useHistory();
  const alert = useAlert();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [errMsg, setErrMsg] = useState("");
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    setErrMsg("");
  }, [username, password]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("hi");
    try {
      const response = await axios.post(
        LOGIN_URL,
        JSON.stringify({ username, password }),
        {
          headers: { "Content-Type": "application/json" },
          // withCredentials: true,
        }
      );
      console.log(response.request);
      if (response.status === 200) {
        alert.success("Login Successful");
        localStorage.setItem("token", response.data.response.token);
        history.push("/view-all");
      } else {
        alert.error("Invalid Credentials");
      }
      setUsername("");
      setPassword("");
      setSuccess(true);
    } catch (err) {
      console.log(err);
      alert.error("Invalid Credentials");
    }
  };

  return (
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
                  <span aria-live="assertive"></span>
                  <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>Username</strong>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="username"
                        onChange={(e) => setUsername(e.target.value)}
                        value={username}
                        required
                      />
                    </div>

                    <div className="mb-5">
                      <label className="form-label">
                        {" "}
                        <strong>Password</strong>{" "}
                      </label>

                      <input
                        type="password"
                        className="form-control"
                        id="password"
                        onChange={(e) => setPassword(e.target.value)}
                        value={password}
                        required
                      />
                    </div>
                    <div className=" text-center my-5 ">
                      <button className="btn btn-success btn-lg" type="submit">
                        Login
                      </button>
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
};

export default Login;
