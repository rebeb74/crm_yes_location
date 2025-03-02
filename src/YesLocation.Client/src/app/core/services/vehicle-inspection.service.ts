import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  VehicleInspectionDto,
  VehicleInspectionInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class VehicleInspectionService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _inspections = new BehaviorSubject<VehicleInspectionDto[]>([]);
  private _currentInspection = new BehaviorSubject<VehicleInspectionDto | null>(
    null
  );

  loading$: Observable<boolean> = this._loading.asObservable();
  inspections$: Observable<VehicleInspectionDto[]> =
    this._inspections.asObservable();
  currentInspection$: Observable<VehicleInspectionDto | null> =
    this._currentInspection.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<VehicleInspectionDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehicleInspections
        .get()
        .then((response) => {
          const inspections = response || [];
          this._inspections.next(inspections);
          return inspections;
        })
        .catch((error) => {
          console.error('Erreur lors du chargement des états des lieux', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<VehicleInspectionDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleInspections
        .byId(id)
        .get()
        .then((response: VehicleInspectionDto | undefined) => {
          if (response) this._currentInspection.next(response);
          return response || null;
        })
        .catch((error) => {
          console.error(
            `Erreur lors du chargement de l'état des lieux ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    inspectionData: VehicleInspectionInputDto
  ): Observable<VehicleInspectionDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleInspections
        .post(inspectionData)
        .then((response: VehicleInspectionDto | undefined) => {
          if (response) this._currentInspection.next(response);
          return response || null;
        })
        .catch((error) => {
          console.error(
            "Erreur lors de la création de l'état des lieux",
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    inspectionData: VehicleInspectionInputDto
  ): Observable<VehicleInspectionDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleInspections
        .byId(id)
        .put(inspectionData)
        .then((response) => {
          // Transformer la réponse en VehicleInspectionDto si nécessaire
          const inspectionResponse = response as unknown as
            | VehicleInspectionDto
            | undefined;
          if (inspectionResponse) {
            const updatedInspections = this._inspections.value.map((i) =>
              i.id === id ? inspectionResponse : i
            );
            this._inspections.next(updatedInspections);
            this._currentInspection.next(inspectionResponse);
          }
          return inspectionResponse || null;
        })
        .catch((error) => {
          console.error(
            `Erreur lors de la mise à jour de l'état des lieux ${id}`,
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
      this.api.vehicleInspections
        .byId(id)
        .delete()
        .then(() => {
          const updatedInspections = this._inspections.value.filter(
            (i) => i.id !== id
          );
          this._inspections.next(updatedInspections);
          if (this._currentInspection.value?.id === id) {
            this._currentInspection.next(null);
          }
          return true;
        })
        .catch((error) => {
          console.error(
            `Erreur lors de la suppression de l'état des lieux ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques
  loadByBooking(bookingId: number): Observable<VehicleInspectionDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehicleInspections.byBooking
        .byBookingId(bookingId)
        .get()
        .then((response) => {
          const inspections = response || [];
          return inspections;
        })
        .catch((error) => {
          console.error(
            `Erreur lors du chargement des états des lieux pour la réservation ${bookingId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
