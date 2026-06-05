import { Task, TaskPriority, TaskStatus } from '../models/Task';
import { TaskService } from '../services/TaskService';

export class TaskForm {
    private element: HTMLFormElement;
    private currentTask: Task | null = null;
    private isEditing = false;

    constructor(
        private containerId: string,
        private taskService: TaskService,
        private onSubmit: () => void
    ) {
        this.element = document.createElement('form');
        this.element.className = 'task-form';
        this.render();
        this.attachToContainer();
    }

    private render(): void {
        this.element.innerHTML = `
            <div class="form-group">
                <label for="taskTitle">Task Title:</label>
                <input type="text" id="taskTitle" name="title" required
                    value="${this.currentTask?.title || ''}">
            </div>
            <div class="form-group">
                <label for="taskDescription">Description:</label>
                <textarea id="taskDescription" name="description" rows="3">${this.currentTask?.description || ''}</textarea>
            </div>
            <div class="form-group">
                <label for="taskPriority">Priority:</label>
                <select id="taskPriority" name="priority">
                    <option value="${TaskPriority.Low}" ${this.currentTask?.priority === TaskPriority.Low ? 'selected' : ''}>Low</option>
                    <option value="${TaskPriority.Medium}" ${!this.currentTask?.priority || this.currentTask?.priority === TaskPriority.Medium ? 'selected' : ''}>Medium</option>
                    <option value="${TaskPriority.High}" ${this.currentTask?.priority === TaskPriority.High ? 'selected' : ''}>High</option>
                </select>
            </div>
            ${this.isEditing ? `
                <div class="form-group">
                    <label for="taskStatus">Status:</label>
                    <select id="taskStatus" name="status">
                        <option value="${TaskStatus.Pending}" ${!this.currentTask?.status || this.currentTask?.status === TaskStatus.Pending ? 'selected' : ''}>Pending</option>
                        <option value="${TaskStatus.InProgress}" ${this.currentTask?.status === TaskStatus.InProgress ? 'selected' : ''}>In Progress</option>
                        <option value="${TaskStatus.Completed}" ${this.currentTask?.status === TaskStatus.Completed ? 'selected' : ''}>Completed</option>
                    </select>
                </div>` : ''
            }
            <div class="form-actions">
                <button type="submit" class="btn btn-primary">
                    ${this.isEditing ? 'Update Task' : 'Add Task'}
                </button>
                ${this.isEditing ? 
                    '<button type="button" class="btn btn-danger cancel-btn">Cancel</button>' : ''}
            </div>
        `;

        // Add event listeners
        this.element.addEventListener('submit', (e) => this.handleSubmit(e));
        
        const cancelBtn = this.element.querySelector('.cancel-btn');
        if (cancelBtn) {
            cancelBtn.addEventListener('click', () => this.resetForm());
        }
    }

    private handleSubmit(event: Event): void {
        event.preventDefault();
        
        const formData = new FormData(this.element);
        const title = formData.get('title') as string;
        const description = formData.get('description') as string;
        const priority = formData.get('priority') as TaskPriority;
        const status = formData.get('status') as TaskStatus;

        if (!title) {
            console.error('Title is required');
            return;
        }

        if (this.isEditing && this.currentTask) {
            this.taskService.updateTask(this.currentTask.id, {
                title,
                description,
                priority,
                status
            });
        } else {
            this.taskService.addTask(title, description, priority);
        }

        this.resetForm();
        this.onSubmit();
    }

    public editTask(task: Task): void {
        this.currentTask = task;
        this.isEditing = true;
        this.render();
        this.attachToContainer();

        // Focus on the title input
        const titleInput = document.getElementById('taskTitle') as HTMLInputElement;
        if (titleInput) {
            titleInput.focus();
        }
    }

    private resetForm(): void {
        this.currentTask = null;
        this.isEditing = false;
        this.render();
        this.attachToContainer();
    }

    private attachToContainer(): void {
        const container = document.getElementById(this.containerId);
        if (container) {
            container.innerHTML = '';
            container.appendChild(this.element);
        }
    }
}