import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import NavMenu from "./NavMenu";
import folderIcon from "../Assets/img/folder.png";
import folderUpload from "../Assets/img/folder-upload.png";

import axios from "../api/Base";
import { AxiosResponse } from "axios";
import { Button } from "reactstrap";

interface folderStructure {
  folderName: string;
  folderId: string;
}

const View_all = () => {
  const [folder, setFolder] = useState([]);

  const user = null;
  const token = localStorage.getItem("token");

  const role = localStorage.getItem("role");
  useEffect(() => {
    axios
      .get("/get-folders", {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        console.log(response);
        setFolder(response.data.data);
      })
      .catch(() => {});
  }, []);
  return (
    <div>
      <NavMenu />

      <div className="container">
        <div className="row">
          <div className="col-2">
            {role === "Admin" ? (
              <Link to="/create-folder">
                <Button className="btn btn-success">Create Folder</Button>
              </Link>
            ) : null}
          </div>
          <div className="col-2">
            {role === "Admin" ? (
              <Link to="/create-user">
                <Button className="btn btn-info text-white">Create user</Button>
              </Link>
            ) : null}
          </div>
        </div>
        <div className="vh-100  justify-content-center align-items-center text-center">
          <div className="row">
            {folder.map((fold: folderStructure, i) => (
              <div className="col-md m-5" key={i}>
                <Link to={`/view-upload/${fold.folderId}`}>
                  <img width={200} height={200} src={folderIcon} />
                  <h4 className="text-dark"> {fold.folderName} </h4>
                </Link>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default View_all;
