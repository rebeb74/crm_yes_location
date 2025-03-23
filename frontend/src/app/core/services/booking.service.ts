import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type BookingDto,
  type BookingInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _bookings = new BehaviorSubject<BookingDto[]>([]);
  private _currentBooking = new BehaviorSubject<BookingDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  bookings$: Observable<BookingDto[]> = this._bookings.asObservable();
  currentBooking$: Observable<BookingDto | null> =
    this._currentBooking.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<BookingDto[]> {
    this._loading.next(true);

    return from(
      this.api.bookings
        .get()
        .then((response: BookingDto[] | undefined) => {
          const bookings = response || [];
          this._bookings.next(bookings);
          return bookings;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des réservations', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<BookingDto | null> {
    this._loading.next(true);

    return from(
      this.api.bookings
        .byId(id)
        .get()
        .then((response: BookingDto | undefined) => {
          if (response) this._currentBooking.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement de la réservation ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(bookingData: BookingInputDto): Observable<BookingDto | null> {
    this._loading.next(true);

    return from(
      this.api.bookings
        .post(bookingData)
        .then((response: BookingDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création de la réservation', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    bookingData: BookingInputDto
  ): Observable<BookingDto | null> {
    this._loading.next(true);

    return from(
      this.api.bookings
        .byId(id)
        .put(bookingData)
        .then((response) => {
          // Transformer la réponse en BookingDto si nécessaire
          const bookingResponse = response as unknown as BookingDto | undefined;
          if (bookingResponse) {
            const updatedBookings = this._bookings.value.map((b) =>
              b.id === id ? bookingResponse : b
            );
            this._bookings.next(updatedBookings);
            this._currentBooking.next(bookingResponse);
          }
          return bookingResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de la réservation ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  delete(id: number): Observable<boolean> {
    this._loading.next(true);

    return from(
      this.api.bookings
        .byId(id)
        .delete()
        .then(() => {
          const updatedBookings = this._bookings.value.filter(
            (b) => b.id !== id
          );
          this._bookings.next(updatedBookings);
          if (this._currentBooking.value?.id === id) {
            this._currentBooking.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de la réservation ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux réservations
  loadByCustomerId(customerId: number): Observable<BookingDto[]> {
    this._loading.next(true);

    return from(
      this.api.bookings.byCustomer
        .byCustomerId(customerId)
        .get()
        .then((response: BookingDto[] | undefined) => {
          const bookings = response || [];
          return bookings;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des réservations du client ${customerId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadByVehicleId(vehicleId: number): Observable<BookingDto[]> {
    this._loading.next(true);

    return from(
      this.api.bookings.byVehicle
        .byVehicleId(vehicleId)
        .get()
        .then((response: BookingDto[] | undefined) => {
          const bookings = response || [];
          return bookings;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des réservations du véhicule ${vehicleId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
