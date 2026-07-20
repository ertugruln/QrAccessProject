import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5197/api/Auth/Login'; 

  constructor(private http: HttpClient) { }

  login(loginData: any): Observable<any>{
    return this.http.post(this.apiUrl, loginData);
  }

  // 1. Yeni Veri Çekme Metodumuz (Token otomatik eklenecek!)
  getProtectedData(): Observable<any> {
    // Swagger'daki listeleme metodunun adresini buraya yazmalısın
    const dataUrl = 'http://localhost:5197/api/Kullanicilar'; 
    return this.http.get(dataUrl);
  }
}