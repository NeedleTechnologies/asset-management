import * as React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import NavMenu from "./NavMenu";

import { FilePond, registerPlugin } from "react-filepond";
import "filepond/dist/filepond.min.css";

const Upload_file = () => (
  <div>
    <NavMenu />

    <div className="container">
      <div className="justify-content-center align-items-center text-center">
        <div className="row">
          <div className="col-12 m-5">
            <FilePond allowMultiple={true} maxFiles={3} server="/api" />
          </div>
        </div>
      </div>
    </div>
  </div>
);

export default connect()(Upload_file);
