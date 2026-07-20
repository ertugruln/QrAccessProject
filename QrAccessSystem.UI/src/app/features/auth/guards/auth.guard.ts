import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  // Angular'ın yönlendirme servisini (Router) içeri alıyoruz (inject)
  const router = inject(Router); 
  
  // Tarayıcının hafızasındaki token'ı okuyoruz
  const token = localStorage.getItem('token'); 

  if (token) {
    return true; // Token bulunduysa kapıyı aç
  } else {
    router.navigate(['/login']); // Token yoksa kullanıcıyı login ekranına geri şutla
    return false; // Geçişi engelle
  }
};