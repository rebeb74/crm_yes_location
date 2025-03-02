/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createUserDtoFromDiscriminatorValue, serializeUserInputDto, type UserDto, type UserInputDto } from '../../../models/index.js';
// @ts-ignore
import { RolesRequestBuilderNavigationMetadata, RolesRequestBuilderRequestsMetadata, type RolesRequestBuilder } from './roles/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/User/{-id}
 */
export interface ItemRequestBuilder extends BaseRequestBuilder<ItemRequestBuilder> {
    /**
     * The roles property
     */
    get roles(): RolesRequestBuilder;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     */
     delete(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<UserDto>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<UserDto | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     */
     put(body: UserInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toDeleteRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
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
     toPutRequestInformation(body: UserInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const ItemRequestBuilderUriTemplate = "{+baseurl}/api/User/{%2Did}";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const ItemRequestBuilderNavigationMetadata: Record<Exclude<keyof ItemRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    roles: {
        requestsMetadata: RolesRequestBuilderRequestsMetadata,
        navigationMetadata: RolesRequestBuilderNavigationMetadata,
    },
};
/**
 * Metadata for all the requests in the request builder.
 */
export const ItemRequestBuilderRequestsMetadata: RequestsMetadata = {
    delete: {
        uriTemplate: ItemRequestBuilderUriTemplate,
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
    },
    get: {
        uriTemplate: ItemRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createUserDtoFromDiscriminatorValue,
    },
    put: {
        uriTemplate: ItemRequestBuilderUriTemplate,
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeUserInputDto,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
