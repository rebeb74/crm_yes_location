import { Injectable } from '@angular/core';
import { ApiConfiguration } from './api-configuration';
import { type ApiClient } from './kiota-generated/apiClient';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiClient: ApiClient;

  // Clients pour chaque domaine directement exposés depuis l'API client
  get vehicles() {
    return this.apiClient.api.vehicles;
  }

  get bookings() {
    return this.apiClient.api.bookings;
  }

  get customers() {
    return this.apiClient.api.customers;
  }

  get vehicleInspections() {
    return this.apiClient.api.vehicleInspections;
  }

  get vehicleIncidents() {
    return this.apiClient.api.vehicleIncidents;
  }

  get vehicleIncidentPhotos() {
    return this.apiClient.api.vehicleIncidentPhotos;
  }

  get locations() {
    return this.apiClient.api.locations;
  }

  get quotations() {
    return this.apiClient.api.quotations;
  }

  get invoices() {
    return this.apiClient.api.invoices;
  }

  get payments() {
    return this.apiClient.api.payments;
  }

  get maintenanceRecords() {
    return this.apiClient.api.maintenanceRecords;
  }

  get agencies() {
    return this.apiClient.api.agency;
  }

  get seasons() {
    return this.apiClient.api.seasons;
  }

  get durationTiers() {
    return this.apiClient.api.durationTiers;
  }

  get vehiclePricings() {
    return this.apiClient.api.vehiclePricings;
  }

  get users() {
    return this.apiClient.api.user;
  }

  get roles() {
    return this.apiClient.api.role;
  }

  // Client d'authentification
  get auth() {
    return this.apiClient.auth;
  }

  constructor(private apiConfig: ApiConfiguration) {
    this.apiClient = this.apiConfig.createApiClient();
  }
}
