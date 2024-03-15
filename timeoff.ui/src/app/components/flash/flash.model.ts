export function isSuccess(message: string) {
    return {
        isError: false,
        messages: [message],
    } as FlashModel;
}

export function isError(message: string) {
    return {
        isError: true,
        messages: [message],
    } as FlashModel;
}

export function hasErrors(messages: string[]) {
    return {
        isError: true,
        messages: messages,
    } as FlashModel;
}

export class FlashModel {
    public isError: boolean = false;

    public messages: string[] = [];
}
