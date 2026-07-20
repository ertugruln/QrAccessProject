import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // 1. Tarayıcının hafızasından token'ı alıyoruz
    const token = localStorage.getItem('token');

    // 2. Eğer token varsa, dışarı çıkan isteği kopyalayıp içine token'ı gömüyoruz
    if (token) {
      request = request.clone({
        setHeaders: {
          // DİKKAT: 'Bearer ' kelimesinden sonra bir boşluk bırakmak zorunludur!
          Authorization: `Bearer ${token}`
        }
      });
    }

    // 3. Modifiye edilmiş isteği yoluna (API'ye) devam etmesi için salıveriyoruz
    return next.handle(request);
  }
}