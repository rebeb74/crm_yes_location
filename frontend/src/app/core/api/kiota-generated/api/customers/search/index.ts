/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type WithQueryItemRequestBuilder, WithQueryItemRequestBuilderRequestsMetadata } from './item/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/Customers/Search
 */
export interface SearchRequestBuilder extends BaseRequestBuilder<SearchRequestBuilder> {
    /**
     * Gets an item from the ApiSdk.api.Customers.Search.item collection
     * @param query Unique identifier of the item
     * @returns {WithQueryItemRequestBuilder}
     */
     byQuery(query: string) : WithQueryItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const SearchRequestBuilderUriTemplate = "{+baseurl}/api/Customers/Search";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const SearchRequestBuilderNavigationMetadata: Record<Exclude<keyof SearchRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byQuery: {
        requestsMetadata: WithQueryItemRequestBuilderRequestsMetadata,
        pathParametersMappings: ["query"],
    },
};
/* tslint:enable */
/* eslint-enable */
