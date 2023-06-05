import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AUTH_URL } from 'src/app/constants/api.constant';
import { IUser } from 'src/app/entities/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

constructor(private http: HttpClient) { }

createUserAccount(userForCreation: IUser) {
  let params = new HttpParams();
  params = new HttpParams().set('isExternalReg', true)

  return this.http.post<IUser>(AUTH_URL.REGISTER, userForCreation,  {params: params});
}

updateUser(userId: number, userToUpdate: IUser) {
  // return this.http.put(USER_URL.BASE_URL + userId, userToUpdate);
}


}
