import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  if (localStorage.getItem('user') !== null)
    return true;

  const router = inject(Router);
  const toastr = inject(ToastrService);

  toastr.info('Usuário não autentidado!');
  router.navigate(['/user/login']);
  return false;
};
