import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type LocationDto,
  type LocationInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _locations = new BehaviorSubject<LocationDto[]>([]);
  private _currentLocation = new BehaviorSubject<LocationDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  locations$: Observable<LocationDto[]> = this._locations.asObservable();
  currentLocation$: Observable<LocationDto | null> =
    this._currentLocation.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<LocationDto[]> {
    this._loading.next(true);

    return from(
      this.api.locations
        .get()
        .then((response: LocationDto[] | undefined) => {
          const locations = response || [];
          this._locations.next(locations);
          return locations;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des emplacements', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<LocationDto | null> {
    this._loading.next(true);

    return from(
      this.api.locations
        .byId(id)
        .get()
        .then((response: LocationDto | undefined) => {
          if (response) this._currentLocation.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement de l'emplacement ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(locationData: LocationInputDto): Observable<LocationDto | null> {
    this._loading.next(true);

    return from(
      this.api.locations
        .post(locationData)
        .then((response: LocationDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error("Erreur lors de la création de l'emplacement", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    locationData: LocationInputDto
  ): Observable<LocationDto | null> {
    this._loading.next(true);

    return from(
      this.api.locations
        .byId(id)
        .put(locationData)
        .then((response) => {
          // Transformer la réponse en LocationDto si nécessaire
          const locationResponse = response as unknown as
            | LocationDto
            | undefined;
          if (locationResponse) {
            const updatedLocations = this._locations.value.map((l) =>
              l.id === id ? locationResponse : l
            );
            this._locations.next(updatedLocations);
            this._currentLocation.next(locationResponse);
          }
          return locationResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de l'emplacement ${id}`,
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
      this.api.locations
        .byId(id)
        .delete()
        .then(() => {
          const updatedLocations = this._locations.value.filter(
            (l) => l.id !== id
          );
          this._locations.next(updatedLocations);
          if (this._currentLocation.value?.id === id) {
            this._currentLocation.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de l'emplacement ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux emplacements
  loadByAgencyId(agencyId: number): Observable<LocationDto[]> {
    this._loading.next(true);

    return from(
      this.api.locations
        .get()
        .then((response: LocationDto[] | undefined) => {
          // Filtrer par agence si l'attribut existe
          const locations = response || [];
          return locations;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des emplacements pour l'agence ${agencyId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
