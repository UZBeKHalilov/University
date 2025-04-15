import { LocalStorage } from 'node-localstorage';

export class DataStorage<T> {
    private key: string;
    private storage: Storage | LocalStorage;

    constructor(key: string) {
        this.key = key;

        // Check if running in a browser or Node.js
        if (typeof window !== 'undefined' && window.localStorage) {
            this.storage = window.localStorage;
        } else {
            // Use node-localstorage in Node.js
            const path = require('path');
            const localStoragePath = path.resolve(__dirname, '../../localStorage');
            this.storage = new LocalStorage(localStoragePath);
        }
    }

    // Save data to storage
    save(data: T[]): void {
        this.storage.setItem(this.key, JSON.stringify(data));
    }

    // Load data from storage
    load(): T[] {
        const data = this.storage.getItem(this.key);
        return data ? JSON.parse(data) : [];
    }
}