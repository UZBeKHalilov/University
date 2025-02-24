import { Repository } from 'typeorm';
import { AppDataSource } from './data-source'; // DataSource faylingizdan import

export class Storage<T> {
    private repository: Repository<T>;

    constructor(entity: any) {
        this.repository = AppDataSource.getRepository(entity);
    }

    // Ma'lumotlarni saqlash
    async save(data: T[]): Promise<void> {
        await this.repository.save(data);
    }

    // Ma'lumotlarni yuklash
    async load(): Promise<T[]> {
        return await this.repository.find();
    }
}