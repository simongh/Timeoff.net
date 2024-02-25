import { Country } from "../services/country.model";

export interface RegisterOptions{
    countries: Country[];
    timezones: {
        name: string;
        description: string;
    }[];
}