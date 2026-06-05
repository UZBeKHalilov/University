import { Task } from '../models/Task';
import { TaskService } from '../services/TaskService';
import { TaskItem } from './TaskItem';

export class TaskList {
    private element: HTMLDivElement;
    private tasks: Task[] = [];
    private taskItems: Map<string, TaskItem> = new Map();

    constructor(
        private containerId: string,
        private taskService: TaskService,
        private onEditTask: (task: Task) => void
    ) {
        this.element = document.getElementById(this.containerId) as HTMLDivElement;
        this.initialize();
    }

    private initialize(): void {
        this.tasks = this.taskService.getAllTasks();
        this.render();

        // Add listener for task changes
        this.taskService.addChangeListener(() => {
            this.tasks = this.taskService.getAllTasks();
            this.render();
        });
    }

    private render(): void {
        if (!this.element) return;
        
        this.element.innerHTML = '';
        
        if (this.tasks.length === 0) {
            const emptyMessage = document.createElement('p');
            emptyMessage.textContent = 'No tasks yet. Create a new task to get started!';
            emptyMessage.className = 'empty-message';
            this.element.appendChild(emptyMessage);
            return;
        }

        // Sort tasks by status (completed last) and then by creation date (newest first)
        const sortedTasks = [...this.tasks].sort((a, b) => {
            if (a.status === 'completed' && b.status !== 'completed') return 1;
            if (a.status !== 'completed' && b.status === 'completed') return -1;
            return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
        });

        sortedTasks.forEach(task => {
            const taskItem = new TaskItem(task, this.taskService, this.onEditTask);
            this.taskItems.set(task.id, taskItem);
            this.element.appendChild(taskItem.getElement());
        });
    }

    refresh(): void {
        this.tasks = this.taskService.getAllTasks();
        this.render();
    }
}