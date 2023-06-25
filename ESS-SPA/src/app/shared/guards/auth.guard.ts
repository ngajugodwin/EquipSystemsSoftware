import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
// import { AuthService } from '../services/auth-service/auth.service';
import {AuthService} from '../services/auth-service/auth.service';
import { INavData } from '@coreui/angular';
import { ToasterService } from '../services/toaster-service/toaster.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService,
      private toasterService: ToasterService,
        private router: Router) {}

    canActivate(next: ActivatedRouteSnapshot): boolean {
      const roles = next.firstChild?.data['roles'] as Array<string>;

      if (roles) {
        const match = this.authService.roleMatch(roles);
        if (match) {
          return true;
        } else {
          this.router.navigate(['/dashboard']);
          this.toasterService.showError('Auth', 'You do not have sufficient permission to access this area');
        }
      }

      if (this.authService.isLoggedIn()) {
          return true;
      }

      this.router.navigate(['/login']);
      return false;
    }
}