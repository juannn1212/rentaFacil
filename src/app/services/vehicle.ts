// src/app/services/vehicle.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Vehicle {
  id: string;
  licensePlate: string;
  type: string;
  isAvailable: boolean;
}

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private base = environment.vehiclesApiUrl;

  constructor(private http: HttpClient) {}

  getAvailable(type?: string, date?: string): Observable<Vehicle[]> {
    let params = new HttpParams();
    if (type) params = params.set('type', type);
    if (date) params = params.set('date', date);
    return this.http.get<Vehicle[]>(`${this.base}/availability`, { params });
  }
}
