import * as React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import NavMenu from "./NavMenu";
import folderIcon from "../Assets/img/folder.png";
import folderUpload from "../Assets/img/folder-upload.png";

const Home = () => (
  <div>
    <NavMenu />

    <div className="container">
      <div className="vh-100 d-flex justify-content-center align-items-center text-center">
        <div className="row">
          <div className="col m-5">
            <Link to="/view-all">
              <img width={200} height={200} src={folderIcon} />
              <h4 className="text-dark">View</h4>
            </Link>
          </div>
          <div className="col m-5 ">
            <Link to="/upload-file">
              <img width={200} height={200} src={folderUpload} />
              <h4 className="text-dark">Upload</h4>
            </Link>
          </div>
        </div>
      </div>
    </div>
  </div>
);

export default connect()(Home);
