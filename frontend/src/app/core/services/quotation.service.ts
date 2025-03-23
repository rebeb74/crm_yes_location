import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { ApiService } from '../api/api.service';
import {
  type QuotationDto,
  type QuotationInputDto,
} from '../api/kiota-generated/models';

@Injectable({
  providedIn: 'root',
})
export class QuotationService {
  private _loading = new BehaviorSubject<boolean>(false);
  private _quotations = new BehaviorSubject<QuotationDto[]>([]);
  private _currentQuotation = new BehaviorSubject<QuotationDto | null>(null);

  loading$: Observable<boolean> = this._loading.asObservable();
  quotations$: Observable<QuotationDto[]> = this._quotations.asObservable();
  currentQuotation$: Observable<QuotationDto | null> =
    this._currentQuotation.asObservable();

  constructor(private api: ApiService) {}

  loadAll(): Observable<QuotationDto[]> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .get()
        .then((response: QuotationDto[] | undefined) => {
          const quotations = response || [];
          this._quotations.next(quotations);
          return quotations;
        })
        .catch((error: any) => {
          console.error('Erreur lors du chargement des devis', error);
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  loadById(id: number): Observable<QuotationDto | null> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .byId(id)
        .get()
        .then((response: QuotationDto | undefined) => {
          if (response) this._currentQuotation.next(response);
          return response || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors du chargement du devis ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  create(quotationData: QuotationInputDto): Observable<QuotationDto | null> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .post(quotationData)
        .then((response: QuotationDto | undefined) => {
          this.loadAll();
          return response || null;
        })
        .catch((error: any) => {
          console.error('Erreur lors de la création du devis', error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  update(
    id: number,
    quotationData: QuotationInputDto
  ): Observable<QuotationDto | null> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .byId(id)
        .put(quotationData)
        .then((response) => {
          // Transformer la réponse en QuotationDto si nécessaire
          const quotationResponse = response as unknown as
            | QuotationDto
            | undefined;
          if (quotationResponse) {
            const updatedQuotations = this._quotations.value.map((q) =>
              q.id === id ? quotationResponse : q
            );
            this._quotations.next(updatedQuotations);
            this._currentQuotation.next(quotationResponse);
          }
          return quotationResponse || null;
        })
        .catch((error: any) => {
          console.error(`Erreur lors de la mise à jour du devis ${id}`, error);
          return null;
        })
        .finally(() => this._loading.next(false))
    );
  }

  delete(id: number): Observable<boolean> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .byId(id)
        .delete()
        .then(() => {
          const updatedQuotations = this._quotations.value.filter(
            (q) => q.id !== id
          );
          this._quotations.next(updatedQuotations);
          if (this._currentQuotation.value?.id === id) {
            this._currentQuotation.next(null);
          }
          return true;
        })
        .catch((error: any) => {
          console.error(`Erreur lors de la suppression du devis ${id}`, error);
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }

  // Méthodes spécifiques aux devis
  loadByCustomerId(customerId: number): Observable<QuotationDto[]> {
    this._loading.next(true);

    return from(
      this.api.quotations
        .get()
        .then((response: QuotationDto[] | undefined) => {
          // Si l'attribut customerId n'existe pas, cette méthode devra être ajustée
          const quotations = response || [];
          // Filtrer par client si l'attribut existe
          /*
          const quotations = (response || []).filter(
            (quotation) => quotation.customerId === customerId
          );
          */
          return quotations;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors du chargement des devis du client ${customerId}`,
            error
          );
          return [];
        })
        .finally(() => this._loading.next(false))
    );
  }

  convertToBooking(id: number): Observable<boolean> {
    this._loading.next(true);

    // Cette méthode dépend de l'implémentation du backend
    // Logique à adapter selon l'API disponible
    return from(
      Promise.resolve(true)
        .then(() => {
          console.log(`Le devis ${id} a été converti en réservation`);
          return true;
        })
        .catch((error: any) => {
          console.error(
            `Erreur lors de la conversion du devis ${id} en réservation`,
            error
          );
          return false;
        })
        .finally(() => this._loading.next(false))
    );
  }
}
