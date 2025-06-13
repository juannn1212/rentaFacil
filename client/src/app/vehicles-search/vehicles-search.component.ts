import { Component, OnInit } from '@angular/core';
import { CommonModule }     from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule }  from '@angular/common/http';
import { RouterModule }      from '@angular/router';
import { VehicleService, Vehicle } from '../services/vehicle';

@Component({
  selector: 'app-vehicles-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  templateUrl: './vehicles-search.component.html',
  styleUrls: ['./vehicles-search.component.scss']
})
export class VehiclesSearchComponent implements OnInit {
  form!: FormGroup;
  vehicles: Vehicle[] = [];

  constructor(private fb: FormBuilder, private vs: VehicleService) {}

  ngOnInit(): void {
    this.form = this.fb.group({ type: [''], date: [''] });
  }

  onSearch(): void {
    const { type, date } = this.form.value;
    this.vs.getAvailable(type, date).subscribe((res: Vehicle[]) => {
      this.vehicles = res;
    });
  }
}
