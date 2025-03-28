/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { AgencyRequestBuilderNavigationMetadata, AgencyRequestBuilderRequestsMetadata, type AgencyRequestBuilder } from './agency/index.js';
// @ts-ignore
import { BookingsRequestBuilderNavigationMetadata, BookingsRequestBuilderRequestsMetadata, type BookingsRequestBuilder } from './bookings/index.js';
// @ts-ignore
import { CustomersRequestBuilderNavigationMetadata, CustomersRequestBuilderRequestsMetadata, type CustomersRequestBuilder } from './customers/index.js';
// @ts-ignore
import { DurationTiersRequestBuilderNavigationMetadata, DurationTiersRequestBuilderRequestsMetadata, type DurationTiersRequestBuilder } from './durationTiers/index.js';
// @ts-ignore
import { InvoicesRequestBuilderNavigationMetadata, InvoicesRequestBuilderRequestsMetadata, type InvoicesRequestBuilder } from './invoices/index.js';
// @ts-ignore
import { LocationsRequestBuilderNavigationMetadata, LocationsRequestBuilderRequestsMetadata, type LocationsRequestBuilder } from './locations/index.js';
// @ts-ignore
import { MaintenanceRecordsRequestBuilderNavigationMetadata, MaintenanceRecordsRequestBuilderRequestsMetadata, type MaintenanceRecordsRequestBuilder } from './maintenanceRecords/index.js';
// @ts-ignore
import { PaymentsRequestBuilderNavigationMetadata, PaymentsRequestBuilderRequestsMetadata, type PaymentsRequestBuilder } from './payments/index.js';
// @ts-ignore
import { QuotationsRequestBuilderNavigationMetadata, QuotationsRequestBuilderRequestsMetadata, type QuotationsRequestBuilder } from './quotations/index.js';
// @ts-ignore
import { RoleRequestBuilderNavigationMetadata, RoleRequestBuilderRequestsMetadata, type RoleRequestBuilder } from './role/index.js';
// @ts-ignore
import { SeasonsRequestBuilderNavigationMetadata, SeasonsRequestBuilderRequestsMetadata, type SeasonsRequestBuilder } from './seasons/index.js';
// @ts-ignore
import { type UserRequestBuilder, UserRequestBuilderNavigationMetadata, UserRequestBuilderRequestsMetadata } from './user/index.js';
// @ts-ignore
import { type VehicleIncidentPhotosRequestBuilder, VehicleIncidentPhotosRequestBuilderNavigationMetadata, VehicleIncidentPhotosRequestBuilderRequestsMetadata } from './vehicleIncidentPhotos/index.js';
// @ts-ignore
import { type VehicleIncidentsRequestBuilder, VehicleIncidentsRequestBuilderNavigationMetadata, VehicleIncidentsRequestBuilderRequestsMetadata } from './vehicleIncidents/index.js';
// @ts-ignore
import { type VehicleInspectionsRequestBuilder, VehicleInspectionsRequestBuilderNavigationMetadata, VehicleInspectionsRequestBuilderRequestsMetadata } from './vehicleInspections/index.js';
// @ts-ignore
import { type VehiclePricingsRequestBuilder, VehiclePricingsRequestBuilderNavigationMetadata, VehiclePricingsRequestBuilderRequestsMetadata } from './vehiclePricings/index.js';
// @ts-ignore
import { type VehiclesRequestBuilder, VehiclesRequestBuilderNavigationMetadata, VehiclesRequestBuilderRequestsMetadata } from './vehicles/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api
 */
export interface ApiRequestBuilder extends BaseRequestBuilder<ApiRequestBuilder> {
    /**
     * The Agency property
     */
    get agency(): AgencyRequestBuilder;
    /**
     * The Bookings property
     */
    get bookings(): BookingsRequestBuilder;
    /**
     * The Customers property
     */
    get customers(): CustomersRequestBuilder;
    /**
     * The DurationTiers property
     */
    get durationTiers(): DurationTiersRequestBuilder;
    /**
     * The Invoices property
     */
    get invoices(): InvoicesRequestBuilder;
    /**
     * The Locations property
     */
    get locations(): LocationsRequestBuilder;
    /**
     * The MaintenanceRecords property
     */
    get maintenanceRecords(): MaintenanceRecordsRequestBuilder;
    /**
     * The Payments property
     */
    get payments(): PaymentsRequestBuilder;
    /**
     * The Quotations property
     */
    get quotations(): QuotationsRequestBuilder;
    /**
     * The Role property
     */
    get role(): RoleRequestBuilder;
    /**
     * The Seasons property
     */
    get seasons(): SeasonsRequestBuilder;
    /**
     * The User property
     */
    get user(): UserRequestBuilder;
    /**
     * The VehicleIncidentPhotos property
     */
    get vehicleIncidentPhotos(): VehicleIncidentPhotosRequestBuilder;
    /**
     * The VehicleIncidents property
     */
    get vehicleIncidents(): VehicleIncidentsRequestBuilder;
    /**
     * The VehicleInspections property
     */
    get vehicleInspections(): VehicleInspectionsRequestBuilder;
    /**
     * The VehiclePricings property
     */
    get vehiclePricings(): VehiclePricingsRequestBuilder;
    /**
     * The Vehicles property
     */
    get vehicles(): VehiclesRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const ApiRequestBuilderUriTemplate = "{+baseurl}/api";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const ApiRequestBuilderNavigationMetadata: Record<Exclude<keyof ApiRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    agency: {
        requestsMetadata: AgencyRequestBuilderRequestsMetadata,
        navigationMetadata: AgencyRequestBuilderNavigationMetadata,
    },
    bookings: {
        requestsMetadata: BookingsRequestBuilderRequestsMetadata,
        navigationMetadata: BookingsRequestBuilderNavigationMetadata,
    },
    customers: {
        requestsMetadata: CustomersRequestBuilderRequestsMetadata,
        navigationMetadata: CustomersRequestBuilderNavigationMetadata,
    },
    durationTiers: {
        requestsMetadata: DurationTiersRequestBuilderRequestsMetadata,
        navigationMetadata: DurationTiersRequestBuilderNavigationMetadata,
    },
    invoices: {
        requestsMetadata: InvoicesRequestBuilderRequestsMetadata,
        navigationMetadata: InvoicesRequestBuilderNavigationMetadata,
    },
    locations: {
        requestsMetadata: LocationsRequestBuilderRequestsMetadata,
        navigationMetadata: LocationsRequestBuilderNavigationMetadata,
    },
    maintenanceRecords: {
        requestsMetadata: MaintenanceRecordsRequestBuilderRequestsMetadata,
        navigationMetadata: MaintenanceRecordsRequestBuilderNavigationMetadata,
    },
    payments: {
        requestsMetadata: PaymentsRequestBuilderRequestsMetadata,
        navigationMetadata: PaymentsRequestBuilderNavigationMetadata,
    },
    quotations: {
        requestsMetadata: QuotationsRequestBuilderRequestsMetadata,
        navigationMetadata: QuotationsRequestBuilderNavigationMetadata,
    },
    role: {
        requestsMetadata: RoleRequestBuilderRequestsMetadata,
        navigationMetadata: RoleRequestBuilderNavigationMetadata,
    },
    seasons: {
        requestsMetadata: SeasonsRequestBuilderRequestsMetadata,
        navigationMetadata: SeasonsRequestBuilderNavigationMetadata,
    },
    user: {
        requestsMetadata: UserRequestBuilderRequestsMetadata,
        navigationMetadata: UserRequestBuilderNavigationMetadata,
    },
    vehicleIncidentPhotos: {
        requestsMetadata: VehicleIncidentPhotosRequestBuilderRequestsMetadata,
        navigationMetadata: VehicleIncidentPhotosRequestBuilderNavigationMetadata,
    },
    vehicleIncidents: {
        requestsMetadata: VehicleIncidentsRequestBuilderRequestsMetadata,
        navigationMetadata: VehicleIncidentsRequestBuilderNavigationMetadata,
    },
    vehicleInspections: {
        requestsMetadata: VehicleInspectionsRequestBuilderRequestsMetadata,
        navigationMetadata: VehicleInspectionsRequestBuilderNavigationMetadata,
    },
    vehiclePricings: {
        requestsMetadata: VehiclePricingsRequestBuilderRequestsMetadata,
        navigationMetadata: VehiclePricingsRequestBuilderNavigationMetadata,
    },
    vehicles: {
        requestsMetadata: VehiclesRequestBuilderRequestsMetadata,
        navigationMetadata: VehiclesRequestBuilderNavigationMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
