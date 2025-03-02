/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { LoginRequestBuilderRequestsMetadata, type LoginRequestBuilder } from './login/index.js';
// @ts-ignore
import { RefreshTokenRequestBuilderRequestsMetadata, type RefreshTokenRequestBuilder } from './refreshToken/index.js';
// @ts-ignore
import { RegisterRequestBuilderRequestsMetadata, type RegisterRequestBuilder } from './register/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /Auth
 */
export interface AuthRequestBuilder extends BaseRequestBuilder<AuthRequestBuilder> {
    /**
     * The Login property
     */
    get login(): LoginRequestBuilder;
    /**
     * The RefreshToken property
     */
    get refreshToken(): RefreshTokenRequestBuilder;
    /**
     * The Register property
     */
    get register(): RegisterRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const AuthRequestBuilderUriTemplate = "{+baseurl}/Auth";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const AuthRequestBuilderNavigationMetadata: Record<Exclude<keyof AuthRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    login: {
        requestsMetadata: LoginRequestBuilderRequestsMetadata,
    },
    refreshToken: {
        requestsMetadata: RefreshTokenRequestBuilderRequestsMetadata,
    },
    register: {
        requestsMetadata: RegisterRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
