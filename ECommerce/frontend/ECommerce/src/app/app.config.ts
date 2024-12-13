import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { DetailsOfUsersService } from './services/DetailsOfUserService/details-of-users.service';
import { custominterceptorInterceptor } from './interceptor/custominterceptor.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideToastr } from 'ngx-toastr';
export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    provideHttpClient(),
    provideHttpClient(withInterceptors([custominterceptorInterceptor])), provideAnimationsAsync(),
  provideToastr()]
};
