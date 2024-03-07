/* eslint-disable @typescript-eslint/no-explicit-any */
export default function debounce(func: (...args: any[]) => void, delay = 1000) {
    let timeout: NodeJS.Timeout;

    return function (...args: any[]): void {
        clearTimeout(timeout);

        timeout = setTimeout(() => {
            func(...args);
        }, delay);
    };
}
