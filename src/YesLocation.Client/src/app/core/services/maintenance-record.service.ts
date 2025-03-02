import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type MaintenanceRecordDto,
  type MaintenanceRecordInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class MaintenanceRecordService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _records = new BehaviorSubject<MaintenanceRecordDto[]>([]);
  private _currentRecord = new BehaviorSubject<MaintenanceRecordDto | null>(
    null
  );

  loading$: Observable<boolean> = this._loading.asObservable();
  records$: Observable<MaintenanceRecordDto[]> = this._records.asObservable();
  currentRecord$: Observable<MaintenanceRecordDto | null> =
    this._currentRecord.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<MaintenanceRecordDto[]> {
    this._loading.next(true);

    return from(
      this.api.maintenanceRecords
        .get()
        .then((response: MaintenanceRecordDto[] | undefined) => {
          const records = response || [];
          this._records.next(records);
          return records;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors du chargement des dossiers de maintenance',
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<MaintenanceRecordDto | null> {
    this._loading.next(true);

    return from(
      this.api.maintenanceRecords
        .byId(id)
        .get()
        .then((response: MaintenanceRecordDto | undefined) => {
          if (response) this._currentRecord.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement du dossier de maintenance ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    recordData: MaintenanceRecordInputDto
  ): Observable<MaintenanceRecordDto | null> {
    this._loading.next(true);

    return from(
      this.api.maintenanceRecords
        .post(recordData)
        .then((response: MaintenanceRecordDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            'Erreur lors de la création du dossier de maintenance',
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    recordData: MaintenanceRecordInputDto
  ): Observable<MaintenanceRecordDto | null> {
    this._loading.next(true);

    return from(
      this.api.maintenanceRecords
        .byId(id)
        .put(recordData)
        .then((response) => {
          // Transformer la réponse en MaintenanceRecordDto si nécessaire
          const recordResponse = response as unknown as
            | MaintenanceRecordDto
            | undefined;
          if (recordResponse) {
            const updatedRecords = this._records.value.map((r) =>
              r.id === id ? recordResponse : r
            );
            this._records.next(updatedRecords);
            this._currentRecord.next(recordResponse);
          }
          return recordResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour du dossier de maintenance ${id}`,
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
      this.api.maintenanceRecords
        .byId(id)
        .delete()
        .then(() => {
          const updatedRecords = this._records.value.filter((r) => r.id !== id);
          this._records.next(updatedRecords);
          if (this._currentRecord.value?.id === id) {
            this._currentRecord.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression du dossier de maintenance ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux dossiers de maintenance
  loadByVehicleId(vehicleId: number): Observable<MaintenanceRecordDto[]> {
    this._loading.next(true);

    return from(
      this.api.maintenanceRecords
        .get()
        .then((response: MaintenanceRecordDto[] | undefined) => {
          // Filtrer par véhicule si l'attribut existe
          const records = response || [];
          return records;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des dossiers pour le véhicule ${vehicleId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
