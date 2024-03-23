import { Country } from '../services/company/country.model';

export interface RegisterOptions {
    countries: Country[];
    timezones: {
        id: string;
        name: string;
    }[];
}
