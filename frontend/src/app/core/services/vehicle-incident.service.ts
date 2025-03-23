import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type VehicleIncidentDto,
  type VehicleIncidentInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class VehicleIncidentService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _incidents = new BehaviorSubject<VehicleIncidentDto[]>([]);
  private _currentIncident = new BehaviorSubject<VehicleIncidentDto | null>(
    null
  );

  loading$: Observable<boolean> = this._loading.asObservable();
  incidents$: Observable<VehicleIncidentDto[]> = this._incidents.asObservable();
  currentIncident$: Observable<VehicleIncidentDto | null> =
    this._currentIncident.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<VehicleIncidentDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidents
        .get()
        .then((response: VehicleIncidentDto[] | undefined) => {
          const incidents = response || [];
          this._incidents.next(incidents);
          return incidents;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors du chargement des incidents de véhicule',
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<VehicleIncidentDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidents
        .byId(id)
        .get()
        .then((response: VehicleIncidentDto | undefined) => {
          if (response) this._currentIncident.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement de l'incident ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    incidentData: VehicleIncidentInputDto
  ): Observable<VehicleIncidentDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidents
        .post(incidentData)
        .then((response: VehicleIncidentDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error("Erreur lors de la création de l'incident", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    incidentData: VehicleIncidentInputDto
  ): Observable<VehicleIncidentDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidents
        .byId(id)
        .put(incidentData)
        .then((response) => {
          // Transformer la réponse en VehicleIncidentDto si nécessaire
          const incidentResponse = response as unknown as
            | VehicleIncidentDto
            | undefined;
          if (incidentResponse) {
            const updatedIncidents = this._incidents.value.map((i) =>
              i.id === id ? incidentResponse : i
            );
            this._incidents.next(updatedIncidents);
            this._currentIncident.next(incidentResponse);
          }
          return incidentResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de l'incident ${id}`,
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
      this.api.vehicleIncidents
        .byId(id)
        .delete()
        .then(() => {
          const updatedIncidents = this._incidents.value.filter(
            (i) => i.id !== id
          );
          this._incidents.next(updatedIncidents);
          if (this._currentIncident.value?.id === id) {
            this._currentIncident.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de l'incident ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
