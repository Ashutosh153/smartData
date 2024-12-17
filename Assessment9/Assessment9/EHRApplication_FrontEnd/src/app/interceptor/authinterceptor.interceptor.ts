import { HttpInterceptorFn } from '@angular/common/http';
import { LoaderServiceService } from '../services/loader-service.service';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';

export const authinterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderServiceService);
  loaderService.show();

  const token = localStorage.getItem('token');
  const authReq = token ? req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  }) : req;

  return next(authReq).pipe(
    finalize(() => loaderService.hide())
  );
};