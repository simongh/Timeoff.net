import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { TeamModel } from "./team.model";
import { of } from "rxjs";

@Injectable()
export class TeamsService {
    constructor(
        private readonly client: HttpClient) {}

    public getTeams(){
        return of([]);
        //return this.client.get<[TeamModel]>('/api/teams');
    }
}