import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";

export class DataService {
  protected url = environment.api_url + this.controllerName;

  constructor(public controllerName: string, protected http: HttpClient) { }

  getAll() {
    return this.http.get(this.url);
  }

  get(id) {
    return this.http.get(this.url + '/' + id);
  }

  create(resource) {
    return this.http.post (this.url, JSON.stringify(resource));
  }

  update(resource) {
    return this.http.patch(this.url + '/' + resource.id, JSON.stringify(resource));
  }

  delete(id) {
    return this.http.delete(this.url + '/' + id);
  }
}

