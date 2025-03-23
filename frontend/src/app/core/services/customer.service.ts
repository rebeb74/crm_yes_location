import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type CustomerDto,
  type CustomerInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _customers = new BehaviorSubject<CustomerDto[]>([]);
  private _currentCustomer = new BehaviorSubject<CustomerDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  customers$: Observable<CustomerDto[]> = this._customers.asObservable();
  currentCustomer$: Observable<CustomerDto | null> =
    this._currentCustomer.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<CustomerDto[]> {
    this._loading.next(true);

    return from(
      this.api.customers
        .get()
        .then((response: CustomerDto[] | undefined) => {
          const customers = response || [];
          this._customers.next(customers);
          return customers;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des clients', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<CustomerDto | null> {
    this._loading.next(true);

    return from(
      this.api.customers
        .byId(id)
        .get()
        .then((response: CustomerDto | undefined) => {
          if (response) this._currentCustomer.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement du client ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(customerData: CustomerInputDto): Observable<CustomerDto | null> {
    this._loading.next(true);

    return from(
      this.api.customers
        .post(customerData)
        .then((response: CustomerDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création du client', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    customerData: CustomerInputDto
  ): Observable<CustomerDto | null> {
    this._loading.next(true);

    return from(
      this.api.customers
        .byId(id)
        .put(customerData)
        .then((response) => {
          // Transformer la réponse en CustomerDto si nécessaire
          const customerResponse = response as unknown as
            | CustomerDto
            | undefined;
          if (customerResponse) {
            const updatedCustomers = this._customers.value.map((c) =>
              c.id === id ? customerResponse : c
            );
            this._customers.next(updatedCustomers);
            this._currentCustomer.next(customerResponse);
          }
          return customerResponse || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors de la mise à jour du client ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  delete(id: number): Observable<boolean> {
    this._loading.next(true);

    return from(
      this.api.customers
        .byId(id)
        .delete()
        .then(() => {
          const updatedCustomers = this._customers.value.filter(
            (c) => c.id !== id
          );
          this._customers.next(updatedCustomers);
          if (this._currentCustomer.value?.id === id) {
            this._currentCustomer.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(`Erreur lors de la suppression du client ${id}`, error);
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthode de recherche
  search(query: string): Observable<CustomerDto[]> {
    this._loading.next(true);

    return from(
      this.api.customers.search
        .byQuery(query)
        .get()
        .then((response: CustomerDto[] | undefined) => {
          const customers = response || [];
          return customers;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la recherche de clients "${query}"`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
}
