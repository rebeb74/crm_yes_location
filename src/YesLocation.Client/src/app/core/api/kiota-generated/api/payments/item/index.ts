/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createPaymentDtoFromDiscriminatorValue, serializePaymentInputDto, type PaymentDto, type PaymentInputDto } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/Payments/{id}
 */
export interface PaymentsItemRequestBuilder extends BaseRequestBuilder<PaymentsItemRequestBuilder> {
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     */
     delete(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<PaymentDto>}
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<PaymentDto | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     */
     put(body: PaymentInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
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
     toPutRequestInformation(body: PaymentInputDto, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const PaymentsItemRequestBuilderUriTemplate = "{+baseurl}/api/Payments/{id}";
/**
 * Metadata for all the requests in the request builder.
 */
export const PaymentsItemRequestBuilderRequestsMetadata: RequestsMetadata = {
    delete: {
        uriTemplate: PaymentsItemRequestBuilderUriTemplate,
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
    },
    get: {
        uriTemplate: PaymentsItemRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        adapterMethodName: "send",
        responseBodyFactory:  createPaymentDtoFromDiscriminatorValue,
    },
    put: {
        uriTemplate: PaymentsItemRequestBuilderUriTemplate,
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
        requestBodyContentType: "application/json",
        requestBodySerializer: serializePaymentInputDto,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
