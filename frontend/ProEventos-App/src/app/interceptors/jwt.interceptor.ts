import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { take } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  let curretUser: User = new User;

  accountService.currentUser$.pipe(take(1)).subscribe({
    next: user => {
      if (user != null) curretUser = user;

      if (curretUser.token) {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ${curretUser.token}`
          }
        });
      }
    }
  });
  
  return next(req);
};
