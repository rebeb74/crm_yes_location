/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createVehicleInspectionDtoFromDiscriminatorValue, serializeVehicleInspectionDto, serializeVehicleInspectionInputDto, type VehicleInspectionDto, type VehicleInspectionInputDto } from '../../models/index.js';
// @ts-ignore
import { ByBookingRequestBuilderNavigationMetadata, type ByBookingRequestBuilder } from './byBooking/index.js';
// @ts-ignore
import { ByTypeRequestBuilderNavigationMetadata, type ByTypeRequestBuilder } from './byType/index.js';
// @ts-ignore
import { type VehicleInspectionsItemRequestBuilder, VehicleInspectionsItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/VehicleInspections
 */
export interface VehicleInspectionsRequestBuilder extends BaseRequestBuilder<VehicleInspectionsRequestBuilder> {
    /**
     * The ByBooking property
     */
    get byBooking(): ByBookingRequestBuilder;
    /**
     * The ByType property
     */
    get byType(): ByTypeRequestBuilder;
    /**
     * Gets an item from the ApiSdk.api.VehicleInspections.item collection
     * @param id Unique identifier of the item
     * @returns {VehicleInspectionsItemRequestBuilder}
     */
     byId(id: number) : VehicleInspectionsItemRequestBuilder;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<VehicleInspectionDto[]>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<VehicleInspectionDto[] | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<VehicleInspectionDto>}
     */
     post(body: VehicleInspectionInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<VehicleInspectionDto | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toPostRequestInformation(body: VehicleInspectionInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const VehicleInspectionsRequestBuilderUriTemplate = "{+baseurl}/api/VehicleInspections";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const VehicleInspectionsRequestBuilderNavigationMetadata: Record<Exclude<keyof VehicleInspectionsRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byId: {
        requestsMetadata: VehicleInspectionsItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["id"],
    },
    byBooking: {
        navigationMetadata: ByBookingRequestBuilderNavigationMetadata,
    },
    byType: {
        navigationMetadata: ByTypeRequestBuilderNavigationMetadata,
    },
};
/**
 * Metadata for all the requests in the request builder.
 */
export const VehicleInspectionsRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: VehicleInspectionsRequestBuilderUriTemplate,
        responseBodyContentType: "text/plain;q=0.9",
        adapterMethodName: "sendCollection",
        responseBodyFactory:  createVehicleInspectionDtoFromDiscriminatorValue,
    },
    post: {
        uriTemplate: VehicleInspectionsRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createVehicleInspectionDtoFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeVehicleInspectionInputDto,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
