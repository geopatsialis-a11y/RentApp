import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpRequest, provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorsInterceptor } from '../core/interceptors/errors-interceptor';
import { authInterceptor } from '../core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        errorsInterceptor,
        authInterceptor
      ])
    ),
    provideZonelessChangeDetection()
    ]
};
