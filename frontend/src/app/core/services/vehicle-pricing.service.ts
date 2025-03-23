import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type VehiclePricingDto,
  type VehiclePricingInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class VehiclePricingService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _pricings = new BehaviorSubject<VehiclePricingDto[]>([]);
  private _currentPricing = new BehaviorSubject<VehiclePricingDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  pricings$: Observable<VehiclePricingDto[]> = this._pricings.asObservable();
  currentPricing$: Observable<VehiclePricingDto | null> =
    this._currentPricing.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<VehiclePricingDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehiclePricings
        .get()
        .then((response: VehiclePricingDto[] | undefined) => {
          const pricings = response || [];
          this._pricings.next(pricings);
          return pricings;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors du chargement des tarifs de véhicule',
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<VehiclePricingDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehiclePricings
        .byId(id)
        .get()
        .then((response: VehiclePricingDto | undefined) => {
          if (response) this._currentPricing.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement du tarif de véhicule ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    pricingData: VehiclePricingInputDto
  ): Observable<VehiclePricingDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehiclePricings
        .post(pricingData)
        .then((response: VehiclePricingDto | undefined) => {
          if (response) {
            const currentPricings = this._pricings.value;
            this._pricings.next([...currentPricings, response]);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors de la création du tarif de véhicule',
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    pricingData: VehiclePricingInputDto
  ): Observable<VehiclePricingDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehiclePricings
        .byId(id)
        .put(pricingData)
        .then((response: ArrayBuffer | undefined) => {
          // PUT typically doesn't return data, so we need to fetch the updated entity
          return this.api.vehiclePricings.byId(id).get();
        })
        .then((response: VehiclePricingDto | undefined) => {
          if (response) {
            const currentPricings = this._pricings.value;
            const index = currentPricings.findIndex((p) => p.id === id);

            if (index !== -1) {
              currentPricings[index] = response;
              this._pricings.next([...currentPricings]);
            }

            this._currentPricing.next(response);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour du tarif de véhicule ${id}`,
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
      this.api.vehiclePricings
        .byId(id)
        .delete()
        .then(() => {
          const currentPricings = this._pricings.value;
          this._pricings.next(currentPricings.filter((p) => p.id !== id));

          if (this._currentPricing.value?.id === id) {
            this._currentPricing.next(null);
          }

          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression du tarif de véhicule ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadByVehicleId(vehicleId: number): Observable<VehiclePricingDto[]> {
    this._loading.next(true);

    // Si l'endpoint direct n'existe pas, on peut récupérer tous les tarifs
    // et les filtrer côté client
    return from(
      this.api.vehiclePricings
        .get()
        .then((response: VehiclePricingDto[] | undefined) => {
          const pricings = (response || []).filter(
            (pricing) => pricing.vehicleId === vehicleId
          );
          return pricings;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des tarifs pour le véhicule ${vehicleId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadBySeasonId(seasonId: number): Observable<VehiclePricingDto[]> {
    this._loading.next(true);

    // Si l'endpoint direct n'existe pas, on peut récupérer tous les tarifs
    // et les filtrer côté client
    return from(
      this.api.vehiclePricings
        .get()
        .then((response: VehiclePricingDto[] | undefined) => {
          const pricings = (response || []).filter(
            (pricing) => pricing.seasonId === seasonId
          );
          return pricings;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des tarifs pour la saison ${seasonId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadByDurationTierId(
    durationTierId: number
  ): Observable<VehiclePricingDto[]> {
    this._loading.next(true);

    // Si l'endpoint direct n'existe pas, on peut récupérer tous les tarifs
    // et les filtrer côté client
    return from(
      this.api.vehiclePricings
        .get()
        .then((response: VehiclePricingDto[] | undefined) => {
          const pricings = (response || []).filter(
            (pricing) => pricing.durationTierId === durationTierId
          );
          return pricings;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des tarifs pour le palier de durée ${durationTierId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
