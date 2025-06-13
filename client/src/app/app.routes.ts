import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { VehiclesSearchComponent } from './vehicles-search/vehicles-search.component';
import { BookingFormComponent } from './booking-form/booking-form.component';
import { BookingHistoryComponent } from './booking-history/booking-history.component';
import { RegisterVehicleComponent } from './register-vehicle/register-vehicle.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'search', component: VehiclesSearchComponent },
  { path: 'book', component: BookingFormComponent },
  { path: 'history', component: BookingHistoryComponent },
  { path: 'register-vehicle', component: RegisterVehicleComponent },
  
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
