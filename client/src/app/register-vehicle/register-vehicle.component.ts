import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { VehicleService, VehicleCreate } from '../services/vehicle';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule }      from '@angular/router';


@Component({
  selector: 'app-register-vehicle',
  standalone: true,
  imports: [
    CommonModule,         
    ReactiveFormsModule,  
    HttpClientModule,
    RouterModule       
  ],
  templateUrl: './register-vehicle.component.html',
  styleUrls: ['./register-vehicle.component.scss']
})
export class RegisterVehicleComponent {
  form: FormGroup;
  submitting = false;
  success = false;
  errorMsg = '';

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService
  ) {
    this.form = this.fb.group({
      licensePlate: ['', Validators.required],
      type: ['', Validators.required],
      isAvailable: [true]
    });
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.submitting = true;
    this.errorMsg = '';
    const data: VehicleCreate = this.form.value;
    this.vehicleService.registerVehicle(data)
      .subscribe({
        next: () => {
          this.success = true;
          this.form.reset({ isAvailable: true });
        },
        error: err => {
          this.errorMsg = err.message || 'Error al crear vehículo';
          this.submitting = false;
        },
        complete: () => this.submitting = false
      });
  }
}
