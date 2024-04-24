export type dateString = string;

export enum datePart {
    morning = 'Morning',
    afternoon = 'Afternoon',
    wholeDay = 'All',
}

export function datePartList() {
    return [
        {
            id: datePart.wholeDay,
            name: "All Day"
        },
        {
            id: datePart.morning,
            name: "Morning",
        },
        {
            id: datePart.afternoon,
            name: "Afternoon",
        }
    ];
}