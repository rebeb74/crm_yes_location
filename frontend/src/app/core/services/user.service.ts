import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import { type UserDto, type UserInputDto } from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _users = new BehaviorSubject<UserDto[]>([]);
  private _currentUser = new BehaviorSubject<UserDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  users$: Observable<UserDto[]> = this._users.asObservable();
  currentUser$: Observable<UserDto | null> = this._currentUser.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<UserDto[]> {
    this._loading.next(true);

    return from(
      this.api.users
        .get()
        .then((response: UserDto[] | undefined) => {
          const users = response || [];
          this._users.next(users);
          return users;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des utilisateurs', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<UserDto | null> {
    this._loading.next(true);

    return from(
      this.api.users
        .byId(id)
        .get()
        .then((response: UserDto | undefined) => {
          if (response) this._currentUser.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement de l'utilisateur ${id}`,
            error
          );
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(userData: UserInputDto): Observable<UserDto | null> {
    this._loading.next(true);

    return from(
      this.api.users
        .post(userData)
        .then((response: UserDto | undefined) => {
          if (response) {
            const currentUsers = this._users.value;
            this._users.next([...currentUsers, response]);
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error("Erreur lors de la création de l'utilisateur", error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(id: number, userData: UserInputDto): Observable<UserDto | null> {
    this._loading.next(true);

    return from(
      this.api.users
        .byId(id)
        .put(userData)
        .then((response: ArrayBuffer | undefined) => {
          // PUT typically doesn't return data, so we need to fetch the updated entity
          return this.api.users.byId(id).get();
        })
        .then((response: UserDto | undefined) => {
          if (response) {
            const currentUsers = this._users.value;
            const index = currentUsers.findIndex((u) => u.id === id);

            if (index !== -1) {
              currentUsers[index] = response;
              this._users.next([...currentUsers]);
            }

            // If we're updating the currently selected user, update it too
            if (this._currentUser.value?.id === id) {
              this._currentUser.next(response);
            }
          }
          return response || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de l'utilisateur ${id}`,
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
      this.api.users
        .byId(id)
        .delete()
        .then(() => {
          const currentUsers = this._users.value;
          this._users.next(currentUsers.filter((u) => u.id !== id));

          if (this._currentUser.value?.id === id) {
            this._currentUser.next(null);
          }

          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de l'utilisateur ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
