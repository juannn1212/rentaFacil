// src/app/services/booking.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Booking {
  id: string;
  vehicleId: string;
  clientId: string;
  startDate: string;
  endDate: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private base = environment.bookingsApiUrl;

  constructor(private http: HttpClient) {}

  create(data: Partial<Booking>): Observable<Booking> {
    return this.http.post<Booking>(this.base, data);
  }

  history(clientId: string): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.base}/history/${clientId}`);
  }
}
