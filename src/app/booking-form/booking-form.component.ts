import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule }      from '@angular/common';
import { HttpClientModule }  from '@angular/common/http';
import { RouterModule }      from '@angular/router';
import { BookingService, Booking } from '../services/booking';
import { VehicleService, Vehicle } from '../services/vehicle';

@Component({
  selector: 'app-booking-form',
  standalone: true,
  encapsulation: ViewEncapsulation.None,   
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './booking-form.component.html',
  styleUrls: ['./booking-form.component.scss']
})
export class BookingFormComponent implements OnInit {
  form!: FormGroup;
  booking?: Booking;
  vehicles: Vehicle[] = [];

  constructor(
    private fb: FormBuilder,
    private bs: BookingService,
    private vs: VehicleService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      clientId:   ['', Validators.required],
      vehicleId:  ['', Validators.required],
      startDate:  ['', Validators.required],
      endDate:    ['', Validators.required]
    });

    this.vs.getAvailable('', '').subscribe(list => this.vehicles = list);
  }

  onBook(): void {
    if (this.form.invalid) return;
    this.bs.create(this.form.value).subscribe(res => this.booking = res);
  }
}
