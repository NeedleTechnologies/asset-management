import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import NavMenu from "./NavMenu";
import { useDropzone } from "react-dropzone";
import moment from "moment";

import axios from "../api/Base";
interface fileStructure {
  documentName: string;
  folderId: string;
  dateUploaded: string;
  uploadedBy: string;
}

const View_upload = ({ match }: RouteComponentProps<{ id: string }>) => {
  console.log(match.params.id);
  const [files, setFiles] = useState([]);

  const token = localStorage.getItem("token");
  useEffect(() => {
    axios
      .get(`/get-folder-documents/${match.params.id}`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        console.log(response);
        setFiles(response.data.data);
      })
      .catch(() => {});
  }, []);

  return (
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
              {files.map((file: fileStructure, i) => (
                <tr key={i}>
                  <th scope="row">{i + 1}</th>
                  <td>{file.documentName} </td>
                  <td> {moment(file.dateUploaded).format("ll")} </td>
                  <td> {file.uploadedBy} </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default View_upload;
