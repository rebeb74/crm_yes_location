/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createVehiclePricingDtoFromDiscriminatorValue, type VehiclePricingDto } from '../../../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/VehiclePricings/ByVehicleAndSeason/{vehicleId}/{seasonId}
 */
export interface WithSeasonItemRequestBuilder extends BaseRequestBuilder<WithSeasonItemRequestBuilder> {
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<VehiclePricingDto[]>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<VehiclePricingDto[] | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const WithSeasonItemRequestBuilderUriTemplate = "{+baseurl}/api/VehiclePricings/ByVehicleAndSeason/{vehicleId}/{seasonId}";
/**
 * Metadata for all the requests in the request builder.
 */
export const WithSeasonItemRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: WithSeasonItemRequestBuilderUriTemplate,
        responseBodyContentType: "text/plain;q=0.9",
        adapterMethodName: "sendCollection",
        responseBodyFactory:  createVehiclePricingDtoFromDiscriminatorValue,
    },
};
/* tslint:enable */
/* eslint-enable */
