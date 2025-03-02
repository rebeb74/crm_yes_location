import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type VehicleDto,
  type VehicleInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _vehicles = new BehaviorSubject<VehicleDto[]>([]);
  private _currentVehicle = new BehaviorSubject<VehicleDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  vehicles$: Observable<VehicleDto[]> = this._vehicles.asObservable();
  currentVehicle$: Observable<VehicleDto | null> =
    this._currentVehicle.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<VehicleDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehicles
        .get()
        .then((response: VehicleDto[] | undefined) => {
          const vehicles = response || [];
          this._vehicles.next(vehicles);
          return vehicles;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des véhicules', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<VehicleDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicles
        .byId(id)
        .get()
        .then((response: VehicleDto | undefined) => {
          if (response) this._currentVehicle.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement du véhicule ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(vehicleData: VehicleInputDto): Observable<VehicleDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicles
        .post(vehicleData)
        .then((response: VehicleDto | undefined) => {
          // Recharger la liste des véhicules après création
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création du véhicule', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    vehicleData: VehicleInputDto
  ): Observable<VehicleDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicles
        .byId(id)
        .put(vehicleData)
        .then((response) => {
          // Transformer la réponse en VehicleDto si nécessaire
          const vehicleResponse = response as unknown as VehicleDto | undefined;
          // Mettre à jour le véhicule dans la liste locale
          if (vehicleResponse) {
            const updatedVehicles = this._vehicles.value.map((v) =>
              v.id === id ? vehicleResponse : v
            );
            this._vehicles.next(updatedVehicles);
            this._currentVehicle.next(vehicleResponse);
          }
          return vehicleResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour du véhicule ${id}`,
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
      this.api.vehicles
        .byId(id)
        .delete()
        .then(() => {
          // Supprimer le véhicule de la liste locale
          const updatedVehicles = this._vehicles.value.filter(
            (v) => v.id !== id
          );
          this._vehicles.next(updatedVehicles);
          if (this._currentVehicle.value?.id === id) {
            this._currentVehicle.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression du véhicule ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
