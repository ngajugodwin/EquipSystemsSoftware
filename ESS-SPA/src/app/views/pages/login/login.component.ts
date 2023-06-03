import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model:any = {};
  isProcessing = false;

  constructor(private authService: AuthService, private router: Router) { }

  login() {
    this.authService.clearStorage();
    this.isProcessing = true;
    this.authService.login(this.model).subscribe({
      next: (response) => {
        this.router.navigate(['/dashboard']);
      console.log('Login successfully');
      console.log(response);
      }, error: (error: ErrorResponse) => {
        this.isProcessing = false;
        console.log(error);
      }
    });
  }


}
