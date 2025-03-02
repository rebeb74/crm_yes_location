/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type WithSeasonItemRequestBuilder, WithSeasonItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/VehiclePricings/ByVehicleAndSeason/{vehicleId}
 */
export interface WithVehicleItemRequestBuilder extends BaseRequestBuilder<WithVehicleItemRequestBuilder> {
    /**
     * Gets an item from the ApiSdk.api.VehiclePricings.ByVehicleAndSeason.item.item collection
     * @param seasonId Unique identifier of the item
     * @returns {WithSeasonItemRequestBuilder}
     */
     bySeasonId(seasonId: number) : WithSeasonItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const WithVehicleItemRequestBuilderUriTemplate = "{+baseurl}/api/VehiclePricings/ByVehicleAndSeason/{vehicleId}";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const WithVehicleItemRequestBuilderNavigationMetadata: Record<Exclude<keyof WithVehicleItemRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    bySeasonId: {
        requestsMetadata: WithSeasonItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["seasonId"],
    },
};
/* tslint:enable */
/* eslint-enable */
