import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type SeasonDto,
  type SeasonInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class SeasonService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _seasons = new BehaviorSubject<SeasonDto[]>([]);
  private _currentSeason = new BehaviorSubject<SeasonDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  seasons$: Observable<SeasonDto[]> = this._seasons.asObservable();
  currentSeason$: Observable<SeasonDto | null> =
    this._currentSeason.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<SeasonDto[]> {
    this._loading.next(true);

    return from(
      this.api.seasons
        .get()
        .then((response: SeasonDto[] | undefined) => {
          const seasons = response || [];
          this._seasons.next(seasons);
          return seasons;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des saisons', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<SeasonDto | null> {
    this._loading.next(true);

    return from(
      this.api.seasons
        .byId(id)
        .get()
        .then((response: SeasonDto | undefined) => {
          if (response) this._currentSeason.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement de la saison ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(seasonData: SeasonInputDto): Observable<SeasonDto | null> {
    this._loading.next(true);

    return from(
      this.api.seasons
        .post(seasonData)
        .then((response: SeasonDto | undefined) => {
          if (response) {
            const currentSeasons = this._seasons.value;
            this._seasons.next([...currentSeasons, response]);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création de la saison', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(id: number, seasonData: SeasonInputDto): Observable<SeasonDto | null> {
    this._loading.next(true);

    return from(
      this.api.seasons
        .byId(id)
        .put(seasonData)
        .then((response: ArrayBuffer | undefined) => {
          // PUT typically doesn't return data, so we need to fetch the updated entity
          return this.api.seasons.byId(id).get();
        })
        .then((response: SeasonDto | undefined) => {
          if (response) {
            const currentSeasons = this._seasons.value;
            const index = currentSeasons.findIndex((s) => s.id === id);

            if (index !== -1) {
              currentSeasons[index] = response;
              this._seasons.next([...currentSeasons]);
            }

            this._currentSeason.next(response);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de la saison ${id}`,
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
      this.api.seasons
        .byId(id)
        .delete()
        .then(() => {
          const currentSeasons = this._seasons.value;
          this._seasons.next(currentSeasons.filter((s) => s.id !== id));

          if (this._currentSeason.value?.id === id) {
            this._currentSeason.next(null);
          }

          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de la saison ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
