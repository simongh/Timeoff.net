import { HttpClient, HttpParams } from "@angular/common/http";
import { TeamModel } from "../../models/team.model";
import { UserModel } from "./user.model";
import { Injectable } from "@angular/core";

@Injectable()
export class UsersService {
    constructor(private readonly client: HttpClient) {}

    public getTeams() {
        return this.client.get<TeamModel[]>('/api/users/teams');
    }

    public getUsers(team: number | null) {
        const options = team ? {
            params:  new HttpParams().set('team',team)
        } : {};
        

        return this.client.get<UserModel[]>('/api/users', options);
    }
}