/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type WithYearItemRequestBuilder, WithYearItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/Seasons/ByYear
 */
export interface ByYearRequestBuilder extends BaseRequestBuilder<ByYearRequestBuilder> {
    /**
     * Gets an item from the ApiSdk.api.Seasons.ByYear.item collection
     * @param year Unique identifier of the item
     * @returns {WithYearItemRequestBuilder}
     */
     byYear(year: number) : WithYearItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const ByYearRequestBuilderUriTemplate = "{+baseurl}/api/Seasons/ByYear";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const ByYearRequestBuilderNavigationMetadata: Record<Exclude<keyof ByYearRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byYear: {
        requestsMetadata: WithYearItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["year"],
    },
};
/* tslint:enable */
/* eslint-enable */
