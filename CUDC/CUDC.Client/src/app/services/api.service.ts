import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

export abstract class ApiService {
    constructor(protected httpClient: HttpClient) {
    }

    protected url(path: string): string {
        return environment.serviceBaseUrl + path;
    }
}
