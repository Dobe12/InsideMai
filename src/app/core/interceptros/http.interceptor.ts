import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, retry} from "rxjs/operators";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
     // retry(1),

      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        // if client side error else server side error
        if (error.error instanceof ErrorEvent) {
          errorMessage = `Ошибка: ${error.error.message}`;
        } else {
          errorMessage = `Код ошибки: ${error.status}. ${error.message}`;
        }

        return throwError(errorMessage);
      })
    );
  }
}
