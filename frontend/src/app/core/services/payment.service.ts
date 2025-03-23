import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type PaymentDto,
  type PaymentInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _payments = new BehaviorSubject<PaymentDto[]>([]);
  private _currentPayment = new BehaviorSubject<PaymentDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  payments$: Observable<PaymentDto[]> = this._payments.asObservable();
  currentPayment$: Observable<PaymentDto | null> =
    this._currentPayment.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<PaymentDto[]> {
    this._loading.next(true);

    return from(
      this.api.payments
        .get()
        .then((response: PaymentDto[] | undefined) => {
          const payments = response || [];
          this._payments.next(payments);
          return payments;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des paiements', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<PaymentDto | null> {
    this._loading.next(true);

    return from(
      this.api.payments
        .byId(id)
        .get()
        .then((response: PaymentDto | undefined) => {
          if (response) this._currentPayment.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement du paiement ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(paymentData: PaymentInputDto): Observable<PaymentDto | null> {
    this._loading.next(true);

    return from(
      this.api.payments
        .post(paymentData)
        .then((response: PaymentDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création du paiement', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    paymentData: PaymentInputDto
  ): Observable<PaymentDto | null> {
    this._loading.next(true);

    return from(
      this.api.payments
        .byId(id)
        .put(paymentData)
        .then((response) => {
          // Transformer la réponse en PaymentDto si nécessaire
          const paymentResponse = response as unknown as PaymentDto | undefined;
          if (paymentResponse) {
            const updatedPayments = this._payments.value.map((p) =>
              p.id === id ? paymentResponse : p
            );
            this._payments.next(updatedPayments);
            this._currentPayment.next(paymentResponse);
          }
          return paymentResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour du paiement ${id}`,
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
      this.api.payments
        .byId(id)
        .delete()
        .then(() => {
          const updatedPayments = this._payments.value.filter(
            (p) => p.id !== id
          );
          this._payments.next(updatedPayments);
          if (this._currentPayment.value?.id === id) {
            this._currentPayment.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression du paiement ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
