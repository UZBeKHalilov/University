import { Task } from '../types/Task'; // Ensure the correct path to Task type
import TaskItem from './TaskItem'; // Ensure the correct path to TaskItem

class TaskList {
    private tasks: Task[];

    constructor() {
        this.tasks = [];
    }

    public addTask(task: Task): void {
        this.tasks.push(task);
        this.render();
    }

    public removeTask(taskId: number): void {
        this.tasks = this.tasks.filter(task => task.id !== taskId);
        this.render();
    }

    public render(): HTMLElement | null {
        const taskListElement = document.getElementById('task-list');
        if (taskListElement) {
            taskListElement.innerHTML = '';
            this.tasks.forEach(task => {
                const taskItem = new TaskItem(task);
                taskListElement.appendChild(taskItem.render());
            });
        }
        return taskListElement;
    }
}

export default TaskList;