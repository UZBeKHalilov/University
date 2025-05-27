export enum TaskPriority {
    Low = "low",
    Medium = "medium",
    High = "high"
}

export enum TaskStatus {
    Pending = "pending",
    InProgress = "in-progress",
    Completed = "completed"
}

export interface Task {
    id: string;
    title: string;
    description: string;
    priority: TaskPriority;
    status: TaskStatus;
    createdAt: Date;
    completedAt?: Date;
}

export class TaskModel implements Task {
    id: string;
    title: string;
    description: string;
    priority: TaskPriority;
    status: TaskStatus;
    createdAt: Date;
    completedAt?: Date;

    constructor(
        title: string,
        description: string,
        priority: TaskPriority = TaskPriority.Medium,
        status: TaskStatus = TaskStatus.Pending,
        id?: string,
        createdAt?: Date,
        completedAt?: Date
    ) {
        this.id = id || this.generateId();
        this.title = title;
        this.description = description;
        this.priority = priority;
        this.status = status;
        this.createdAt = createdAt || new Date();
        this.completedAt = completedAt;
    }

    private generateId(): string {
        return Math.random().toString(36).substring(2, 9);
    }

    markAsCompleted(): void {
        this.status = TaskStatus.Completed;
        this.completedAt = new Date();
    }

    updateStatus(status: TaskStatus): void {
        this.status = status;
        if (status === TaskStatus.Completed) {
            this.completedAt = new Date();
        } else {
            this.completedAt = undefined;
        }
    }
}