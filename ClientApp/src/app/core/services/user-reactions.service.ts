import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserReactionsService {
  constructor(private http: HttpClient) { }

  addLikeToPost(id: number) {
    return this.http.post(`${environment.api_url}posts/${id}/like`, {});
  }

  removeLikeFromPost(id: number) {
    return this.http.delete(`${environment.api_url}posts/${id}/like`);
  }

  addLikeToComment(id: number) {
    return this.http.post(`${environment.api_url}comments/${id}/like`, {});
  }

  removeLikeFromComment(id: number) {
    return this.http.delete(`${environment.api_url}comments/${id}/like`);
  }

  addToFav(id: number) {
    return this.http.post(`${environment.api_url}posts/${id}/fav`, {});
  }

  removeFromFav(id: number) {
    return this.http.delete(`${environment.api_url}posts/${id}/fav`);
  }
}
