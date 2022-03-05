import * as React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import Layout from "./components/Layout";
import Home from "./components/Home";
import "./custom.css";
import Login from "./components/Login";
import View_upload from "./components/View_page";
import Upload_file from "./components/Upload_page";
import View_all from "./components/View_all";

export default () => (
  <Router>
    <Layout>
      <Route exact path="/" component={Login} />
      <Route path="/home" component={Home} />
      <Route path="/view-upload/:id" component={View_upload} />
      <Route path="/upload-file" component={Upload_file} />
      <Route path="/view-all" component={View_all} />
    </Layout>
  </Router>
);
