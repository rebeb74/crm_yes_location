import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { EnvironmentProviders, Provider } from '@angular/core';
import { ApiConfiguration } from './api/api-configuration';
import { ApiService } from './api/api.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { AuthService } from './auth/auth.service';
import { AgencyService } from './services/agency.service';
import { BookingService } from './services/booking.service';
import { CustomerService } from './services/customer.service';
import { DurationTierService } from './services/duration-tier.service';
import { InvoiceService } from './services/invoice.service';
import { LocationService } from './services/location.service';
import { MaintenanceRecordService } from './services/maintenance-record.service';
import { PaymentService } from './services/payment.service';
import { QuotationService } from './services/quotation.service';
import { RoleService } from './services/role.service';
import { SeasonService } from './services/season.service';
import { UserService } from './services/user.service';
import { VehicleIncidentPhotoService } from './services/vehicle-incident-photo.service';
import { VehicleIncidentService } from './services/vehicle-incident.service';
import { VehicleInspectionService } from './services/vehicle-inspection.service';
import { VehiclePricingService } from './services/vehicle-pricing.service';
import { VehicleService } from './services/vehicle.service';
import { LocalStorageService } from './storage/local-storage.service';

/**
 * Fournit tous les services nécessaires pour l'application YesLocation
 */
export const appProviders: (Provider | EnvironmentProviders)[] = [
  provideHttpClient(withInterceptorsFromDi()),
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ApiConfiguration,
  ApiService,
  VehicleService,
  BookingService,
  CustomerService,
  VehicleInspectionService,
  VehicleIncidentService,
  InvoiceService,
  PaymentService,
  QuotationService,
  AgencyService,
  AuthService,
  LocalStorageService,
  DurationTierService,
  LocationService,
  MaintenanceRecordService,
  RoleService,
  SeasonService,
  UserService,
  VehicleIncidentPhotoService,
  VehiclePricingService,
];

/**
 * Fonction utilitaire pour obtenir les providers liés à l'authentification
 */
export function getAuthProviders(): Provider[] {
  return [
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    LocalStorageService,
  ];
}

/**
 * Fonction utilitaire pour obtenir les providers API
 */
export function getApiProviders(): Provider[] {
  return [ApiConfiguration, ApiService];
}

/**
 * Fonction utilitaire pour obtenir les providers de services métier
 */
export function getBusinessProviders(): Provider[] {
  return [
    VehicleService,
    BookingService,
    CustomerService,
    VehicleInspectionService,
    VehicleIncidentService,
    InvoiceService,
    PaymentService,
    QuotationService,
    AgencyService,
    DurationTierService,
    LocationService,
    MaintenanceRecordService,
    RoleService,
    SeasonService,
    UserService,
    VehicleIncidentPhotoService,
    VehiclePricingService,
  ];
}
