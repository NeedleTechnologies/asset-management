import React, { useRef, useState, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";
import { useAlert } from "react-alert";
import NavMenu from "./NavMenu";

import axios from "../api/Base";

const Create_folder = () => {
  const history = useHistory();
  const alert = useAlert();
  const [folderName, setFolderName] = useState("");
  const [errMsg, setErrMsg] = useState("");
  const [success, setSuccess] = useState(false);

  const token = localStorage.getItem("token");

  useEffect(() => {
    setErrMsg("");
  }, [folderName]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "/create-folder",
        JSON.stringify({ folderName }),
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
      setFolderName("");
      setSuccess(true);
    } catch (err) {
      alert.error("Folder name in existence");
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
                        <strong>Folder Name</strong>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="folderName"
                        onChange={(e) => setFolderName(e.target.value)}
                        value={folderName}
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

export default Create_folder;
