import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ROLE_URL } from 'src/app/constants/api.constant';
import { IRole } from 'src/app/entities/models/role';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient) { }

  getRoles() {
    return this.http.get<IRole[]>(ROLE_URL.BASE_URL);
  }

}
