import * as React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import Counter from "./components/Counter";
import FetchData from "./components/FetchData";

import "./custom.css";
import { Login } from "./components/Login";
import View_upload from "./components/View_page";
import Upload_file from "./components/Upload_page";
import View_all from "./components/View_all";

export default () => (
  <Layout>
    <Route exact path="/" component={Login} />
    <Route path="/home" component={Home} />
    <Route path="/view-upload" component={View_upload} />
    <Route path="/upload-file" component={Upload_file} />
    <Route path="/view-all" component={View_all} />
  </Layout>
);
