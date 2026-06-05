import { Task, TaskModel, TaskPriority, TaskStatus } from '../models/Task';

export class TaskService {
    private static STORAGE_KEY = 'ts-task-manager';
    private tasks: Task[] = [];
    private listeners: (() => void)[] = [];

    constructor() {
        this.loadTasks();
    }

    private loadTasks(): void {
        const tasksJson = localStorage.getItem(TaskService.STORAGE_KEY);
        if (tasksJson) {
            try {
                const parsedTasks = JSON.parse(tasksJson);
                this.tasks = parsedTasks.map((task: any) => {
                    return new TaskModel(
                        task.title,
                        task.description,
                        task.priority,
                        task.status,
                        task.id,
                        task.createdAt ? new Date(task.createdAt) : undefined,
                        task.completedAt ? new Date(task.completedAt) : undefined
                    );
                });
            } catch (error) {
                console.error('Error loading tasks from localStorage:', error);
                this.tasks = [];
            }
        }
    }

    private saveTasks(): void {
        localStorage.setItem(TaskService.STORAGE_KEY, JSON.stringify(this.tasks));
        this.notifyListeners();
    }

    getAllTasks(): Task[] {
        return [...this.tasks];
    }

    getTaskById(id: string): Task | undefined {
        return this.tasks.find(task => task.id === id);
    }

    addTask(title: string, description: string, priority: TaskPriority = TaskPriority.Medium): Task {
        const newTask = new TaskModel(title, description, priority);
        this.tasks.push(newTask);
        this.saveTasks();
        return newTask;
    }

    updateTask(id: string, updates: Partial<Omit<Task, 'id' | 'createdAt'>>): Task | null {
        const taskIndex = this.tasks.findIndex(task => task.id === id);
        if (taskIndex === -1) return null;

        const updatedTask = {
            ...this.tasks[taskIndex],
            ...updates
        };

        this.tasks[taskIndex] = updatedTask as TaskModel;
        this.saveTasks();
        return updatedTask as Task;
    }

    deleteTask(id: string): boolean {
        const initialLength = this.tasks.length;
        this.tasks = this.tasks.filter(task => task.id !== id);
        
        if (initialLength !== this.tasks.length) {
            this.saveTasks();
            return true;
        }
        return false;
    }

    markTaskAsCompleted(id: string): Task | null {
        const task = this.getTaskById(id);
        if (!task) return null;

        const updatedTask = this.updateTask(id, { 
            status: TaskStatus.Completed,
            completedAt: new Date()
        });
        return updatedTask;
    }

    updateTaskStatus(id: string, status: TaskStatus): Task | null {
        const task = this.getTaskById(id);
        if (!task) return null;

        const updates: Partial<Task> = { status };
        
        if (status === TaskStatus.Completed) {
            updates.completedAt = new Date();
        } else if (task.completedAt) {
            updates.completedAt = undefined;
        }

        return this.updateTask(id, updates);
    }

    addChangeListener(listener: () => void): void {
        this.listeners.push(listener);
    }

    removeChangeListener(listener: () => void): void {
        this.listeners = this.listeners.filter(l => l !== listener);
    }

    private notifyListeners(): void {
        this.listeners.forEach(listener => listener());
    }
}