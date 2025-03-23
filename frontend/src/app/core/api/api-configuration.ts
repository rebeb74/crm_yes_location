import { Injectable } from '@angular/core';
import { AnonymousAuthenticationProvider } from '@microsoft/kiota-abstractions';
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary';
import { environment } from '../../../environments/environment';
import { createApiClient, type ApiClient } from './kiota-generated/apiClient';

@Injectable({
  providedIn: 'root',
})
export class ApiConfiguration {
  private baseUrl = environment.apiUrl || 'http://localhost:5000';

  createApiClient(): ApiClient {
    const authProvider = new AnonymousAuthenticationProvider();
    const adapter = new FetchRequestAdapter(authProvider);
    adapter.baseUrl = this.baseUrl;

    return createApiClient(adapter);
  }
}
