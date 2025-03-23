import {
  BackingStoreFactorySingleton,
  DefaultApiError,
  Headers,
  HttpMethod,
  ParseNodeFactoryRegistry,
  ResponseHandlerOptionKey,
  SerializationWriterFactoryRegistry,
  SpanStatusCode,
  enableBackingStoreForParseNodeFactory,
  enableBackingStoreForSerializationWriterFactory,
  inNodeEnv,
  trace
} from "./chunk-K64FEM4K.js";
import {
  __async
} from "./chunk-TXDUYLVM.js";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/customFetchHandler.js
var CustomFetchHandler = class {
  constructor(customFetch) {
    this.customFetch = customFetch;
  }
  /**
   * @inheritdoc
   */
  execute(url, requestInit) {
    return __async(this, null, function* () {
      return yield this.customFetch(url, requestInit);
    });
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/httpClient.js
var HttpClient = class {
  /**
   *
   * Creates an instance of a HttpClient which contains the middlewares and fetch implementation for request execution.
   * @param customFetch - custom fetch function - a Fetch API implementation
   * @param middlewares - an array of Middleware handlers
   */
  constructor(customFetch, ...middlewares) {
    this.customFetch = customFetch;
    middlewares = (middlewares === null || middlewares === void 0 ? void 0 : middlewares.length) && middlewares[0] ? middlewares : MiddlewareFactory.getDefaultMiddlewares(customFetch);
    if (this.customFetch) {
      middlewares.push(new CustomFetchHandler(customFetch));
    }
    console.debug("Registered middlewares: " + middlewares.map((m) => m.constructor.name).join(", "));
    console.debug("Hint: To improve performance, use MiddlewareFactory.getPerformanceMiddlewares(customFetch) instead of MiddlewareFactory.getDefaultMiddlewares(customFetch)");
    this.setMiddleware(...middlewares);
  }
  /**
   * Processes the middleware parameter passed to set this.middleware property
   * The calling function should validate if middleware is not undefined or not empty.
   * @param middleware - The middleware passed
   */
  setMiddleware(...middleware) {
    for (let i = 0; i < middleware.length - 1; i++) {
      middleware[i].next = middleware[i + 1];
    }
    this.middleware = middleware[0];
  }
  /**
   * Executes a request and returns a promise resolving the response.
   * @param url the request url.
   * @param requestInit the RequestInit object.
   * @param requestOptions the request options.
   * @returns the promise resolving the response.
   */
  executeFetch(url, requestInit, requestOptions) {
    return __async(this, null, function* () {
      if (this.middleware) {
        return yield this.middleware.execute(url, requestInit, requestOptions);
      } else if (this.customFetch) {
        return this.customFetch(url, requestInit);
      }
      throw new Error("Please provide middlewares or a custom fetch function to execute the request");
    });
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/observabilityOptions.js
var ObservabilityOptionKey = "ObservabilityOptionKey";
var ObservabilityOptionsImpl = class {
  constructor(originalOptions) {
    this._originalOptions = originalOptions !== null && originalOptions !== void 0 ? originalOptions : {};
  }
  getKey() {
    return ObservabilityOptionKey;
  }
  get includeEUIIAttributes() {
    return this._originalOptions.includeEUIIAttributes;
  }
  set includeEUIIAttributes(value) {
    this._originalOptions.includeEUIIAttributes = value;
  }
  getTracerInstrumentationName() {
    return "@microsoft/kiota-http-fetchlibrary";
  }
};
function getObservabilityOptionsFromRequest(requestOptions) {
  if (requestOptions) {
    const observabilityOptions = requestOptions[ObservabilityOptionKey];
    if (observabilityOptions instanceof ObservabilityOptionsImpl) {
      return observabilityOptions;
    }
  }
  return void 0;
}

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/fetchRequestAdapter.js
var FetchRequestAdapter = class _FetchRequestAdapter {
  getSerializationWriterFactory() {
    return this.serializationWriterFactory;
  }
  /**
   * Instantiates a new request adapter.
   * @param authenticationProvider the authentication provider to use.
   * @param parseNodeFactory the parse node factory to deserialize responses.
   * @param serializationWriterFactory the serialization writer factory to use to serialize request bodies.
   * @param httpClient the http client to use to execute requests.
   * @param observabilityOptions the observability options to use.
   */
  constructor(authenticationProvider, parseNodeFactory = ParseNodeFactoryRegistry.defaultInstance, serializationWriterFactory = SerializationWriterFactoryRegistry.defaultInstance, httpClient = new HttpClient(), observabilityOptions = new ObservabilityOptionsImpl()) {
    this.authenticationProvider = authenticationProvider;
    this.parseNodeFactory = parseNodeFactory;
    this.serializationWriterFactory = serializationWriterFactory;
    this.httpClient = httpClient;
    this.baseUrl = "";
    this.getResponseContentType = (response) => {
      var _a;
      const header = (_a = response.headers.get("content-type")) === null || _a === void 0 ? void 0 : _a.toLowerCase();
      if (!header) return void 0;
      const segments = header.split(";");
      if (segments.length === 0) return void 0;
      else return segments[0];
    };
    this.getResponseHandler = (response) => {
      const options = response.getRequestOptions();
      const responseHandlerOption = options[ResponseHandlerOptionKey];
      return responseHandlerOption === null || responseHandlerOption === void 0 ? void 0 : responseHandlerOption.responseHandler;
    };
    this.sendCollectionOfPrimitive = (requestInfo, responseType, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendCollectionOfPrimitive", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              switch (responseType) {
                case "string":
                case "number":
                case "boolean":
                case "Date":
                  const rootNode = yield this.getRootParseNode(response);
                  return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan(`getCollectionOf${responseType}Value`, (deserializeSpan) => {
                    try {
                      span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, responseType);
                      if (responseType === "string") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "number") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "boolean") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "Date") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "Duration") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "DateOnly") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else if (responseType === "TimeOnly") {
                        return rootNode.getCollectionOfPrimitiveValues();
                      } else {
                        throw new Error("unexpected type to deserialize");
                      }
                    } finally {
                      deserializeSpan.end();
                    }
                  });
              }
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.sendCollection = (requestInfo, deserialization, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendCollection", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              const rootNode = yield this.getRootParseNode(response);
              return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getCollectionOfObjectValues", (deserializeSpan) => {
                try {
                  const result = rootNode.getCollectionOfObjectValues(deserialization);
                  span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, "object[]");
                  return result;
                } finally {
                  deserializeSpan.end();
                }
              });
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.startTracingSpan = (requestInfo, methodName, callback) => {
      var _a;
      const urlTemplate = decodeURIComponent((_a = requestInfo.urlTemplate) !== null && _a !== void 0 ? _a : "");
      const telemetryPathValue = urlTemplate.replace(/\{\?[^}]+\}/gi, "");
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan(`${methodName} - ${telemetryPathValue}`, (span) => __async(this, null, function* () {
        try {
          span.setAttribute("url.uri_template", urlTemplate);
          return yield callback(span);
        } finally {
          span.end();
        }
      }));
    };
    this.send = (requestInfo, deserializer, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "send", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              const rootNode = yield this.getRootParseNode(response);
              return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getObjectValue", (deserializeSpan) => {
                try {
                  span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, "object");
                  const result = rootNode.getObjectValue(deserializer);
                  return result;
                } finally {
                  deserializeSpan.end();
                }
              });
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.sendPrimitive = (requestInfo, responseType, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendPrimitive", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              switch (responseType) {
                case "ArrayBuffer":
                  if (!response.body) {
                    return void 0;
                  }
                  return yield response.arrayBuffer();
                case "string":
                case "number":
                case "boolean":
                case "Date":
                  const rootNode = yield this.getRootParseNode(response);
                  span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, responseType);
                  return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan(`get${responseType}Value`, (deserializeSpan) => {
                    try {
                      if (responseType === "string") {
                        return rootNode.getStringValue();
                      } else if (responseType === "number") {
                        return rootNode.getNumberValue();
                      } else if (responseType === "boolean") {
                        return rootNode.getBooleanValue();
                      } else if (responseType === "Date") {
                        return rootNode.getDateValue();
                      } else if (responseType === "Duration") {
                        return rootNode.getDurationValue();
                      } else if (responseType === "DateOnly") {
                        return rootNode.getDateOnlyValue();
                      } else if (responseType === "TimeOnly") {
                        return rootNode.getTimeOnlyValue();
                      } else {
                        throw new Error("unexpected type to deserialize");
                      }
                    } finally {
                      deserializeSpan.end();
                    }
                  });
              }
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.sendNoResponseContent = (requestInfo, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendNoResponseContent", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          }
          try {
            yield this.throwIfFailedResponse(response, errorMappings, span);
          } finally {
            yield this.purgeResponseBody(response);
          }
        } finally {
          span.end();
        }
      }));
    };
    this.sendEnum = (requestInfo, enumObject, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendEnum", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              const rootNode = yield this.getRootParseNode(response);
              return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getEnumValue", (deserializeSpan) => {
                try {
                  span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, "enum");
                  const result = rootNode.getEnumValue(enumObject);
                  return result;
                } finally {
                  deserializeSpan.end();
                }
              });
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.sendCollectionOfEnum = (requestInfo, enumObject, errorMappings) => {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      return this.startTracingSpan(requestInfo, "sendCollectionOfEnum", (span) => __async(this, null, function* () {
        try {
          const response = yield this.getHttpResponseMessage(requestInfo, span);
          const responseHandler = this.getResponseHandler(requestInfo);
          if (responseHandler) {
            span.addEvent(_FetchRequestAdapter.eventResponseHandlerInvokedKey);
            return yield responseHandler.handleResponse(response, errorMappings);
          } else {
            try {
              yield this.throwIfFailedResponse(response, errorMappings, span);
              if (this.shouldReturnUndefined(response)) return void 0;
              const rootNode = yield this.getRootParseNode(response);
              return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getCollectionOfEnumValues", (deserializeSpan) => {
                try {
                  const result = rootNode.getCollectionOfEnumValues(enumObject);
                  span.setAttribute(_FetchRequestAdapter.responseTypeAttributeKey, "enum[]");
                  return result;
                } finally {
                  deserializeSpan.end();
                }
              });
            } finally {
              yield this.purgeResponseBody(response);
            }
          }
        } finally {
          span.end();
        }
      }));
    };
    this.enableBackingStore = (backingStoreFactory) => {
      this.parseNodeFactory = enableBackingStoreForParseNodeFactory(this.parseNodeFactory);
      this.serializationWriterFactory = enableBackingStoreForSerializationWriterFactory(this.serializationWriterFactory);
      if (!this.serializationWriterFactory || !this.parseNodeFactory) throw new Error("unable to enable backing store");
      if (backingStoreFactory) {
        BackingStoreFactorySingleton.instance = backingStoreFactory;
      }
    };
    this.getRootParseNode = (response) => {
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getRootParseNode", (span) => __async(this, null, function* () {
        try {
          const payload = yield response.arrayBuffer();
          const responseContentType = this.getResponseContentType(response);
          if (!responseContentType) throw new Error("no response content type found for deserialization");
          return this.parseNodeFactory.getRootParseNode(responseContentType, payload);
        } finally {
          span.end();
        }
      }));
    };
    this.shouldReturnUndefined = (response) => {
      return response.status === 204 || !response.body;
    };
    this.purgeResponseBody = (response) => __async(this, null, function* () {
      if (!response.bodyUsed && response.body) {
        yield response.arrayBuffer();
      }
    });
    this.throwIfFailedResponse = (response, errorMappings, spanForAttributes) => {
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("throwIfFailedResponse", (span) => __async(this, null, function* () {
        var _a, _b, _c;
        try {
          if (response.ok || response.status >= 300 && response.status < 400 && !response.headers.has(_FetchRequestAdapter.locationHeaderName)) return;
          spanForAttributes.setStatus({
            code: SpanStatusCode.ERROR,
            message: "received_error_response"
          });
          const statusCode = response.status;
          const responseHeaders = {};
          response.headers.forEach((value, key) => {
            responseHeaders[key] = value.split(",");
          });
          const factory = errorMappings ? (_c = (_b = (_a = errorMappings[statusCode]) !== null && _a !== void 0 ? _a : statusCode >= 400 && statusCode < 500 ? errorMappings._4XX : void 0) !== null && _b !== void 0 ? _b : statusCode >= 500 && statusCode < 600 ? errorMappings._5XX : void 0) !== null && _c !== void 0 ? _c : errorMappings.XXX : void 0;
          if (!factory) {
            spanForAttributes.setAttribute(_FetchRequestAdapter.errorMappingFoundAttributeName, false);
            const error = new DefaultApiError("the server returned an unexpected status code and no error class is registered for this code " + statusCode);
            error.responseStatusCode = statusCode;
            error.responseHeaders = responseHeaders;
            spanForAttributes.recordException(error);
            throw error;
          }
          spanForAttributes.setAttribute(_FetchRequestAdapter.errorMappingFoundAttributeName, true);
          const rootNode = yield this.getRootParseNode(response);
          let deserializedError = trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getObjectValue", (deserializeSpan) => {
            try {
              return rootNode.getObjectValue(factory);
            } finally {
              deserializeSpan.end();
            }
          });
          spanForAttributes.setAttribute(_FetchRequestAdapter.errorBodyFoundAttributeName, !!deserializedError);
          if (!deserializedError) deserializedError = new DefaultApiError("unexpected error type" + typeof deserializedError);
          const errorObject = deserializedError;
          errorObject.responseStatusCode = statusCode;
          errorObject.responseHeaders = responseHeaders;
          spanForAttributes.recordException(errorObject);
          throw errorObject;
        } finally {
          span.end();
        }
      }));
    };
    this.getHttpResponseMessage = (requestInfo, spanForAttributes, claims) => {
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getHttpResponseMessage", (span) => __async(this, null, function* () {
        try {
          if (!requestInfo) {
            throw new Error("requestInfo cannot be null");
          }
          this.setBaseUrlForRequestInformation(requestInfo);
          const additionalContext = {};
          if (claims) {
            additionalContext.claims = claims;
          }
          yield this.authenticationProvider.authenticateRequest(requestInfo, additionalContext);
          const request = yield this.getRequestFromRequestInformation(requestInfo, spanForAttributes);
          if (this.observabilityOptions) {
            requestInfo.addRequestOptions([this.observabilityOptions]);
          }
          let response = yield this.httpClient.executeFetch(requestInfo.URL, request, requestInfo.getRequestOptions());
          response = yield this.retryCAEResponseIfRequired(requestInfo, response, spanForAttributes, claims);
          if (response) {
            const responseContentLength = response.headers.get("Content-Length");
            if (responseContentLength) {
              spanForAttributes.setAttribute("http.response.body.size", parseInt(responseContentLength, 10));
            }
            const responseContentType = response.headers.get("Content-Type");
            if (responseContentType) {
              spanForAttributes.setAttribute("http.response.header.content-type", responseContentType);
            }
            spanForAttributes.setAttribute("http.response.status_code", response.status);
          }
          return response;
        } finally {
          span.end();
        }
      }));
    };
    this.retryCAEResponseIfRequired = (requestInfo, response, spanForAttributes, claims) => __async(this, null, function* () {
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("retryCAEResponseIfRequired", (span) => __async(this, null, function* () {
        try {
          const responseClaims = this.getClaimsFromResponse(response, claims);
          if (responseClaims) {
            span.addEvent(_FetchRequestAdapter.authenticateChallengedEventKey);
            spanForAttributes.setAttribute("http.request.resend_count", 1);
            yield this.purgeResponseBody(response);
            return yield this.getHttpResponseMessage(requestInfo, spanForAttributes, responseClaims);
          }
          return response;
        } finally {
          span.end();
        }
      }));
    });
    this.getClaimsFromResponse = (response, claims) => {
      if (response.status === 401 && !claims) {
        const rawAuthenticateHeader = response.headers.get("WWW-Authenticate");
        if (rawAuthenticateHeader && /^Bearer /gi.test(rawAuthenticateHeader)) {
          const rawParameters = rawAuthenticateHeader.replace(/^Bearer /gi, "").split(",");
          for (const rawParameter of rawParameters) {
            const trimmedParameter = rawParameter.trim();
            if (/claims="[^"]+"/gi.test(trimmedParameter)) {
              return trimmedParameter.replace(/claims="([^"]+)"/gi, "$1");
            }
          }
        }
      }
      return void 0;
    };
    this.setBaseUrlForRequestInformation = (requestInfo) => {
      requestInfo.pathParameters.baseurl = this.baseUrl;
    };
    this.getRequestFromRequestInformation = (requestInfo, spanForAttributes) => {
      return trace.getTracer(this.observabilityOptions.getTracerInstrumentationName()).startActiveSpan("getRequestFromRequestInformation", (span) => __async(this, null, function* () {
        var _a, _b;
        try {
          const method = (_a = requestInfo.httpMethod) === null || _a === void 0 ? void 0 : _a.toString();
          const uri = requestInfo.URL;
          spanForAttributes.setAttribute("http.request.method", method !== null && method !== void 0 ? method : "");
          const uriContainsScheme = uri.includes("://");
          const schemeSplatUri = uri.split("://");
          if (uriContainsScheme) {
            spanForAttributes.setAttribute("server.address", schemeSplatUri[0]);
          }
          const uriWithoutScheme = uriContainsScheme ? schemeSplatUri[1] : uri;
          spanForAttributes.setAttribute("url.scheme", uriWithoutScheme.split("/")[0]);
          if (this.observabilityOptions.includeEUIIAttributes) {
            spanForAttributes.setAttribute("url.full", decodeURIComponent(uri));
          }
          const requestContentLength = requestInfo.headers.tryGetValue("Content-Length");
          if (requestContentLength) {
            spanForAttributes.setAttribute("http.response.body.size", parseInt(requestContentLength[0], 10));
          }
          const requestContentType = requestInfo.headers.tryGetValue("Content-Type");
          if (requestContentType) {
            spanForAttributes.setAttribute("http.request.header.content-type", requestContentType);
          }
          const headers = {};
          (_b = requestInfo.headers) === null || _b === void 0 ? void 0 : _b.forEach((_, key) => {
            headers[key.toString().toLocaleLowerCase()] = this.foldHeaderValue(requestInfo.headers.tryGetValue(key));
          });
          const request = {
            method,
            headers,
            body: requestInfo.content
          };
          return request;
        } finally {
          span.end();
        }
      }));
    };
    this.foldHeaderValue = (value) => {
      if (!value || value.length < 1) {
        return "";
      } else if (value.length === 1) {
        return value[0];
      } else {
        return value.reduce((acc, val) => acc + val, ",");
      }
    };
    this.convertToNativeRequest = (requestInfo) => __async(this, null, function* () {
      if (!requestInfo) {
        throw new Error("requestInfo cannot be null");
      }
      yield this.authenticationProvider.authenticateRequest(requestInfo, void 0);
      return this.startTracingSpan(requestInfo, "convertToNativeRequest", (span) => __async(this, null, function* () {
        const request = yield this.getRequestFromRequestInformation(requestInfo, span);
        return request;
      }));
    });
    if (!authenticationProvider) {
      throw new Error("authentication provider cannot be null");
    }
    if (!parseNodeFactory) {
      throw new Error("parse node factory cannot be null");
    }
    if (!serializationWriterFactory) {
      throw new Error("serialization writer factory cannot be null");
    }
    if (!httpClient) {
      throw new Error("http client cannot be null");
    }
    if (!observabilityOptions) {
      throw new Error("observability options cannot be null");
    } else {
      this.observabilityOptions = new ObservabilityOptionsImpl(observabilityOptions);
    }
  }
};
FetchRequestAdapter.responseTypeAttributeKey = "com.microsoft.kiota.response.type";
FetchRequestAdapter.eventResponseHandlerInvokedKey = "com.microsoft.kiota.response_handler_invoked";
FetchRequestAdapter.errorMappingFoundAttributeName = "com.microsoft.kiota.error.mapping_found";
FetchRequestAdapter.errorBodyFoundAttributeName = "com.microsoft.kiota.error.body_found";
FetchRequestAdapter.locationHeaderName = "Location";
FetchRequestAdapter.authenticateChallengedEventKey = "com.microsoft.kiota.authenticate_challenge_received";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/utils/headersUtil.js
var getRequestHeader = (options, key) => {
  if (options && options.headers) {
    return options.headers[key];
  }
  return void 0;
};
var setRequestHeader = (options, key, value) => {
  if (options) {
    if (!options.headers) {
      options.headers = {};
    }
    options.headers[key] = value;
  }
};
var deleteRequestHeader = (options, key) => {
  if (options) {
    if (!options.headers) {
      options.headers = {};
    }
    delete options.headers[key];
  }
};
var appendRequestHeader = (options, key, value, separator = ", ") => {
  if (options) {
    if (!options.headers) {
      options.headers = {};
    }
    if (!options.headers[key]) {
      options.headers[key] = value;
    } else {
      options.headers[key] += `${separator}${value}`;
    }
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/authorizationHandler.js
var AuthorizationHandler = class _AuthorizationHandler {
  constructor(authenticationProvider) {
    this.authenticationProvider = authenticationProvider;
    this.getClaimsFromResponse = (response, claims) => {
      if (response.status === 401 && !claims) {
        const rawAuthenticateHeader = response.headers.get("WWW-Authenticate");
        if (rawAuthenticateHeader && /^Bearer /gi.test(rawAuthenticateHeader)) {
          const rawParameters = rawAuthenticateHeader.replace(/^Bearer /gi, "").split(",");
          for (const rawParameter of rawParameters) {
            const trimmedParameter = rawParameter.trim();
            if (/claims="[^"]+"/gi.test(trimmedParameter)) {
              return trimmedParameter.replace(/claims="([^"]+)"/gi, "$1");
            }
          }
        }
      }
      return void 0;
    };
    if (!authenticationProvider) {
      throw new Error("authenticationProvider cannot be undefined");
    }
  }
  execute(url, requestInit, requestOptions) {
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("authorizationHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.authorization.enable", true);
          return this.executeInternal(url, requestInit, requestOptions, span);
        } finally {
          span.end();
        }
      });
    }
    return this.executeInternal(url, requestInit, requestOptions, void 0);
  }
  executeInternal(url, fetchRequestInit, requestOptions, span) {
    return __async(this, null, function* () {
      var _a, _b;
      if (this.authorizationIsPresent(fetchRequestInit)) {
        span === null || span === void 0 ? void 0 : span.setAttribute("com.microsoft.kiota.handler.authorization.token_present", true);
        return yield this.next.execute(url, fetchRequestInit, requestOptions);
      }
      const token = yield this.authenticateRequest(url);
      setRequestHeader(fetchRequestInit, _AuthorizationHandler.AUTHORIZATION_HEADER, `Bearer ${token}`);
      const response = yield (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, fetchRequestInit, requestOptions);
      if (!response) {
        throw new Error("Response is undefined");
      }
      if (response.status !== 401) {
        return response;
      }
      const claims = this.getClaimsFromResponse(response);
      if (!claims) {
        return response;
      }
      span === null || span === void 0 ? void 0 : span.addEvent("com.microsoft.kiota.handler.authorization.challenge_received");
      const claimsToken = yield this.authenticateRequest(url, claims);
      setRequestHeader(fetchRequestInit, _AuthorizationHandler.AUTHORIZATION_HEADER, `Bearer ${claimsToken}`);
      span === null || span === void 0 ? void 0 : span.setAttribute("http.request.resend_count", 1);
      const retryResponse = yield (_b = this.next) === null || _b === void 0 ? void 0 : _b.execute(url, fetchRequestInit, requestOptions);
      if (!retryResponse) {
        throw new Error("Response is undefined");
      }
      return retryResponse;
    });
  }
  authorizationIsPresent(request) {
    if (!request) {
      return false;
    }
    const authorizationHeader = getRequestHeader(request, _AuthorizationHandler.AUTHORIZATION_HEADER);
    return authorizationHeader !== void 0 && authorizationHeader !== null;
  }
  authenticateRequest(url, claims) {
    return __async(this, null, function* () {
      const additionalAuthenticationContext = {};
      if (claims) {
        additionalAuthenticationContext.claims = claims;
      }
      return yield this.authenticationProvider.accessTokenProvider.getAuthorizationToken(url, additionalAuthenticationContext);
    });
  }
};
AuthorizationHandler.AUTHORIZATION_HEADER = "Authorization";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/ChaosHandlerData.js
var methodStatusCode = {
  GET: [429, 500, 502, 503, 504],
  POST: [429, 500, 502, 503, 504, 507],
  PUT: [429, 500, 502, 503, 504, 507],
  PATCH: [429, 500, 502, 503, 504],
  DELETE: [429, 500, 502, 503, 504, 507]
};
var httpStatusCode = {
  100: "Continue",
  101: "Switching Protocols",
  102: "Processing",
  103: "Early Hints",
  200: "OK",
  201: "Created",
  202: "Accepted",
  203: "Non-Authoritative Information",
  204: "No Content",
  205: "Reset Content",
  206: "Partial Content",
  207: "Multi-Status",
  208: "Already Reported",
  226: "IM Used",
  300: "Multiple Choices",
  301: "Moved Permanently",
  302: "Found",
  303: "See Other",
  304: "Not Modified",
  305: "Use Proxy",
  307: "Temporary Redirect",
  308: "Permanent Redirect",
  400: "Bad Request",
  401: "Unauthorized",
  402: "Payment Required",
  403: "Forbidden",
  404: "Not Found",
  405: "Method Not Allowed",
  406: "Not Acceptable",
  407: "Proxy Authentication Required",
  408: "Request Timeout",
  409: "Conflict",
  410: "Gone",
  411: "Length Required",
  412: "Precondition Failed",
  413: "Payload Too Large",
  414: "URI Too Long",
  415: "Unsupported Media Type",
  416: "Range Not Satisfiable",
  417: "Expectation Failed",
  421: "Misdirected Request",
  422: "Unprocessable Entity",
  423: "Locked",
  424: "Failed Dependency",
  425: "Too Early",
  426: "Upgrade Required",
  428: "Precondition Required",
  429: "Too Many Requests",
  431: "Request Header Fields Too Large",
  451: "Unavailable For Legal Reasons",
  500: "Internal Server Error",
  501: "Not Implemented",
  502: "Bad Gateway",
  503: "Service Unavailable",
  504: "Gateway Timeout",
  505: "HTTP Version Not Supported",
  506: "Variant Also Negotiates",
  507: "Insufficient Storage",
  508: "Loop Detected",
  510: "Not Extended",
  511: "Network Authentication Required"
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/chaosStrategy.js
var ChaosStrategy;
(function(ChaosStrategy2) {
  ChaosStrategy2[ChaosStrategy2["MANUAL"] = 0] = "MANUAL";
  ChaosStrategy2[ChaosStrategy2["RANDOM"] = 1] = "RANDOM";
})(ChaosStrategy || (ChaosStrategy = {}));

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/chaosHandler.js
var ChaosHandler = class _ChaosHandler {
  /**
   *
   * To create an instance of ChaosHandler
   * @param [options] - The chaos handler options instance
   * @param manualMap - The Map passed by user containing url-statusCode info
   */
  constructor(options, manualMap) {
    this.options = {
      chaosStrategy: ChaosStrategy.RANDOM,
      statusMessage: "A random status message",
      chaosPercentage: 10
    };
    const chaosOptions = Object.assign(this.options, options);
    if (chaosOptions.chaosPercentage > 100 || chaosOptions.chaosPercentage < 0) {
      throw new Error("Chaos Percentage must be set to a value between 0 and 100.");
    }
    this.options = chaosOptions;
    this.manualMap = manualMap !== null && manualMap !== void 0 ? manualMap : /* @__PURE__ */ new Map();
  }
  /**
   * Fetches a random status code for the RANDOM mode from the predefined array
   * @param requestMethod - the API method for the request
   * @returns a random status code from a given set of status codes
   */
  generateRandomStatusCode(requestMethod) {
    const statusCodeArray = methodStatusCode[requestMethod];
    return statusCodeArray[Math.floor(Math.random() * statusCodeArray.length)];
  }
  /**
   * Strips out the host url and returns the relative url only
   * @param chaosHandlerOptions - The ChaosHandlerOptions object
   * @param urlMethod - the complete URL
   * @returns the string as relative URL
   */
  getRelativeURL(chaosHandlerOptions, urlMethod) {
    const baseUrl = chaosHandlerOptions.baseUrl;
    if (baseUrl === void 0) {
      return urlMethod;
    }
    return urlMethod.replace(baseUrl, "").trim();
  }
  /**
   * Gets a status code from the options or a randomly generated status code
   * @param chaosHandlerOptions - The ChaosHandlerOptions object
   * @param requestURL - the URL for the request
   * @param requestMethod - the API method for the request
   * @returns generated statusCode
   */
  getStatusCode(chaosHandlerOptions, requestURL, requestMethod) {
    if (chaosHandlerOptions.chaosStrategy === ChaosStrategy.MANUAL) {
      if (chaosHandlerOptions.statusCode !== void 0) {
        return chaosHandlerOptions.statusCode;
      } else {
        const relativeURL = this.getRelativeURL(chaosHandlerOptions, requestURL);
        const definedResponses = this.manualMap.get(relativeURL);
        if (definedResponses !== void 0) {
          const mapCode = definedResponses.get(requestMethod);
          if (mapCode !== void 0) {
            return mapCode;
          }
        } else {
          this.manualMap.forEach((value, key) => {
            var _a;
            const regexURL = new RegExp(key + "$");
            if (regexURL.test(relativeURL)) {
              const responseCode = (_a = this.manualMap.get(key)) === null || _a === void 0 ? void 0 : _a.get(requestMethod);
              if (responseCode !== void 0) {
                return responseCode;
              }
            }
          });
        }
      }
    }
    return this.generateRandomStatusCode(requestMethod);
  }
  /**
   * Generates a respondy for the chaoe response
   * @param chaosHandlerOptions - The ChaosHandlerOptions object
   * @param statusCode - the status code for the response
   * @returns the response body
   */
  createResponseBody(chaosHandlerOptions, statusCode) {
    if (chaosHandlerOptions.responseBody) {
      return chaosHandlerOptions.responseBody;
    }
    let body;
    if (statusCode >= 400) {
      const codeMessage = httpStatusCode[statusCode];
      const errMessage = chaosHandlerOptions.statusMessage;
      body = {
        error: {
          code: codeMessage,
          message: errMessage
        }
      };
    } else {
      body = {};
    }
    return body;
  }
  /**
   * Composes a new chaotic response code with the configured parameters
   * @param url The url of the request
   * @param fetchRequestInit The fetch request init object
   * @returns a response object with the configured parameters
   */
  createChaosResponse(url, fetchRequestInit) {
    var _a;
    if (fetchRequestInit.method === void 0) {
      throw new Error("Request method must be defined.");
    }
    const requestMethod = fetchRequestInit.method;
    const statusCode = this.getStatusCode(this.options, url, requestMethod);
    const responseBody = this.createResponseBody(this.options, statusCode);
    const stringBody = typeof responseBody === "string" ? responseBody : JSON.stringify(responseBody);
    return {
      url,
      body: stringBody,
      status: statusCode,
      statusText: this.options.statusMessage,
      headers: (_a = this.options.headers) !== null && _a !== void 0 ? _a : {}
    };
  }
  execute(url, requestInit, requestOptions) {
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("chaosHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.chaos.enable", true);
          return this.runChaos(url, requestInit, requestOptions);
        } finally {
          span.end();
        }
      });
    }
    return this.runChaos(url, requestInit, requestOptions);
  }
  runChaos(url, requestInit, requestOptions, span) {
    if (Math.floor(Math.random() * 100) < this.options.chaosPercentage) {
      span === null || span === void 0 ? void 0 : span.addEvent(_ChaosHandler.chaosHandlerTriggeredEventKey);
      return Promise.resolve(this.createChaosResponse(url, requestInit));
    } else {
      if (!this.next) {
        throw new Error("Please set the next middleware to continue the request");
      }
      return this.next.execute(url, requestInit, requestOptions);
    }
  }
};
ChaosHandler.chaosHandlerTriggeredEventKey = "com.microsoft.kiota.chaos_handler_triggered";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/compressionHandlerOptions.js
var CompressionHandlerOptionsKey = "CompressionHandlerOptionsKey";
var CompressionHandlerOptions = class {
  /**
   * Create a new instance of the CompressionHandlerOptions class
   * @param config the configuration to apply to the compression handler options.
   */
  constructor(config) {
    var _a;
    this._enableCompression = (_a = config === null || config === void 0 ? void 0 : config.enableCompression) !== null && _a !== void 0 ? _a : true;
  }
  /**
   * @inheritdoc
   */
  getKey() {
    return CompressionHandlerOptionsKey;
  }
  /**
   * Returns whether the compression handler is enabled or not.
   * @returns whether the compression handler is enabled or not.
   */
  get ShouldCompress() {
    return this._enableCompression;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/compressionHandler.js
var CompressionHandler = class _CompressionHandler {
  /**
   * Creates a new instance of the CompressionHandler class
   * @param handlerOptions The options for the compression handler.
   * @returns An instance of the CompressionHandler class
   */
  constructor(handlerOptions = new CompressionHandlerOptions()) {
    this.handlerOptions = handlerOptions;
    if (!handlerOptions) {
      throw new Error("handlerOptions cannot be undefined");
    }
  }
  /**
   * @inheritdoc
   */
  execute(url, requestInit, requestOptions) {
    let currentOptions = this.handlerOptions;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[CompressionHandlerOptionsKey]) {
      currentOptions = requestOptions[CompressionHandlerOptionsKey];
    }
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("compressionHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.compression.enable", currentOptions.ShouldCompress);
          return this.executeInternal(currentOptions, url, requestInit, requestOptions, span);
        } finally {
          span.end();
        }
      });
    }
    return this.executeInternal(currentOptions, url, requestInit, requestOptions);
  }
  executeInternal(options, url, requestInit, requestOptions, span) {
    return __async(this, null, function* () {
      var _a, _b, _c, _d;
      if (!options.ShouldCompress || this.contentRangeBytesIsPresent(requestInit.headers) || this.contentEncodingIsPresent(requestInit.headers) || requestInit.body === null || requestInit.body === void 0) {
        return (_b = (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, requestInit, requestOptions)) !== null && _b !== void 0 ? _b : Promise.reject(new Error("Response is undefined"));
      }
      span === null || span === void 0 ? void 0 : span.setAttribute("http.request.body.compressed", true);
      const unCompressedBody = requestInit.body;
      const unCompressedBodySize = this.getRequestBodySize(unCompressedBody);
      const compressedBody = yield this.compressRequestBody(unCompressedBody);
      setRequestHeader(requestInit, _CompressionHandler.CONTENT_ENCODING_HEADER, "gzip");
      requestInit.body = compressedBody.compressedBody;
      span === null || span === void 0 ? void 0 : span.setAttribute("http.request.body.size", compressedBody.size);
      let response = yield (_c = this.next) === null || _c === void 0 ? void 0 : _c.execute(url, requestInit, requestOptions);
      if (!response) {
        throw new Error("Response is undefined");
      }
      if (response.status === 415) {
        deleteRequestHeader(requestInit, _CompressionHandler.CONTENT_ENCODING_HEADER);
        requestInit.body = unCompressedBody;
        span === null || span === void 0 ? void 0 : span.setAttribute("http.request.body.compressed", false);
        span === null || span === void 0 ? void 0 : span.setAttribute("http.request.body.size", unCompressedBodySize);
        response = yield (_d = this.next) === null || _d === void 0 ? void 0 : _d.execute(url, requestInit, requestOptions);
      }
      return response !== void 0 && response !== null ? Promise.resolve(response) : Promise.reject(new Error("Response is undefined"));
    });
  }
  contentRangeBytesIsPresent(header) {
    var _a;
    if (!header) {
      return false;
    }
    const contentRange = getRequestHeader(header, _CompressionHandler.CONTENT_RANGE_HEADER);
    return (_a = contentRange === null || contentRange === void 0 ? void 0 : contentRange.toLowerCase().includes("bytes")) !== null && _a !== void 0 ? _a : false;
  }
  contentEncodingIsPresent(header) {
    if (!header) {
      return false;
    }
    return getRequestHeader(header, _CompressionHandler.CONTENT_ENCODING_HEADER) !== void 0;
  }
  getRequestBodySize(body) {
    if (!body) {
      return 0;
    }
    if (typeof body === "string") {
      return body.length;
    }
    if (body instanceof Blob) {
      return body.size;
    }
    if (body instanceof ArrayBuffer) {
      return body.byteLength;
    }
    if (ArrayBuffer.isView(body)) {
      return body.byteLength;
    }
    if (inNodeEnv() && Buffer.isBuffer(body)) {
      return body.byteLength;
    }
    throw new Error("Unsupported body type");
  }
  readBodyAsBytes(body) {
    if (!body) {
      return {
        stream: new ReadableStream(),
        size: 0
      };
    }
    const uint8ArrayToStream = (uint8Array) => {
      return new ReadableStream({
        start(controller) {
          controller.enqueue(uint8Array);
          controller.close();
        }
      });
    };
    if (typeof body === "string") {
      return {
        stream: uint8ArrayToStream(new TextEncoder().encode(body)),
        size: body.length
      };
    }
    if (body instanceof Blob) {
      return {
        stream: body.stream(),
        size: body.size
      };
    }
    if (body instanceof ArrayBuffer) {
      return {
        stream: uint8ArrayToStream(new Uint8Array(body)),
        size: body.byteLength
      };
    }
    if (ArrayBuffer.isView(body)) {
      return {
        stream: uint8ArrayToStream(new Uint8Array(body.buffer, body.byteOffset, body.byteLength)),
        size: body.byteLength
      };
    }
    throw new Error("Unsupported body type");
  }
  compressRequestBody(body) {
    return __async(this, null, function* () {
      const compressionData = this.readBodyAsBytes(body);
      const compressedBody = yield this.compressUsingCompressionStream(compressionData.stream);
      return {
        compressedBody: compressedBody.body,
        size: compressedBody.size
      };
    });
  }
  compressUsingCompressionStream(uint8ArrayStream) {
    return __async(this, null, function* () {
      const compressionStream = new CompressionStream("gzip");
      const compressedStream = uint8ArrayStream.pipeThrough(compressionStream);
      const reader = compressedStream.getReader();
      const compressedChunks = [];
      let totalLength = 0;
      let result = yield reader.read();
      while (!result.done) {
        const chunk = result.value;
        compressedChunks.push(chunk);
        totalLength += chunk.length;
        result = yield reader.read();
      }
      const compressedArray = new Uint8Array(totalLength);
      let offset = 0;
      for (const chunk of compressedChunks) {
        compressedArray.set(chunk, offset);
        offset += chunk.length;
      }
      return {
        body: compressedArray.buffer,
        size: compressedArray.length
      };
    });
  }
};
CompressionHandler.CONTENT_RANGE_HEADER = "Content-Range";
CompressionHandler.CONTENT_ENCODING_HEADER = "Content-Encoding";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/headersInspectionOptions.js
var HeadersInspectionOptionsKey = "HeadersInspectionOptionsKey";
var HeadersInspectionOptions = class {
  /**
   * Gets the request headers
   * @returns the request headers
   */
  getRequestHeaders() {
    return this.requestHeaders;
  }
  /**
   * Gets the response headers
   * @returns the response headers
   */
  getResponseHeaders() {
    return this.responseHeaders;
  }
  /**
   *
   * To create an instance of HeadersInspectionOptions
   * @param [options] - The headers inspection options value
   * @returns An instance of HeadersInspectionOptions
   * @example const options = new HeadersInspectionOptions({ inspectRequestHeaders: true, inspectResponseHeaders: true });
   */
  constructor(options = {}) {
    var _a, _b;
    this.requestHeaders = new Headers();
    this.responseHeaders = new Headers();
    this.inspectRequestHeaders = (_a = options.inspectRequestHeaders) !== null && _a !== void 0 ? _a : false;
    this.inspectResponseHeaders = (_b = options.inspectResponseHeaders) !== null && _b !== void 0 ? _b : false;
  }
  getKey() {
    return HeadersInspectionOptionsKey;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/headersInspectionHandler.js
var HeadersInspectionHandler = class {
  /**
   *
   * Creates new instance of HeadersInspectionHandler
   * @param _options The options for inspecting the headers
   */
  constructor(_options = new HeadersInspectionOptions()) {
    this._options = _options;
  }
  execute(url, requestInit, requestOptions) {
    let currentOptions = this._options;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[HeadersInspectionOptionsKey]) {
      currentOptions = requestOptions[HeadersInspectionOptionsKey];
    }
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("retryHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.headersInspection.enable", true);
          return this.executeInternal(url, requestInit, requestOptions, currentOptions);
        } finally {
          span.end();
        }
      });
    }
    return this.executeInternal(url, requestInit, requestOptions, currentOptions);
  }
  executeInternal(url, requestInit, requestOptions, currentOptions) {
    return __async(this, null, function* () {
      if (!this.next) {
        throw new Error("next middleware is undefined.");
      }
      if (currentOptions.inspectRequestHeaders && requestInit.headers) {
        for (const [key, value] of requestInit.headers) {
          currentOptions.getRequestHeaders().add(key, value);
        }
      }
      const response = yield this.next.execute(url, requestInit, requestOptions);
      if (currentOptions.inspectResponseHeaders && response.headers) {
        for (const [key, value] of response.headers.entries()) {
          currentOptions.getResponseHeaders().add(key, value);
        }
      }
      return response;
    });
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/parametersNameDecodingOptions.js
var ParametersNameDecodingHandlerOptionsKey = "RetryHandlerOptionKey";
var ParametersNameDecodingHandlerOptions = class {
  getKey() {
    return ParametersNameDecodingHandlerOptionsKey;
  }
  /**
   *
   * To create an instance of ParametersNameDecodingHandlerOptions
   * @param [options] - The optional parameters
   * @returns An instance of ParametersNameDecodingHandlerOptions
   * @example ParametersNameDecodingHandlerOptions({ enable: true, charactersToDecode: [".", "-", "~", "$"] });
   */
  constructor(options = {}) {
    var _a, _b;
    this.enable = (_a = options.enable) !== null && _a !== void 0 ? _a : true;
    this.charactersToDecode = (_b = options.charactersToDecode) !== null && _b !== void 0 ? _b : [".", "-", "~", "$"];
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/parametersNameDecodingHandler.js
var ParametersNameDecodingHandler = class {
  /**
   *
   * To create an instance of ParametersNameDecodingHandler
   * @param [options] - The parameters name decoding handler options value
   */
  constructor(options = new ParametersNameDecodingHandlerOptions()) {
    this.options = options;
    if (!options) {
      throw new Error("The options parameter is required.");
    }
  }
  /**
   * To execute the current middleware
   * @param url - The url to be fetched
   * @param requestInit - The request init object
   * @param requestOptions - The request options
   * @returns A Promise that resolves to nothing
   */
  execute(url, requestInit, requestOptions) {
    let currentOptions = this.options;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[ParametersNameDecodingHandlerOptionsKey]) {
      currentOptions = requestOptions[ParametersNameDecodingHandlerOptionsKey];
    }
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("parametersNameDecodingHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.parameters_name_decoding.enable", currentOptions.enable);
          return this.decodeParameters(url, requestInit, currentOptions, requestOptions);
        } finally {
          span.end();
        }
      });
    }
    return this.decodeParameters(url, requestInit, currentOptions, requestOptions);
  }
  decodeParameters(url, requestInit, currentOptions, requestOptions) {
    var _a, _b;
    let updatedUrl = url;
    if (currentOptions && currentOptions.enable && url.includes("%") && currentOptions.charactersToDecode && currentOptions.charactersToDecode.length > 0) {
      currentOptions.charactersToDecode.forEach((character) => {
        updatedUrl = updatedUrl.replace(new RegExp(`%${character.charCodeAt(0).toString(16)}`, "gi"), character);
      });
    }
    return (_b = (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(updatedUrl, requestInit, requestOptions)) !== null && _b !== void 0 ? _b : Promise.reject(new Error("The next middleware is not set."));
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/redirectHandlerOptions.js
var RedirectHandlerOptionKey = "RedirectHandlerOption";
var RedirectHandlerOptions = class _RedirectHandlerOptions {
  /**
   *
   * To create an instance of RedirectHandlerOptions
   * @param [options] - The redirect handler options instance
   * @returns An instance of RedirectHandlerOptions
   * @throws Error if maxRedirects is more than 20 or less than 0
   * @example	const options = new RedirectHandlerOptions({ maxRedirects: 5 });
   */
  constructor(options = {}) {
    var _a, _b;
    if (options.maxRedirects && options.maxRedirects > _RedirectHandlerOptions.MAX_MAX_REDIRECTS) {
      const error = new Error(`MaxRedirects should not be more than ${_RedirectHandlerOptions.MAX_MAX_REDIRECTS}`);
      error.name = "MaxLimitExceeded";
      throw error;
    }
    if (options.maxRedirects !== void 0 && options.maxRedirects < 0) {
      const error = new Error(`MaxRedirects should not be negative`);
      error.name = "MinExpectationNotMet";
      throw error;
    }
    this.maxRedirects = (_a = options.maxRedirects) !== null && _a !== void 0 ? _a : _RedirectHandlerOptions.DEFAULT_MAX_REDIRECTS;
    this.shouldRedirect = (_b = options.shouldRedirect) !== null && _b !== void 0 ? _b : _RedirectHandlerOptions.defaultShouldRetry;
  }
  getKey() {
    return RedirectHandlerOptionKey;
  }
};
RedirectHandlerOptions.DEFAULT_MAX_REDIRECTS = 5;
RedirectHandlerOptions.MAX_MAX_REDIRECTS = 20;
RedirectHandlerOptions.defaultShouldRetry = () => true;

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/redirectHandler.js
var RedirectHandler = class _RedirectHandler {
  /**
   *
   *
   * To create an instance of RedirectHandler
   * @param [options] - The redirect handler options instance
   * @returns An instance of RedirectHandler
   */
  constructor(options = new RedirectHandlerOptions()) {
    this.options = options;
    if (!options) {
      throw new Error("The options parameter is required.");
    }
  }
  /**
   *
   * To check whether the response has the redirect status code or not
   * @param response - The response object
   * @returns A boolean representing whether the response contains the redirect status code or not
   */
  isRedirect(response) {
    return _RedirectHandler.REDIRECT_STATUS_CODES.has(response.status);
  }
  /**
   *
   * To check whether the response has location header or not
   * @param response - The response object
   * @returns A boolean representing the whether the response has location header or not
   */
  hasLocationHeader(response) {
    return response.headers.has(_RedirectHandler.LOCATION_HEADER);
  }
  /**
   *
   * To get the redirect url from location header in response object
   * @param response - The response object
   * @returns A redirect url from location header
   */
  getLocationHeader(response) {
    return response.headers.get(_RedirectHandler.LOCATION_HEADER);
  }
  /**
   *
   * To check whether the given url is a relative url or not
   * @param url - The url string value
   * @returns A boolean representing whether the given url is a relative url or not
   */
  isRelativeURL(url) {
    return !url.includes("://");
  }
  /**
   *
   * To check whether the authorization header in the request should be dropped for consequent redirected requests
   * @param requestUrl - The request url value
   * @param redirectUrl - The redirect url value
   * @returns A boolean representing whether the authorization header in the request should be dropped for consequent redirected requests
   */
  shouldDropAuthorizationHeader(requestUrl, redirectUrl) {
    const schemeHostRegex = /^[A-Za-z].+?:\/\/.+?(?=\/|$)/;
    const requestMatches = schemeHostRegex.exec(requestUrl);
    let requestAuthority;
    let redirectAuthority;
    if (requestMatches !== null) {
      requestAuthority = requestMatches[0];
    }
    const redirectMatches = schemeHostRegex.exec(redirectUrl);
    if (redirectMatches !== null) {
      redirectAuthority = redirectMatches[0];
    }
    return typeof requestAuthority !== "undefined" && typeof redirectAuthority !== "undefined" && requestAuthority !== redirectAuthority;
  }
  /**
   * To execute the next middleware and to handle in case of redirect response returned by the server
   * @param url - The url string value
   * @param fetchRequestInit - The Fetch RequestInit object
   * @param redirectCount - The redirect count value
   * @param currentOptions - The redirect handler options instance
   * @param requestOptions - The request options
   * @param tracerName - The name to use for the tracer
   * @returns A promise that resolves to nothing
   */
  executeWithRedirect(url, fetchRequestInit, redirectCount, currentOptions, requestOptions, tracerName) {
    return __async(this, null, function* () {
      var _a;
      const response = yield (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, fetchRequestInit, requestOptions);
      if (!response) {
        throw new Error("Response is undefined");
      }
      if (redirectCount < currentOptions.maxRedirects && this.isRedirect(response) && this.hasLocationHeader(response) && currentOptions.shouldRedirect(response)) {
        ++redirectCount;
        if (response.status === _RedirectHandler.STATUS_CODE_SEE_OTHER) {
          fetchRequestInit.method = HttpMethod.GET;
          delete fetchRequestInit.body;
        } else {
          const redirectUrl = this.getLocationHeader(response);
          if (redirectUrl) {
            if (fetchRequestInit.headers && !this.isRelativeURL(redirectUrl) && this.shouldDropAuthorizationHeader(url, redirectUrl)) {
              delete fetchRequestInit.headers[_RedirectHandler.AUTHORIZATION_HEADER];
            }
            url = redirectUrl;
          }
        }
        if (tracerName) {
          return trace.getTracer(tracerName).startActiveSpan(`redirectHandler - redirect ${redirectCount}`, (span) => {
            try {
              span.setAttribute("com.microsoft.kiota.handler.redirect.count", redirectCount);
              span.setAttribute("http.response.status_code", response.status);
              return this.executeWithRedirect(url, fetchRequestInit, redirectCount, currentOptions, requestOptions);
            } finally {
              span.end();
            }
          });
        }
        return yield this.executeWithRedirect(url, fetchRequestInit, redirectCount, currentOptions, requestOptions);
      } else {
        return response;
      }
    });
  }
  /**
   * Executes the request and returns a promise resolving the response.
   * @param url - The url for the request
   * @param requestInit - The Fetch RequestInit object.
   * @param requestOptions - The request options.
   * @returns A Promise that resolves to the response.
   */
  execute(url, requestInit, requestOptions) {
    const redirectCount = 0;
    let currentOptions = this.options;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[RedirectHandlerOptionKey]) {
      currentOptions = requestOptions[RedirectHandlerOptionKey];
    }
    requestInit.redirect = _RedirectHandler.MANUAL_REDIRECT;
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("redirectHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.redirect.enable", true);
          return this.executeWithRedirect(url, requestInit, redirectCount, currentOptions, requestOptions, obsOptions.getTracerInstrumentationName());
        } finally {
          span.end();
        }
      });
    }
    return this.executeWithRedirect(url, requestInit, redirectCount, currentOptions, requestOptions);
  }
};
RedirectHandler.REDIRECT_STATUS_CODES = /* @__PURE__ */ new Set([
  301,
  // Moved Permanently
  302,
  // Found
  303,
  // See Other
  307,
  // Temporary Permanently
  308
  // Moved Permanently
]);
RedirectHandler.STATUS_CODE_SEE_OTHER = 303;
RedirectHandler.LOCATION_HEADER = "Location";
RedirectHandler.AUTHORIZATION_HEADER = "Authorization";
RedirectHandler.MANUAL_REDIRECT = "manual";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/retryHandlerOptions.js
var RetryHandlerOptionKey = "RetryHandlerOptionKey";
var RetryHandlerOptions = class _RetryHandlerOptions {
  /**
   *
   * To create an instance of RetryHandlerOptions
   * @param options - The RetryHandlerOptionsParams object
   * @returns An instance of RetryHandlerOptions
   * @example	const options = new RetryHandlerOptions({ maxRetries: 4 });
   */
  constructor(options = {}) {
    var _a, _b, _c;
    if (options.delay !== void 0 && options.delay > _RetryHandlerOptions.MAX_DELAY) {
      throw this.createError(`Delay should not be more than ${_RetryHandlerOptions.MAX_DELAY}`, "MaxLimitExceeded");
    }
    if (options.maxRetries !== void 0 && options.maxRetries > _RetryHandlerOptions.MAX_MAX_RETRIES) {
      throw this.createError(`MaxRetries should not be more than ${_RetryHandlerOptions.MAX_MAX_RETRIES}`, "MaxLimitExceeded");
    }
    if (options.delay !== void 0 && options.delay < 0) {
      throw this.createError(`Delay should not be negative`, "MinExpectationNotMet");
    }
    if (options.maxRetries !== void 0 && options.maxRetries < 0) {
      throw this.createError(`MaxRetries should not be negative`, "MinExpectationNotMet");
    }
    this.delay = Math.min((_a = options.delay) !== null && _a !== void 0 ? _a : _RetryHandlerOptions.DEFAULT_DELAY, _RetryHandlerOptions.MAX_DELAY);
    this.maxRetries = Math.min((_b = options.maxRetries) !== null && _b !== void 0 ? _b : _RetryHandlerOptions.DEFAULT_MAX_RETRIES, _RetryHandlerOptions.MAX_MAX_RETRIES);
    this.shouldRetry = (_c = options.shouldRetry) !== null && _c !== void 0 ? _c : _RetryHandlerOptions.defaultShouldRetry;
  }
  /**
   *
   * Creates an error object with a message and name
   * @param message - The error message
   * @param name - The error name
   * @returns An error object
   */
  createError(message, name) {
    const error = new Error(message);
    error.name = name;
    return error;
  }
  /**
   *
   * To get the maximum delay
   * @returns A maximum delay
   */
  getMaxDelay() {
    return _RetryHandlerOptions.MAX_DELAY;
  }
  getKey() {
    return RetryHandlerOptionKey;
  }
};
RetryHandlerOptions.DEFAULT_DELAY = 3;
RetryHandlerOptions.DEFAULT_MAX_RETRIES = 3;
RetryHandlerOptions.MAX_DELAY = 180;
RetryHandlerOptions.MAX_MAX_RETRIES = 10;
RetryHandlerOptions.defaultShouldRetry = () => true;

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/retryHandler.js
var RetryHandler = class _RetryHandler {
  /**
   *
   * To create an instance of RetryHandler
   * @param [options] - The retry handler options value
   * @returns An instance of RetryHandler
   */
  constructor(options = new RetryHandlerOptions()) {
    this.options = options;
    if (!options) {
      throw new Error("The options parameter is required.");
    }
  }
  /**
   *
   *
   * To check whether the response has the retry status code
   * @param response - The response object
   * @returns Whether the response has retry status code or not
   */
  isRetry(response) {
    return _RetryHandler.RETRY_STATUS_CODES.has(response.status);
  }
  /**
   *
   * To check whether the payload is buffered or not
   * @param options - The options of a request
   * @returns Whether the payload is buffered or not
   */
  isBuffered(options) {
    var _a;
    const method = options.method;
    const isPutPatchOrPost = method === HttpMethod.PUT || method === HttpMethod.PATCH || method === HttpMethod.POST;
    if (isPutPatchOrPost) {
      const isStream = ((_a = getRequestHeader(options, "content-type")) === null || _a === void 0 ? void 0 : _a.toLowerCase()) === "application/octet-stream";
      if (isStream) {
        return false;
      }
    }
    return true;
  }
  /**
   *
   * To get the delay for a retry
   * @param response - The response object
   * @param retryAttempts - The current attempt count
   * @param delay - The delay value in seconds
   * @returns A delay for a retry
   */
  getDelay(response, retryAttempts, delay) {
    const getRandomness = () => Number(Math.random().toFixed(3));
    const retryAfter = response.headers !== void 0 ? response.headers.get(_RetryHandler.RETRY_AFTER_HEADER) : null;
    let newDelay;
    if (retryAfter !== null) {
      if (Number.isNaN(Number(retryAfter))) {
        newDelay = Math.round((new Date(retryAfter).getTime() - Date.now()) / 1e3);
      } else {
        newDelay = Number(retryAfter);
      }
    } else {
      newDelay = retryAttempts >= 2 ? this.getExponentialBackOffTime(retryAttempts) + delay + getRandomness() : delay + getRandomness();
    }
    return Math.min(newDelay, this.options.getMaxDelay() + getRandomness());
  }
  /**
   *
   * To get an exponential back off value
   * @param attempts - The current attempt count
   * @returns An exponential back off value
   */
  getExponentialBackOffTime(attempts) {
    return Math.round(1 / 2 * (2 ** attempts - 1));
  }
  /**
   * To add delay for the execution
   * @param delaySeconds - The delay value in seconds
   * @returns A Promise that resolves to nothing
   */
  sleep(delaySeconds) {
    return __async(this, null, function* () {
      const delayMilliseconds = delaySeconds * 1e3;
      return new Promise((resolve) => setTimeout(resolve, delayMilliseconds));
    });
  }
  /**
   * To execute the middleware with retries
   * @param url - The request url
   * @param fetchRequestInit - The request options
   * @param retryAttempts - The current attempt count
   * @param currentOptions - The current request options for the retry handler.
   * @param requestOptions - The retry middleware options instance
   * @param tracerName - The name to use for the tracer
   * @returns A Promise that resolves to nothing
   */
  executeWithRetry(url, fetchRequestInit, retryAttempts, currentOptions, requestOptions, tracerName) {
    return __async(this, null, function* () {
      var _a;
      const response = yield (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, fetchRequestInit, requestOptions);
      if (!response) {
        throw new Error("Response is undefined");
      }
      if (retryAttempts < currentOptions.maxRetries && this.isRetry(response) && this.isBuffered(fetchRequestInit) && currentOptions.shouldRetry(currentOptions.delay, retryAttempts, url, fetchRequestInit, response)) {
        ++retryAttempts;
        setRequestHeader(fetchRequestInit, _RetryHandler.RETRY_ATTEMPT_HEADER, retryAttempts.toString());
        let delay = null;
        if (response) {
          delay = this.getDelay(response, retryAttempts, currentOptions.delay);
          yield this.sleep(delay);
        }
        if (tracerName) {
          return yield trace.getTracer(tracerName).startActiveSpan(`retryHandler - attempt ${retryAttempts}`, (span) => {
            try {
              span.setAttribute("http.request.resend_count", retryAttempts);
              if (delay) {
                span.setAttribute("http.request.resend_delay", delay);
              }
              span.setAttribute("http.response.status_code", response.status);
              return this.executeWithRetry(url, fetchRequestInit, retryAttempts, currentOptions, requestOptions);
            } finally {
              span.end();
            }
          });
        }
        return yield this.executeWithRetry(url, fetchRequestInit, retryAttempts, currentOptions, requestOptions);
      } else {
        return response;
      }
    });
  }
  /**
   * To execute the current middleware
   * @param url - The request url
   * @param requestInit - The request options
   * @param requestOptions - The request options
   * @returns A Promise that resolves to nothing
   */
  execute(url, requestInit, requestOptions) {
    const retryAttempts = 0;
    let currentOptions = this.options;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[RetryHandlerOptionKey]) {
      currentOptions = requestOptions[RetryHandlerOptionKey];
    }
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("retryHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.retry.enable", true);
          return this.executeWithRetry(url, requestInit, retryAttempts, currentOptions, requestOptions, obsOptions.getTracerInstrumentationName());
        } finally {
          span.end();
        }
      });
    }
    return this.executeWithRetry(url, requestInit, retryAttempts, currentOptions, requestOptions);
  }
};
RetryHandler.RETRY_STATUS_CODES = /* @__PURE__ */ new Set([
  429,
  // Too many requests
  503,
  // Service unavailable
  504
  // Gateway timeout
]);
RetryHandler.RETRY_ATTEMPT_HEADER = "Retry-Attempt";
RetryHandler.RETRY_AFTER_HEADER = "Retry-After";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/version.js
var libraryVersion = "1.0.0-preview.24";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/userAgentHandlerOptions.js
var UserAgentHandlerOptionsKey = "UserAgentHandlerOptionKey";
var UserAgentHandlerOptions = class {
  getKey() {
    return UserAgentHandlerOptionsKey;
  }
  /**
   *
   * To create an instance of UserAgentHandlerOptions
   * @param [options] - The options for the UserAgentHandler
   * @example	const options = new UserAgentHandlerOptions({ enable: false });
   */
  constructor(options = {}) {
    var _a, _b, _c;
    this.enable = (_a = options.enable) !== null && _a !== void 0 ? _a : true;
    this.productName = (_b = options.productName) !== null && _b !== void 0 ? _b : "kiota-typescript";
    this.productVersion = (_c = options.productVersion) !== null && _c !== void 0 ? _c : libraryVersion;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/userAgentHandler.js
var USER_AGENT_HEADER_KEY = "User-Agent";
var UserAgentHandler = class {
  /**
   * To create an instance of UserAgentHandler
   * @param _options - The options for the middleware
   */
  constructor(_options = new UserAgentHandlerOptions()) {
    this._options = _options;
  }
  /** @inheritdoc */
  execute(url, requestInit, requestOptions) {
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("userAgentHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.useragent.enable", true);
          return this.addValue(url, requestInit, requestOptions);
        } finally {
          span.end();
        }
      });
    } else {
      return this.addValue(url, requestInit, requestOptions);
    }
  }
  addValue(url, requestInit, requestOptions) {
    return __async(this, null, function* () {
      var _a;
      let currentOptions = this._options;
      if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[UserAgentHandlerOptionsKey]) {
        currentOptions = requestOptions[UserAgentHandlerOptionsKey];
      }
      if (currentOptions.enable) {
        const additionalValue = `${currentOptions.productName}/${currentOptions.productVersion}`;
        const currentValue = getRequestHeader(requestInit, USER_AGENT_HEADER_KEY);
        if (!(currentValue === null || currentValue === void 0 ? void 0 : currentValue.includes(additionalValue))) {
          appendRequestHeader(requestInit, USER_AGENT_HEADER_KEY, additionalValue, " ");
        }
      }
      const response = yield (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, requestInit, requestOptions);
      if (!response) throw new Error("No response returned by the next middleware");
      return response;
    });
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/urlReplaceHandlerOptions.js
var UrlReplaceHandlerOptionsKey = "UrlReplaceHandlerOptionsKey";
var UrlReplaceHandlerOptions = class {
  /**
   * Create a new instance of the UrlReplaceHandlerOptions class
   * @param config the configuration to apply to the url replace handler options.
   */
  constructor(config) {
    var _a, _b;
    if (config) {
      this._urlReplacements = (_a = config.urlReplacements) !== null && _a !== void 0 ? _a : {};
      this._enabled = (_b = config.enabled) !== null && _b !== void 0 ? _b : true;
    } else {
      this._urlReplacements = {};
      this._enabled = true;
    }
  }
  /**
   * @inheritdoc
   */
  getKey() {
    return UrlReplaceHandlerOptionsKey;
  }
  /**
   * Returns whether the url replace handler is enabled or not.
   * @returns whether the url replace handler is enabled or not.
   */
  get enabled() {
    return this._enabled;
  }
  /**
   * Returns the url replacements combinations.
   * @returns the url replacements combinations.
   */
  get urlReplacements() {
    return this._urlReplacements;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/urlReplaceHandler.js
var UrlReplaceHandler = class {
  /**
   *
   * Creates a new instance of the UrlReplaceHandler class
   * @param handlerOptions The options for the url replace handler.
   * @returns An instance of the UrlReplaceHandler class
   */
  constructor(handlerOptions = new UrlReplaceHandlerOptions()) {
    this.handlerOptions = handlerOptions;
    if (!handlerOptions) {
      throw new Error("handlerOptions cannot be undefined");
    }
  }
  /**
   * @inheritdoc
   */
  execute(url, requestInit, requestOptions) {
    let currentOptions = this.handlerOptions;
    if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[UrlReplaceHandlerOptionsKey]) {
      currentOptions = requestOptions[UrlReplaceHandlerOptionsKey];
    }
    const obsOptions = getObservabilityOptionsFromRequest(requestOptions);
    if (obsOptions) {
      return trace.getTracer(obsOptions.getTracerInstrumentationName()).startActiveSpan("urlReplaceHandler - execute", (span) => {
        try {
          span.setAttribute("com.microsoft.kiota.handler.urlReplace.enable", currentOptions.enabled);
          return this.replaceTokensInUrl(currentOptions, url, requestInit, requestOptions);
        } finally {
          span.end();
        }
      });
    }
    return this.replaceTokensInUrl(currentOptions, url, requestInit, requestOptions);
  }
  replaceTokensInUrl(options, url, requestInit, requestOptions) {
    var _a;
    if (options.enabled) {
      Object.keys(options.urlReplacements).forEach((replacementKey) => {
        url = url.replace(replacementKey, options.urlReplacements[replacementKey]);
      });
    }
    const response = (_a = this.next) === null || _a === void 0 ? void 0 : _a.execute(url, requestInit, requestOptions);
    if (!response) {
      throw new Error("Response is undefined");
    }
    return response;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/options/chaosHandlerOptions.js
var ChaosHandlerOptionsKey = "ChaosHandlerOptionsKey";

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/telemetryHandler.js
var TelemetryHandlerOptionsKey = "TelemetryHandlerOptionsKey";
var TelemetryHandler = class {
  constructor(telemetryHandlerOptions) {
    this.telemetryHandlerOptions = telemetryHandlerOptions;
  }
  execute(url, requestInit, requestOptions) {
    var _a;
    if ((_a = this.telemetryHandlerOptions) === null || _a === void 0 ? void 0 : _a.telemetryConfigurator) {
      this.telemetryHandlerOptions.telemetryConfigurator(url, requestInit, requestOptions, this.telemetryHandlerOptions.telemetryInformation);
    } else if (requestOptions === null || requestOptions === void 0 ? void 0 : requestOptions[TelemetryHandlerOptionsKey]) {
      requestOptions[TelemetryHandlerOptionsKey].telemetryConfigurator(url, requestInit, requestOptions);
    }
    if (!this.next) {
      throw new Error("Please set the next middleware to continue the request");
    }
    return this.next.execute(url, requestInit, requestOptions);
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/middlewares/middlewareFactory.js
var MiddlewareFactory = class _MiddlewareFactory {
  /**
   * @param customFetch - The custom fetch implementation
   * Returns the default middleware chain an array with the  middleware handlers
   * @returns an array of the middleware handlers of the default middleware chain
   */
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  static getDefaultMiddlewares(customFetch = (...args) => fetch(...args)) {
    return [new RetryHandler(), new RedirectHandler(), new ParametersNameDecodingHandler(), new UserAgentHandler(), new HeadersInspectionHandler(), new UrlReplaceHandler(), new CustomFetchHandler(customFetch)];
  }
  /**
   * @param customFetch - The custom fetch implementation
   * Returns the default middleware chain + performance middleware
   * @returns an array of the middleware handlers of the default + performance middleware chain
   */
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  static getPerformanceMiddlewares(customFetch = (...args) => fetch(...args)) {
    const middlewares = _MiddlewareFactory.getDefaultMiddlewares(customFetch);
    middlewares.splice(middlewares.length - 3, 0, new CompressionHandler());
    return middlewares;
  }
};

// node_modules/@microsoft/kiota-http-fetchlibrary/dist/es/src/kiotaClientFactory.js
var KiotaClientFactory = class {
  /**
   * Returns an instance of HttpClient with the provided middlewares and custom fetch implementation both parameters are optional.
   * if not provided, the default fetch implementation and middlewares will be used.
   * @param customFetch - a Fetch API implementation
   * @param middlewares - an aray of Middleware handlers
   * If middlewares param is undefined, the httpClient instance will use the default array of middlewares.
   * Set middlewares to `null` if you do not wish to use middlewares.
   * If custom fetch is undefined, the httpClient instance uses the `DefaultFetchHandler`
   * @param authenticationProvider - an optional instance of BaseBearerTokenAuthenticationProvider to be used for authentication
   * @returns a HttpClient instance
   * @example
   * ```Typescript
   * // Example usage of KiotaClientFactory.create method with both customFetch and middlewares parameters provided
   *  KiotaClientFactory.create(customFetch, [middleware1, middleware2]);
   * ```
   * @example
   * ```Typescript
   * // Example usage of KiotaClientFactory.create method with only customFetch parameter provided
   * KiotaClientFactory.create(customFetch);
   * ```
   * @example
   * ```Typescript
   * // Example usage of KiotaClientFactory.create method with only middlewares parameter provided
   * KiotaClientFactory.create(undefined, [middleware1, middleware2]);
   * ```
   * @example
   * ```Typescript
   * // Example usage of KiotaClientFactory.create method with no parameters provided
   * KiotaClientFactory.create();
   * ```
   */
  // eslint-disable-next-line @typescript-eslint/no-unsafe-return
  static create(customFetch = (...args) => fetch(...args), middlewares, authenticationProvider) {
    const middleware = middlewares || MiddlewareFactory.getDefaultMiddlewares(customFetch);
    if (authenticationProvider) {
      middleware.unshift(new AuthorizationHandler(authenticationProvider));
    }
    return new HttpClient(customFetch, ...middleware);
  }
};
export {
  AuthorizationHandler,
  ChaosHandler,
  ChaosHandlerOptionsKey,
  ChaosStrategy,
  CompressionHandler,
  CompressionHandlerOptions,
  CompressionHandlerOptionsKey,
  CustomFetchHandler,
  FetchRequestAdapter,
  HeadersInspectionHandler,
  HeadersInspectionOptions,
  HeadersInspectionOptionsKey,
  HttpClient,
  KiotaClientFactory,
  MiddlewareFactory,
  ObservabilityOptionKey,
  ObservabilityOptionsImpl,
  ParametersNameDecodingHandler,
  ParametersNameDecodingHandlerOptions,
  ParametersNameDecodingHandlerOptionsKey,
  RedirectHandler,
  RedirectHandlerOptionKey,
  RedirectHandlerOptions,
  RetryHandler,
  RetryHandlerOptionKey,
  RetryHandlerOptions,
  TelemetryHandler,
  TelemetryHandlerOptionsKey,
  UrlReplaceHandler,
  UrlReplaceHandlerOptions,
  UrlReplaceHandlerOptionsKey,
  UserAgentHandler,
  UserAgentHandlerOptions,
  UserAgentHandlerOptionsKey,
  appendRequestHeader,
  deleteRequestHeader,
  getObservabilityOptionsFromRequest,
  getRequestHeader,
  setRequestHeader
};
//# sourceMappingURL=@microsoft_kiota-http-fetchlibrary.js.map
