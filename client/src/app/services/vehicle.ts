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

export interface VehicleCreate {
  licensePlate: string;
  type: string;
  isAvailable: boolean;
}

@Injectable({ providedIn: 'root' })
export class VehicleService {
  private base = environment.vehiclesApiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Obtener vehículos disponibles filtrados por tipo y fecha opcional.
   */
  getAvailable(type?: string, date?: string): Observable<Vehicle[]> {
    let params = new HttpParams();
    if (type) params = params.set('type', type);
    if (date) params = params.set('date', date);
    return this.http.get<Vehicle[]>(`${this.base}/availability`, { params });
  }

  /**
   * Crear un nuevo vehículo.
   * @param data Datos del vehículo a crear.
   */
  registerVehicle(data: VehicleCreate): Observable<Vehicle> {
    return this.http.post<Vehicle>(`${this.base}`, data);
  }

  /**
   * Listar todos los vehículos.
   */
  listAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.base}`);
  }

  /**
   * Obtener detalle de un vehículo por ID.
   */
  getById(id: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.base}/${id}`);
  }

  /**
   * Actualizar un vehículo existente.
   */
  updateVehicle(id: string, data: Partial<VehicleCreate>): Observable<Vehicle> {
    return this.http.put<Vehicle>(`${this.base}/${id}`, data);
  }

  /**
   * Eliminar un vehículo.
   */
  deleteVehicle(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
