import { CanActivateFn, Router } from '@angular/router';
import { routes } from '../app.routes';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {

  const router=inject(Router)
  const token=localStorage.getItem("token");
  if(token!=null)
  {
    return true
  }
  else{
    router.navigateByUrl("/")
    return false
  }

};
