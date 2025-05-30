/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type WithPartLocationItemRequestBuilder, WithPartLocationItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/VehicleIncidents/ByPartLocation/{inspectionId}
 */
export interface WithInspectionItemRequestBuilder extends BaseRequestBuilder<WithInspectionItemRequestBuilder> {
    /**
     * Gets an item from the ApiSdk.api.VehicleIncidents.ByPartLocation.item.item collection
     * @param partLocation Unique identifier of the item
     * @returns {WithPartLocationItemRequestBuilder}
     */
     byPartLocation(partLocation: number) : WithPartLocationItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const WithInspectionItemRequestBuilderUriTemplate = "{+baseurl}/api/VehicleIncidents/ByPartLocation/{inspectionId}";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const WithInspectionItemRequestBuilderNavigationMetadata: Record<Exclude<keyof WithInspectionItemRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byPartLocation: {
        requestsMetadata: WithPartLocationItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["partLocation"],
    },
};
/* tslint:enable */
/* eslint-enable */
