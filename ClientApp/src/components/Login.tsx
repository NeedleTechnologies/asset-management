import React, { useRef, useState, useEffect, useContext } from "react";
import { connect } from "react-redux";
import logo from "../Assets/img/logo.png";
import { Link, useHistory } from "react-router-dom";
import AuthContext from "../context/authContext";

import axios from "../api/Base";
const LOGIN_URL = "/auth";

const Login = () => {
  const setAuth = useContext(AuthContext);
  const history = useHistory();

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
      console.log(response);
      if (response.status === 200) {
        localStorage.setItem("token", response.data.response.token);
        history.push("/home");
      } else if (response.status === 401) {
        console.log("invalid login credentials");
      }

      setUsername("");
      setPassword("");
      setSuccess(true);
    } catch (err) {
      console.log(err);
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
