import React, { useRef, useState, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";
import { useAlert } from "react-alert";
import NavMenu from "./NavMenu";

import axios from "../api/Base";

const Create_user = () => {
  const history = useHistory();
  const alert = useAlert();
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errMsg, setErrMsg] = useState("");
  const [success, setSuccess] = useState(false);

  const token = localStorage.getItem("token");

  useEffect(() => {
    setErrMsg("");
  }, [firstName, lastName, userName, email, password]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "/create-user",
        JSON.stringify({ firstName, lastName, userName, email, password }),
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 200) {
        alert.success(response.data.message);
        history.push("/view-all");
      }
      setFirstName("");
      setLastName("");
      setUserName("");
      setEmail("");
      setPassword("");
      setSuccess(true);
    } catch (err) {
      alert.error(
        "Passwords must have at least one non alphanumeric character."
      );
    }
  };

  return (
    <div>
      <NavMenu />
      <div className="container-fluid">
        <div className="row align-items-center">
          <div className="col"></div>
          <div className="col align-self-center mt-5">
            <div>
              <div className="card">
                <div className="card-body">
                  <span aria-live="assertive"></span>
                  <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>First Name</strong>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="firstName"
                        onChange={(e) => setFirstName(e.target.value)}
                        value={firstName}
                        required
                      />
                    </div>
                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>Last Name</strong>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="lastName"
                        onChange={(e) => setLastName(e.target.value)}
                        value={lastName}
                        required
                      />
                    </div>
                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>Username</strong>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="folderName"
                        onChange={(e) => setUserName(e.target.value)}
                        value={userName}
                        required
                      />
                    </div>
                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>Email</strong>
                      </label>
                      <input
                        type="email"
                        className="form-control"
                        id="email"
                        onChange={(e) => setEmail(e.target.value)}
                        value={email}
                        required
                      />
                    </div>

                    <div className="mb-4">
                      <label htmlFor="email" className="form-label">
                        {" "}
                        <strong>Password</strong>
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
                        Create
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

export default Create_user;
