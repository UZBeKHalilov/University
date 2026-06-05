// This file serves as the entry point of the application. It initializes the application, sets up the main components, and renders the UI.

import TaskInput from './components/TaskInput';
import TaskList from './components/TaskList';
import { Task } from './types/Task'; // Ensure the correct path to Task type

class TaskManager {
    private taskInput: TaskInput;
    private taskList: TaskList;

    constructor() {
        this.taskInput = new TaskInput();
        this.taskList = new TaskList();
        this.render();
    }

    private addTask(taskTitle: string) {
        const task: Task = { id: Date.now(), title: taskTitle, completed: false }; // Assuming Task has 'id', 'title', and 'completed' properties
        this.taskList.addTask(task);
        this.render();
    }

    private render() {
        const appContainer = document.getElementById('app');
        if (appContainer) {
            appContainer.innerHTML = '';
            appContainer.appendChild(this.taskInput.render());
            const taskListElement = this.taskList.render();
            if (taskListElement) {
                appContainer.appendChild(taskListElement);
            }
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new TaskManager();
});