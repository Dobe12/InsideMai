import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {distinctUntilChanged, map} from "rxjs/operators";
import {JwtHelperService} from "@auth0/angular-jwt";
import {UsersService} from "../services/users.service";
import {Roles, User} from "../models/user";
import {BehaviorSubject, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class AuthService  {
  private url = environment.api_url + "account";
  public jwtHelper: JwtHelperService = new JwtHelperService();
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient, private usersService: UsersService) {
    if (this.isLoggedIn()) {
      this.setUser(localStorage.getItem('token'));
    }

    this.currentUserSubject = new BehaviorSubject<User>({} as User);
    this.currentUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(credentials) {
    return this.http.post<JwtResponse>(this.url + '/login', credentials)
      .pipe(map(response => {
        const result = response;
        if (result && result.token) {
          localStorage.setItem('token', result.token);

          this.setUser(result.token);
          return true;
        }
    }));
  }

  changePassword(currentPassword, newPassword) {
    return this.http.post(this.url + '/changePassword', {currentPassword, newPassword});
  }

  private setUser(token) {
    const userId = this.jwtHelper.decodeToken(token).UserID;
    this.usersService.get(userId).subscribe(
      response =>  this.currentUserSubject.next(response as User));
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  isLoggedIn() {
    const token = localStorage.getItem('token');

    if (!token) {
      return false;
    }
    return !this.jwtHelper.isTokenExpired(token);
  }

  isAdmin() {
    return this.currentUserValue.role === Roles.Admin;
  }

}



export class JwtResponse {
  token: string;
}
