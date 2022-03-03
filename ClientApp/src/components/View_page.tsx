import * as React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import NavMenu from "./NavMenu";

const View_upload = () => (
  <div>
    <NavMenu />

    <div className="container">
      <div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">S/N</th>
              <th scope="col">File</th>
              <th scope="col">Date</th>
              <th scope="col">Uploaded By</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <th scope="row">1</th>
              <td>2020 general meeting </td>
              <td>22nd Feb. 2022</td>
              <td>Front Desk</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
);

export default connect()(View_upload);
