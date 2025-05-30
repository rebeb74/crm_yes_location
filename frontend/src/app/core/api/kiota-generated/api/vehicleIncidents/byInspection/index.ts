/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type WithInspectionItemRequestBuilder, WithInspectionItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/VehicleIncidents/ByInspection
 */
export interface ByInspectionRequestBuilder extends BaseRequestBuilder<ByInspectionRequestBuilder> {
    /**
     * Gets an item from the ApiSdk.api.VehicleIncidents.ByInspection.item collection
     * @param inspectionId Unique identifier of the item
     * @returns {WithInspectionItemRequestBuilder}
     */
     byInspectionId(inspectionId: number) : WithInspectionItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const ByInspectionRequestBuilderUriTemplate = "{+baseurl}/api/VehicleIncidents/ByInspection";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const ByInspectionRequestBuilderNavigationMetadata: Record<Exclude<keyof ByInspectionRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byInspectionId: {
        requestsMetadata: WithInspectionItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["inspectionId"],
    },
};
/* tslint:enable */
/* eslint-enable */
