import { Injectable } from '@angular/core';
import {DataService} from "../services/data.service";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {map} from "rxjs/operators";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})

export class AuthService  {
  private url = environment.api_url + "account/login";
  public jwtHelper: JwtHelperService = new JwtHelperService();

  constructor(private http: HttpClient) {
  }

  login(credentials) {
    return this.http.post<JwtResponse>(this.url, credentials)
      .pipe(map(response => {
        const result = response;
        if (result && result.token) {
          localStorage.setItem('token', result.token);
          return true;
        }
    }));
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn() {
    const token = localStorage.getItem('token');

    if (!token) {
      return false;
    }

    return !this.jwtHelper.isTokenExpired(token);
  }

  get currentUser() {
    const token = localStorage.getItem('token');

    if (!token) { return null; }

    return this.jwtHelper.decodeToken(token);
  }

}



export class JwtResponse {
  token: string;
}
