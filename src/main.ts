// src/main.ts
import 'zone.js'; 
import { enableProdMode, importProvidersFrom } from '@angular/core';
import { bootstrapApplication }               from '@angular/platform-browser';
import { provideRouter }                      from '@angular/router';
import { HttpClientModule }                   from '@angular/common/http';
import { ReactiveFormsModule }                from '@angular/forms';

import { AppComponent } from './app/app.component';
import { appRoutes }   from './app/app.routes';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(appRoutes),
    importProvidersFrom(HttpClientModule, ReactiveFormsModule)
  ]
})
.catch(err => console.error(err));
