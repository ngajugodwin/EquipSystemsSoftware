import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { SELF_SERVICE_URL } from 'src/app/constants/api.constant';
import { IUser } from 'src/app/entities/models/user';

@Injectable()
export class SelfService {

constructor(private http: HttpClient) { }


getUserProfile(currentUserId: number): Observable<IUser> {
    return this.http.get<IUser>(SELF_SERVICE_URL.BASE_URL + `${currentUserId}/selfservices/getUserProfile`);
  }

changePassword(currentUserId: number, passwordData: object): Observable<IUser> {
    return this.http.put<IUser>(SELF_SERVICE_URL.BASE_URL + `${currentUserId}/selfservices/`, passwordData);
  }


}
