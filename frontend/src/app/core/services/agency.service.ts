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

  loadAgency(): Observable<AgencyDto | null> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .byId(1)
        .get()
        .then((response: AgencyDto | undefined) => {
          const agency = response || null;
          this._currentAgency.next(agency);
          return agency;
        })
        .catch((error: any) => {
          console.error("Erreur lors du chargement de l'agence", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(agencyData: AgencyInputDto): Observable<AgencyDto | null> {
    this._loading.next(true);

    return from(
      this.api.agencies
        .byId(1)
        .put(agencyData)
        .then((response) => {
          // Transformer la réponse en AgencyDto si nécessaire
          const agencyResponse = response as unknown as AgencyDto | undefined;
          if (agencyResponse) {
            this._currentAgency.next(agencyResponse);
          }
          return agencyResponse || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors de la mise à jour de l'agence`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
