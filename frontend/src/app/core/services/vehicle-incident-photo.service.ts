import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type VehicleIncidentPhotoDto,
  type VehicleIncidentPhotoInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class VehicleIncidentPhotoService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _photos = new BehaviorSubject<VehicleIncidentPhotoDto[]>([]);
  private _currentPhoto = new BehaviorSubject<VehicleIncidentPhotoDto | null>(
    null
  );

  loading$: Observable<boolean> = this._loading.asObservable();
  photos$: Observable<VehicleIncidentPhotoDto[]> = this._photos.asObservable();
  currentPhoto$: Observable<VehicleIncidentPhotoDto | null> =
    this._currentPhoto.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<VehicleIncidentPhotoDto[]> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidentPhotos
        .get()
        .then((response: VehicleIncidentPhotoDto[] | undefined) => {
          const photos = response || [];
          this._photos.next(photos);
          return photos;
        })
        .catch((error: any) => {
          console.error(
            "Erreur lors du chargement des photos d'incident de véhicule",
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<VehicleIncidentPhotoDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidentPhotos
        .byId(id)
        .get()
        .then((response: VehicleIncidentPhotoDto | undefined) => {
          if (response) this._currentPhoto.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement de la photo d'incident ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(
    photoData: VehicleIncidentPhotoInputDto
  ): Observable<VehicleIncidentPhotoDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidentPhotos
        .post(photoData)
        .then((response: VehicleIncidentPhotoDto | undefined) => {
          if (response) {
            const currentPhotos = this._photos.value;
            this._photos.next([...currentPhotos, response]);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error("Erreur lors de l'ajout de la photo d'incident", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    photoData: VehicleIncidentPhotoInputDto
  ): Observable<VehicleIncidentPhotoDto | null> {
    this._loading.next(true);

    return from(
      this.api.vehicleIncidentPhotos
        .byId(id)
        .put(photoData)
        .then((response: ArrayBuffer | undefined) => {
          // PUT typically doesn't return data, so we need to fetch the updated entity
          return this.api.vehicleIncidentPhotos.byId(id).get();
        })
        .then((response: VehicleIncidentPhotoDto | undefined) => {
          if (response) {
            const currentPhotos = this._photos.value;
            const index = currentPhotos.findIndex((p) => p.id === id);

            if (index !== -1) {
              currentPhotos[index] = response;
              this._photos.next([...currentPhotos]);
            }

            this._currentPhoto.next(response);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de la photo d'incident ${id}`,
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
      this.api.vehicleIncidentPhotos
        .byId(id)
        .delete()
        .then(() => {
          const currentPhotos = this._photos.value;
          this._photos.next(currentPhotos.filter((p) => p.id !== id));

          if (this._currentPhoto.value?.id === id) {
            this._currentPhoto.next(null);
          }

          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de la photo d'incident ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadByIncidentId(incidentId: number): Observable<VehicleIncidentPhotoDto[]> {
    this._loading.next(true);

    // Si l'endpoint direct n'existe pas, on peut récupérer toutes les photos
    // et les filtrer côté client
    return from(
      this.api.vehicleIncidentPhotos
        .get()
        .then((response: VehicleIncidentPhotoDto[] | undefined) => {
          const photos = (response || []).filter(
            (photo) => photo.vehicleIncidentId === incidentId
          );
          return photos;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des photos pour l'incident ${incidentId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
