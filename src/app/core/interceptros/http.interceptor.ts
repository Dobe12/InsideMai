import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse
} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, retry} from "rxjs/operators";
import {ToastrService} from "ngx-toastr";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
     // retry(1),

      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        // if client side error else server side error
        if (error.error instanceof ErrorEvent) {
          errorMessage = `Ошибка: ${error.error}`;
        } else {
          errorMessage = `Код ошибки: ${error.status}. ${error.error}`;
        }

        this.toastr.error(error.error);
        return throwError(errorMessage);
      })
    );
  }
}
