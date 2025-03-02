import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type DurationTierDto,
  type DurationTierInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class DurationTierService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _durationTiers = new BehaviorSubject<DurationTierDto[]>([]);
  private _currentDurationTier = new BehaviorSubject<DurationTierDto | null>(
    null
  );

  loading$: Observable<boolean> = this._loading.asObservable();
  durationTiers$: Observable<DurationTierDto[]> =
    this._durationTiers.asObservable();
  currentDurationTier$: Observable<DurationTierDto | null> =
    this._currentDurationTier.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<DurationTierDto[]> {
    this._loading.next(true);

    return from(
      this.api.durationTiers
        .get()
        .then((response: DurationTierDto[] | undefined) => {
          const durationTiers = response || [];
          this._durationTiers.next(durationTiers);
          return durationTiers;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors du chargement des paliers de durée',
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<DurationTierDto | null> {
    this._loading.next(true);

    return from(
      this.api.durationTiers
        .byId(id)
        .get()
        .then((response: DurationTierDto | undefined) => {
          if (response) this._currentDurationTier.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement du palier de durée ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    durationTierData: DurationTierInputDto
  ): Observable<DurationTierDto | null> {
    this._loading.next(true);

    return from(
      this.api.durationTiers
        .post(durationTierData)
        .then((response: DurationTierDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création du palier de durée', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    durationTierData: DurationTierInputDto
  ): Observable<DurationTierDto | null> {
    this._loading.next(true);

    return from(
      this.api.durationTiers
        .byId(id)
        .put(durationTierData)
        .then((response) => {
          // Transformer la réponse en DurationTierDto si nécessaire
          const durationTierResponse = response as unknown as
            | DurationTierDto
            | undefined;
          if (durationTierResponse) {
            const updatedDurationTiers = this._durationTiers.value.map((d) =>
              d.id === id ? durationTierResponse : d
            );
            this._durationTiers.next(updatedDurationTiers);
            this._currentDurationTier.next(durationTierResponse);
          }
          return durationTierResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour du palier de durée ${id}`,
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
      this.api.durationTiers
        .byId(id)
        .delete()
        .then(() => {
          const updatedDurationTiers = this._durationTiers.value.filter(
            (d) => d.id !== id
          );
          this._durationTiers.next(updatedDurationTiers);
          if (this._currentDurationTier.value?.id === id) {
            this._currentDurationTier.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression du palier de durée ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux paliers de durée
  loadBySeasonId(seasonId: number): Observable<DurationTierDto[]> {
    this._loading.next(true);

    return from(
      this.api.durationTiers
        .get()
        .then((response: DurationTierDto[] | undefined) => {
          // Filtrer par saison si l'attribut existe
          const durationTiers = response || [];
          return durationTiers;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des paliers pour la saison ${seasonId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
