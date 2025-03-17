import { DataSource } from 'typeorm';
import { Task } from './Tasks'; // Entity faylingiz

// DataSource obyektini yaratish
const AppDataSource = new DataSource({
    type: 'mssql',
    host: 'localhost',
    port: 1433,
    username: 'sa',
    password: 'KHalilov8841',
    database: 'TaskManagerDB',
    entities: [Task], // Entity sinflarni ro'yxatga olish
    synchronize: true, // Development uchun jadvalni avto-yaratish
    options: {
        encrypt: true,
        trustServerCertificate: true // Lokal server uchun
    }
});

// Ulanishni boshlash
async function initializeDatabase() {
    try {
        await AppDataSource.initialize();
        console.log('Database connected');
    } catch (error) {
        console.error('Database connection error:', error);
    }
}

initializeDatabase();