// 1. OnInit arayüzünü import ettik
import { Component, OnInit } from '@angular/core'; 
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service'; // 2. Servisi import ettik

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  // 3. API'den gelecek verileri tutacağımız boş liste
  dataList: any[] = []; 

  // 4. Servisi constructor içine ekledik
  constructor(private router: Router, private authService: AuthService) {}

  // 5. Sayfa açılır açılmaz çalışacak olan metod
  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.authService.getProtectedData().subscribe({
      next: (response: any) => {
        console.log("API'den Veriler Geldi!", response);
        // Not: Senin API yapına göre dönen veri doğrudan response veya response.data olabilir
        this.dataList = response; 
      },
      error: (err: any) => {
        console.error("Veri çekilirken hata:", err);
        // Eğer 401 Unauthorized verirse Token geçersiz demektir
      }
    });
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}