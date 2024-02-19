export interface RegisterOptions{
    countries: {
        code: string;
        name: string;
    }[];
    timezones: {
        name: string;
        description: string;
    }[];
}