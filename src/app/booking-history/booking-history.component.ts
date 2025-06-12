import { Component, OnInit }          from '@angular/core';
import { CommonModule }               from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { RouterModule }               from '@angular/router';
import { BookingService, Booking }    from '../services/booking';

@Component({
  selector: 'app-booking-history',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './booking-history.component.html',
  styleUrls: ['./booking-history.component.scss']
})
export class BookingHistoryComponent implements OnInit {
  filterForm!: FormGroup;
  allBookings: Booking[] = [];
  bookings: Booking[]    = [];

  constructor(private fb: FormBuilder, private bs: BookingService) {}

  ngOnInit(): void {
    this.filterForm = this.fb.group({
      clientId: [''],
      start:    [''],
      end:      [''],
      status:   ['all']
    });
  }

  /** Carga cruda por Client ID */
  loadHistory(): void {
    const id = this.filterForm.value.clientId;
    if (!id) return;
    this.bs.history(id).subscribe(data => {
      this.allBookings = data;
      this.applyFilters();
    });
  }

  applyFilters(): void {
    const { start, end, status } = this.filterForm.value;
    const startDt = start ? new Date(start) : null;
    const endDt   = end
      ? new Date(new Date(end).setHours(23,59,59,999))
      : null;

    this.bookings = this.allBookings.filter(b => {
      const sd = new Date(b.startDate);
      const ed = new Date(b.endDate);
      if (startDt && sd < startDt) return false;
      if (endDt   && ed > endDt)   return false;
      if (status !== 'all' && b.status.toLowerCase() !== status) return false;
      return true;
    });
  }
}
