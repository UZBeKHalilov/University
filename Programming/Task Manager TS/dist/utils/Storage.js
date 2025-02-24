"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Storage = void 0;
class Storage {
    constructor(key) {
        this.key = key;
    }
    // Save data to local storage 
    save(data) {
        localStorage.setItem(this.key, JSON.stringify(data));
    }
    // Load data from local storage 
    load() {
        const data = localStorage.getItem(this.key);
        return data ? JSON.parse(data) : [];
    }
}
exports.Storage = Storage;
