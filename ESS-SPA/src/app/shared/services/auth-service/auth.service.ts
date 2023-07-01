import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
// import { AUTH_URL } from '../../../constants/api.constant';
import { map } from 'rxjs/operators';
// import {IUser} from '../../entities/models/user';
import {JwtHelperService} from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { AUTH_URL, CUSTOMER_URL } from '../../../constants/api.constant';
import { IUser } from 'src/app/entities/models/user';
import { IAddress } from 'src/app/entities/models/address';
import { ToasterService } from '../toaster-service/toaster.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  decodedToken: any;
  private jwtHelper = new JwtHelperService();
  currentUser: IUser;
  organisationId: number;

 constructor(private http: HttpClient, private toasterService: ToasterService,
    private router: Router) { }

  login(model: any) {
    return this.http.post(AUTH_URL.LOGIN, model).pipe(
        map((response: any) => {
          const user = response;
          if (user) {
           // this.clearStorage();
            this.setLocalStorageValue(user);
          }
        })
      );
  }

  getNewrefreshToken(): Observable<any> {
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<any>(AUTH_URL.REFRESH_TOKEN, {refreshToken}).pipe(
      map((result) => {
        if (result && result.token) {
          this.setLocalStorageValue(result);
        }
        return result;
      })
    );
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    console.log(!this.jwtHelper.isTokenExpired(token));
    return !this.jwtHelper.isTokenExpired(token);
    //return !!token; // if have value, return true, else false;
  }
  
  logout() {
    this.clearStorage();
    this.router.navigate(['/login']);
    // this.toasterService.showInfo('Authentication', 'Logout successfully!');
  }

  roleMatch(allowedRoles: Array<string>): boolean {
    let isMatch = false;

   if (!this.decodedToken) {
     this.logout();
   }  

   const userRoles = this.decodedToken.role as Array<string>;

    allowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true;
        return;
      }
    });

    return isMatch;
  }

  // roleMatch(allowedRoles: Array<string>): boolean {
  //   let isMatch = false;

  //  if (!this.decodedToken) {
  //    this.logout();
  //  }

  //  const userRoles = this.decodedToken.role as Array<string>;

  //  console.log(userRoles);
  //   allowedRoles.forEach(element => {
  //     if (userRoles.includes(element)) {
  //       isMatch = true;
  //       return;
  //     }
  //   });

  //   return isMatch;
  // }

  getOrganisationId(): number{
    const userData: IUser = JSON.parse(localStorage.getItem('user') || "{}");

    return (userData.organisationId > 0 && userData.organisationId !== null) ? userData.organisationId : 0;
  }

  getCurrentUser(): IUser {
    const userData: IUser = JSON.parse(localStorage.getItem('user') || "{}");

    return userData ?? null;
  }

  clearStorage()
  {
    localStorage.removeItem('token');
    localStorage.removeItem('basket_id');
    localStorage.removeItem('user');
    localStorage.removeItem('refreshToken');
    this.currentUser = null!;
    this.decodedToken = null;
  }

  private setLocalStorageValue(result: any) {
    localStorage.setItem('token', result.token);
    localStorage.setItem('refreshToken', result.refreshToken);
    localStorage.setItem('user', JSON.stringify(result.user));
    this.currentUser = result.user;
    this.decodedToken = this.jwtHelper.decodeToken(result.token);
  }

  getUserAddress() {
    return this.http.get<IAddress>(CUSTOMER_URL.BASE_URL + `/users/${this.getCurrentUser().id}/selfservices/getUserAddress`);
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(CUSTOMER_URL.BASE_URL + `/users/${this.getCurrentUser().id}/selfservices/updateUserAddress`, address);
  }
  
}