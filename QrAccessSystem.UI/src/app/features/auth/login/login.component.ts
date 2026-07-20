import { Component } from '@angular/core';
import { Router } from '@angular/router'; 
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginObj: any = {
    username: '',
    password: ''
  };

 
  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.authService.login(this.loginObj).subscribe({
      next: (response: any) => {
        
        
        const token = response.data; 
        
        
        localStorage.setItem('token', token);

        
        this.router.navigate(['/dashboard']);
      },
      error: (err: any) => {
        console.error("GİRİŞ HATALI!", err);
        alert("Giriş başarısız. Lütfen bilgileri kontrol edin.");
      }
    });
  }
}