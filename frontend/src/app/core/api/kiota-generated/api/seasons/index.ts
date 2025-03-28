/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createSeasonDtoFromDiscriminatorValue, serializeSeasonDto, serializeSeasonInputDto, type SeasonDto, type SeasonInputDto } from '../../models/index.js';
// @ts-ignore
import { ByYearRequestBuilderNavigationMetadata, type ByYearRequestBuilder } from './byYear/index.js';
// @ts-ignore
import { DefaultSeasonRequestBuilderRequestsMetadata, type DefaultSeasonRequestBuilder } from './defaultSeason/index.js';
// @ts-ignore
import { SeasonsItemRequestBuilderRequestsMetadata, type SeasonsItemRequestBuilder } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/Seasons
 */
export interface SeasonsRequestBuilder extends BaseRequestBuilder<SeasonsRequestBuilder> {
    /**
     * The ByYear property
     */
    get byYear(): ByYearRequestBuilder;
    /**
     * The DefaultSeason property
     */
    get defaultSeason(): DefaultSeasonRequestBuilder;
    /**
     * Gets an item from the ApiSdk.api.Seasons.item collection
     * @param id Unique identifier of the item
     * @returns {SeasonsItemRequestBuilder}
     */
     byId(id: number) : SeasonsItemRequestBuilder;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<SeasonDto[]>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<SeasonDto[] | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<SeasonDto>}
     */
     post(body: SeasonInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<SeasonDto | undefined>;
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
     toPostRequestInformation(body: SeasonInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const SeasonsRequestBuilderUriTemplate = "{+baseurl}/api/Seasons";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const SeasonsRequestBuilderNavigationMetadata: Record<Exclude<keyof SeasonsRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byId: {
        requestsMetadata: SeasonsItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["id"],
    },
    byYear: {
        navigationMetadata: ByYearRequestBuilderNavigationMetadata,
    },
    defaultSeason: {
        requestsMetadata: DefaultSeasonRequestBuilderRequestsMetadata,
    },
};
/**
 * Metadata for all the requests in the request builder.
 */
export const SeasonsRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: SeasonsRequestBuilderUriTemplate,
        responseBodyContentType: "text/plain;q=0.9",
        adapterMethodName: "sendCollection",
        responseBodyFactory:  createSeasonDtoFromDiscriminatorValue,
    },
    post: {
        uriTemplate: SeasonsRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createSeasonDtoFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeSeasonInputDto,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
