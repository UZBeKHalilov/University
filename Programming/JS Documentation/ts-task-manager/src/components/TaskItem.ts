import { Task, TaskStatus } from '../models/Task';
import { TaskService } from '../services/TaskService';

export class TaskItem {
    private element: HTMLDivElement;
    
    constructor(
        private task: Task,
        private taskService: TaskService,
        private onEdit: (task: Task) => void
    ) {
        this.element = document.createElement('div');
        this.render();
    }

    private render(): void {
        const { id, title, description, priority, status, createdAt } = this.task;
        
        this.element.className = `task-item priority-${priority}`;
        if (status === TaskStatus.Completed) {
            this.element.classList.add('completed');
        }
        
        this.element.innerHTML = `
            <div class="task-content">
                <h3>${title}</h3>
                <p>${description}</p>
                <div class="task-meta">
                    <span class="task-status status-${status}">${this.formatStatus(status)}</span>
                    <small>Created: ${this.formatDate(createdAt)}</small>
                    ${this.task.completedAt ? 
                        `<small>Completed: ${this.formatDate(this.task.completedAt)}</small>` : ''}
                </div>
            </div>
            <div class="task-actions">
                <button class="btn btn-primary edit-btn" data-id="${id}">Edit</button>
                <button class="btn btn-danger delete-btn" data-id="${id}">Delete</button>
                ${status !== TaskStatus.Completed ? 
                    `<button class="btn btn-success complete-btn" data-id="${id}">Complete</button>` : ''}
            </div>
        `;

        // Add event listeners
        const editBtn = this.element.querySelector('.edit-btn');
        const deleteBtn = this.element.querySelector('.delete-btn');
        const completeBtn = this.element.querySelector('.complete-btn');

        if (editBtn) {
            editBtn.addEventListener('click', () => this.onEdit(this.task));
        }

        if (deleteBtn) {
            deleteBtn.addEventListener('click', () => this.deleteTask());
        }

        if (completeBtn) {
            completeBtn.addEventListener('click', () => this.completeTask());
        }
    }

    private formatDate(date: Date): string {
        return new Date(date).toLocaleDateString('en-US', {
            year: 'numeric', 
            month: 'short', 
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    private formatStatus(status: TaskStatus): string {
        switch (status) {
            case TaskStatus.Pending:
                return 'Pending';
            case TaskStatus.InProgress:
                return 'In Progress';
            case TaskStatus.Completed:
                return 'Completed';
            default:
                return status;
        }
    }

    private deleteTask(): void {
        if (confirm('Are you sure you want to delete this task?')) {
            this.taskService.deleteTask(this.task.id);
            this.element.remove();
        }
    }

    private completeTask(): void {
        this.taskService.markTaskAsCompleted(this.task.id);
        this.element.remove(); // Remove old element
    }

    getElement(): HTMLDivElement {
        return this.element;
    }

    update(task: Task): void {
        this.task = task;
        this.render();
    }
}