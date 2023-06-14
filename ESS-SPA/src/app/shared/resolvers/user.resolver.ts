import { Observable, empty, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { IUser } from '../../entities/models/user';
import { ManageAdminOrganisationService } from '../services/manage-admin-organisaton-service/manage-admin-organisation.service';
import { AuthService } from '../services/auth-service/auth.service';


@Injectable()

export class UserResolver implements Resolve<IUser> {

    constructor(private manageAdminOrganisationService: ManageAdminOrganisationService,
        private authService: AuthService,
       private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): IUser | Observable<IUser> | Promise<IUser> {
        return this.manageAdminOrganisationService.getUser(this.authService.getOrganisationId(), route.params['id']).pipe(
            catchError((err) => {
                console.log(err);
                this.router.navigate(['/organisation-admin/manage-organisation-users']);
                return of(err);
            })
           
        )
    }
}