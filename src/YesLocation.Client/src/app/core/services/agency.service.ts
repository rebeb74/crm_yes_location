import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type AgencyDto,
  type AgencyInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class AgencyService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _agencies = new BehaviorSubject<AgencyDto[]>([]);
  private _currentAgency = new BehaviorSubject<AgencyDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  agencies$: Observable<AgencyDto[]> = this._agencies.asObservable();
  currentAgency$: Observable<AgencyDto | null> =
    this._currentAgency.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<AgencyDto[]> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .get()
        .then((response: AgencyDto[] | undefined) => {
          const agencies = response || [];
          this._agencies.next(agencies);
          return agencies;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des agences', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<AgencyDto | null> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .byId(id)
        .get()
        .then((response: AgencyDto | undefined) => {
          if (response) this._currentAgency.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement de l'agence ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(agencyData: AgencyInputDto): Observable<AgencyDto | null> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .post(agencyData)
        .then((response: AgencyDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error("Erreur lors de la création de l'agence", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(id: number, agencyData: AgencyInputDto): Observable<AgencyDto | null> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .byId(id)
        .put(agencyData)
        .then((response) => {
          // Transformer la réponse en AgencyDto si nécessaire
          const agencyResponse = response as unknown as AgencyDto | undefined;
          if (agencyResponse) {
            const updatedAgencies = this._agencies.value.map((a) =>
              a.id === id ? agencyResponse : a
            );
            this._agencies.next(updatedAgencies);
            this._currentAgency.next(agencyResponse);
          }
          return agencyResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de l'agence ${id}`,
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
      this.api.agencies
        .byId(id)
        .delete()
        .then(() => {
          const updatedAgencies = this._agencies.value.filter(
            (a) => a.id !== id
          );
          this._agencies.next(updatedAgencies);
          if (this._currentAgency.value?.id === id) {
            this._currentAgency.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de l'agence ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
