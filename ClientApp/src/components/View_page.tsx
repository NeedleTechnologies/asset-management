import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import NavMenu from "./NavMenu";
import { useDropzone } from "react-dropzone";
import moment from "moment";
import { useAlert } from "react-alert";
import axios from "../api/Base";
import { Container } from "reactstrap";
interface fileStructure {
  documentName: string;
  folderId: string;
  dateUploaded: string;
  uploadedBy: string;
}

const View_upload = ({ match }: RouteComponentProps<{ id: string }>) => {
  console.log(match.params.id);
  const [documents, setDocument] = useState([]);
  const [files, setFiles] = React.useState([]);
  function refresh() {
    window.location.reload();
  }
  const alert = useAlert();
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
        setDocument(response.data.data);
      })
      .catch(() => {});
  }, []);

  function fileSizeValidator(file: any) {
    if (file.size > 1024 ** 2 * 2) {
      return {
        code: "size-too-large",
        message: `File is larger than 2mb`,
      };
    }

    return null;
  }

  const onDrop = React.useCallback((acceptedFiles) => {
    console.log(acceptedFiles);
    setFiles(
      acceptedFiles.map((file: any) =>
        Object.assign(file, {
          preview: URL.createObjectURL(file),
        })
      )
    );
  }, []);

  const { fileRejections, getRootProps, getInputProps, isDragActive } =
    useDropzone({
      onDrop,
      accept: "image/jpeg, application/pdf",
      maxFiles: 5,
      validator: fileSizeValidator,
    });

  return (
    <div>
      <NavMenu />
      <div className="container">
        <div>
          <div className="upload-sec p-4">
            <div className="bg-success p-5 text-white">
              <div {...getRootProps()}>
                <input {...getInputProps()} />
                {isDragActive ? (
                  <p className="text-center text-xl">
                    Drop your media files here
                  </p>
                ) : (
                  <p className="text-center text-xl">
                    Drag and drop some files here, or click to select files
                  </p>
                )}
              </div>
            </div>
          </div>
          {files.length > 0 && (
            <button
              onClick={() => {
                const formData = new FormData();
                let file = files[0];
                formData.append("file", file);
                formData.append("upload_preset", "s9n5tgkf");
                axios
                  .post(`upload-documents/${match.params.id}`, formData, {
                    headers: {
                      "Content-Type": "application/json",
                      Authorization: `Bearer ${token}`,
                    },
                  })
                  .then((response) => {
                    alert.success(`${response.data.message}`);
                    refresh();
                  })
                  .then((data) => {
                    console.log(data);
                  });
                refresh();
              }}
            >
              Upload
            </button>
          )}
          {files.map((f: any) => {
            return <img src={f.name} />;
          })}

          {fileRejections.map(({ errors }) => {
            return (
              <li>
                <ul>
                  {errors.map((e) => (
                    <li key={e.code}>{e.message}</li>
                  ))}
                </ul>
              </li>
            );
          })}
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
              {documents.map((docu: fileStructure, i) => (
                <tr key={i}>
                  <th scope="row">{i + 1}</th>
                  <td>{docu.documentName} </td>
                  <td> {moment(docu.dateUploaded).format("ll")} </td>
                  <td> {docu.uploadedBy} </td>
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
