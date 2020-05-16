import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

export class DataService {
  protected url = environment.api_url + this.controllerName;

  constructor(public controllerName: string, protected http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get(this.url);
  }

  get(id): Observable<any> {
    return this.http.get(this.url + '/' + id);
  }

  create(resource): Observable<any> {
    return this.http.post (this.url, JSON.stringify(resource));
  }

  update(resource): Observable<any> {
    return this.http.patch(this.url + '/' + resource.id, JSON.stringify(resource));
  }

  delete(id): Observable<any> {
    return this.http.delete<any>(this.url + '/' + id);
  }
}

