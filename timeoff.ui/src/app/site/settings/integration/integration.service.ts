import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IntegrationModel } from "./integration.model";

const baseAddress = '/api/settings/integration';

@Injectable()
export class IntegrationService {
    constructor(
        private readonly client: HttpClient
    ) {}

    public get() {
        return this.client.get<IntegrationModel>(baseAddress);
    }
    public update(enabled: boolean) {
        return this.updateApi(enabled,false);
    }

    public regenerate() {
        return this.updateApi(true,true);
    }

    private updateApi(enabled: boolean, regenerate: boolean) {
        return this.client.put<IntegrationModel>(baseAddress, {
            enabled: enabled,
            regenerate: regenerate,
        });
    }
}