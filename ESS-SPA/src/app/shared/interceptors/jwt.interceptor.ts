import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS, HttpResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap, switchMap, finalize, filter, take } from 'rxjs/operators';
import { AuthService } from '../services/auth-service/auth.service';
import { ErrorResponse } from '../../entities/models/errorResponse';

@Injectable()

// @Injectable({
//   providedIn: 'root'
// })
export class JwtInterceptor implements HttpInterceptor {
    private isTokenRefreshing: boolean = false;
    private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

    constructor(private authService: AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(this.attachTokenToRequest(req)).pipe(
        tap((event: HttpEvent<any>) => {          
          if (event instanceof HttpResponse) {
            console.log('Success!!!');
          }
        }), catchError((error): Observable<any> => {
          // TODO: To fix error. Not recieving adequate status code when token expires
          if (error instanceof HttpErrorResponse) {
            console.log(error);

            if (error.status === 0) {
              this.authService.logout();
            }

            if (error.status === 401) {
              // return this.handleHttpResponseError(req, next);
              return throwError(() => new ErrorResponse(error.statusText, error.error));
            }
            if (error.status === 400) {
                return throwError(() => new ErrorResponse(error.statusText, error.error));
            }
            if (error.status === 500) {
                return throwError(() => new ErrorResponse(error.statusText, error.error));
            }
            const applicationError = error.headers.get('Application-Error');
            if (applicationError) {
                return throwError(() => applicationError);
            }

            const serverError = error.error;
            let modalStateErrors = '';
            if (serverError && typeof serverError === 'object') {
                for (const key in serverError) {
                    if (serverError[key]) {
                        modalStateErrors += serverError[key] + '\n';
                    }
                }
            }
            return throwError(() => modalStateErrors || serverError || 'Server Error');
          } else {
            return throwError(() => this.handleGlobalError(error));
          }
        })
      );
    }

    private handleHttpResponseError(request: HttpRequest<any>, next: HttpHandler) {
      // debugger;
      if (!this.isTokenRefreshing) {
        this.isTokenRefreshing = true;
        this.tokenSubject.next('');
        return this.authService.getNewrefreshToken().pipe(
          switchMap((tokenRes: any) => {
            if (tokenRes && tokenRes !== null) {
              this.tokenSubject.next(tokenRes.token);
              console.log('Token Refreshed Successfully'); // if token is expired, refresh the token and send back the request again
              return next.handle(this.attachTokenToRequest(request));
            }
            return <any>this.authService.logout();
          }),
          catchError(error => {
            this.authService.logout();
            return this.handleGlobalError(error);
          }),
          finalize(() => {
            this.isTokenRefreshing = false;
          })
        );
      } else {
        // if token is not expired
        this.isTokenRefreshing = false;
        return this.tokenSubject.pipe(
          filter(token => token !== null),
          take(1),
          switchMap(token => {
            return next.handle(this.attachTokenToRequest(request));
          })
        );
      }
    }

    private handleGlobalError(errorResponse: HttpErrorResponse) {
      let errorMessage: string;
      if (errorResponse.error instanceof Error)  {
        errorMessage = 'An error occured: ' + errorResponse.error.message;
      } else {
        errorMessage = `Backend returned code ${errorResponse.status}, body was: ${errorResponse.error}`;
      }
      return throwError(errorMessage);
    }

    private attachTokenToRequest(request: HttpRequest<any>) {
      // debugger;
      const token = localStorage.getItem('token');

      return request.clone({
        setHeaders: {Authorization: `Bearer ${token}`}
      });
    }

}

export const JwtInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
};