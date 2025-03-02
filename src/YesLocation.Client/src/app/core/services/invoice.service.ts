import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type InvoiceDto,
  type InvoiceInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _invoices = new BehaviorSubject<InvoiceDto[]>([]);
  private _currentInvoice = new BehaviorSubject<InvoiceDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  invoices$: Observable<InvoiceDto[]> = this._invoices.asObservable();
  currentInvoice$: Observable<InvoiceDto | null> =
    this._currentInvoice.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<InvoiceDto[]> {
    this._loading.next(true);

    return from(
      this.api.invoices
        .get()
        .then((response: InvoiceDto[] | undefined) => {
          const invoices = response || [];
          this._invoices.next(invoices);
          return invoices;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des factures', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<InvoiceDto | null> {
    this._loading.next(true);

    return from(
      this.api.invoices
        .byId(id)
        .get()
        .then((response: InvoiceDto | undefined) => {
          if (response) this._currentInvoice.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement de la facture ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(invoiceData: InvoiceInputDto): Observable<InvoiceDto | null> {
    this._loading.next(true);

    return from(
      this.api.invoices
        .post(invoiceData)
        .then((response: InvoiceDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création de la facture', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    invoiceData: InvoiceInputDto
  ): Observable<InvoiceDto | null> {
    this._loading.next(true);

    return from(
      this.api.invoices
        .byId(id)
        .put(invoiceData)
        .then((response) => {
          // Transformer la réponse en InvoiceDto si nécessaire
          const invoiceResponse = response as unknown as InvoiceDto | undefined;
          if (invoiceResponse) {
            const updatedInvoices = this._invoices.value.map((i) =>
              i.id === id ? invoiceResponse : i
            );
            this._invoices.next(updatedInvoices);
            this._currentInvoice.next(invoiceResponse);
          }
          return invoiceResponse || null;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la mise à jour de la facture ${id}`,
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
      this.api.invoices
        .byId(id)
        .delete()
        .then(() => {
          const updatedInvoices = this._invoices.value.filter(
            (i) => i.id !== id
          );
          this._invoices.next(updatedInvoices);
          if (this._currentInvoice.value?.id === id) {
            this._currentInvoice.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la suppression de la facture ${id}`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux factures - Ces méthodes devront être implémentées
  // si les endpoints correspondants sont disponibles dans l'API
  /*
  loadByCustomerId(customerId: number): Observable<InvoiceDto[]> {
    this._loading.next(true);

    // Vérifier si l'endpoint existe dans l'API
    return from(
      this.api.invoices
        .get()
        .then((response: InvoiceDto[] | undefined) => {
          const invoices = (response || []).filter(
            (invoice) => invoice.customerId === customerId
          );
          return invoices;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des factures du client ${customerId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadByBookingId(bookingId: number): Observable<InvoiceDto[]> {
    this._loading.next(true);

    // Vérifier si l'endpoint existe dans l'API
    return from(
      this.api.invoices
        .get()
        .then((response: InvoiceDto[] | undefined) => {
          const invoices = (response || []).filter(
            (invoice) => invoice.bookingId === bookingId
          );
          return invoices;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des factures de la réservation ${bookingId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }
  */
}
