/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createSeasonDtoFromDiscriminatorValue, type SeasonDto } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/Seasons/DefaultSeason
 */
export interface DefaultSeasonRequestBuilder extends BaseRequestBuilder<DefaultSeasonRequestBuilder> {
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<SeasonDto>}
     */
     get(requestConfiguration?: RequestConfiguration<DefaultSeasonRequestBuilderGetQueryParameters> | undefined) : Promise<SeasonDto | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<DefaultSeasonRequestBuilderGetQueryParameters> | undefined) : RequestInformation;
}
export interface DefaultSeasonRequestBuilderGetQueryParameters {
    year?: number;
}
/**
 * Uri template for the request builder.
 */
export const DefaultSeasonRequestBuilderUriTemplate = "{+baseurl}/api/Seasons/DefaultSeason{?year*}";
/**
 * Metadata for all the requests in the request builder.
 */
export const DefaultSeasonRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: DefaultSeasonRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createSeasonDtoFromDiscriminatorValue,
    },
};
/* tslint:enable */
/* eslint-enable */
