/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /Auth/RefreshToken
 */
export interface RefreshTokenRequestBuilder extends BaseRequestBuilder<RefreshTokenRequestBuilder> {
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<string>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<string | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const RefreshTokenRequestBuilderUriTemplate = "{+baseurl}/Auth/RefreshToken";
/**
 * Metadata for all the requests in the request builder.
 */
export const RefreshTokenRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: RefreshTokenRequestBuilderUriTemplate,
        responseBodyContentType: "text/plain;q=0.9",
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "string",
    },
};
/* tslint:enable */
/* eslint-enable */
