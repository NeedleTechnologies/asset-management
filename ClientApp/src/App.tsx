import * as React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Provider as AlertProvider } from "react-alert";
import AlertTemplate from "./components/AlertTemplate";

import Layout from "./components/Layout";
// import Home from "./components/Home";
import "./custom.css";
import Login from "./components/Login";
import View_upload from "./components/View_page";
// import Upload_file from "./components/Upload_page";
import View_all from "./components/View_all";
import Create_folder from "./components/Create_folder";
import Create_user from "./components/Create_user";

export default () => (
  <Router>
    <AlertProvider
      template={AlertTemplate}
      timeout={5000}
      position={"bottom right"}
    >
      <Layout>
        <Route exact path="/" component={Login} />
        {/* <Route path="/home" component={Home} /> */}
        <Route path="/view-upload/:id" component={View_upload} />
        <Route path="/create-folder" component={Create_folder} />
        <Route path="/create-user" component={Create_user} />
        <Route path="/view-all" component={View_all} />
      </Layout>
    </AlertProvider>
  </Router>
);
