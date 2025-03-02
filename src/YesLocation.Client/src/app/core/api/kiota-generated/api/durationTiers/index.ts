/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createDurationTierDtoFromDiscriminatorValue, serializeDurationTierDto, serializeDurationTierInputDto, type DurationTierDto, type DurationTierInputDto } from '../../models/index.js';
// @ts-ignore
import { DurationTiersItemRequestBuilderRequestsMetadata, type DurationTiersItemRequestBuilder } from './item/index.js';
// @ts-ignore
import { OrderedRequestBuilderRequestsMetadata, type OrderedRequestBuilder } from './ordered/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/DurationTiers
 */
export interface DurationTiersRequestBuilder extends BaseRequestBuilder<DurationTiersRequestBuilder> {
    /**
     * The Ordered property
     */
    get ordered(): OrderedRequestBuilder;
    /**
     * Gets an item from the ApiSdk.api.DurationTiers.item collection
     * @param id Unique identifier of the item
     * @returns {DurationTiersItemRequestBuilder}
     */
     byId(id: number) : DurationTiersItemRequestBuilder;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<DurationTierDto[]>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<DurationTierDto[] | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<DurationTierDto>}
     */
     post(body: DurationTierInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<DurationTierDto | undefined>;
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
     toPostRequestInformation(body: DurationTierInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const DurationTiersRequestBuilderUriTemplate = "{+baseurl}/api/DurationTiers";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const DurationTiersRequestBuilderNavigationMetadata: Record<Exclude<keyof DurationTiersRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byId: {
        requestsMetadata: DurationTiersItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["id"],
    },
    ordered: {
        requestsMetadata: OrderedRequestBuilderRequestsMetadata,
    },
};
/**
 * Metadata for all the requests in the request builder.
 */
export const DurationTiersRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: DurationTiersRequestBuilderUriTemplate,
        responseBodyContentType: "text/plain;q=0.9",
        adapterMethodName: "sendCollection",
        responseBodyFactory:  createDurationTierDtoFromDiscriminatorValue,
    },
    post: {
        uriTemplate: DurationTiersRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createDurationTierDtoFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeDurationTierInputDto,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
