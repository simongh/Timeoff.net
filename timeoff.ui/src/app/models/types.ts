export type dateString = string;

export enum datePart {
    morning = 'morning',
    afternoon = 'afternoon',
    wholeDay = 'wholeDay',
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