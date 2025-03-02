import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import { type RoleDto } from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _roles = new BehaviorSubject<RoleDto[]>([]);
  private _currentRole = new BehaviorSubject<RoleDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  roles$: Observable<RoleDto[]> = this._roles.asObservable();
  currentRole$: Observable<RoleDto | null> = this._currentRole.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<RoleDto[]> {
    this._loading.next(true);

    return from(
      this.api.roles
        .get()
        .then((response: RoleDto[] | undefined) => {
          const roles = response || [];
          this._roles.next(roles);
          return roles;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des rôles', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<RoleDto | null> {
    this._loading.next(true);

    return from(
      this.api.roles
        .byId(id)
        .get()
        .then((response: RoleDto | undefined) => {
          if (response) this._currentRole.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement du rôle ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Note: Pour les rôles, il n'y a généralement pas de méthodes create, update et delete
  // disponibles pour les utilisateurs standard. Ces opérations sont souvent réservées
  // aux administrateurs système ou gérées via des processus spécifiques.
  // Si ces fonctionnalités sont requises, elles peuvent être ajoutées ici.
}
