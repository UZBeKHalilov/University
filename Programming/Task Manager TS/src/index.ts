import { TaskManager, Task } from './models/Tasks';
import { Storage } from './utils/Storage';

const taskManager = new TaskManager();
const taskStorage = new Storage<Task>('tasks');


// Load tasks from storage on page load
window.onload = () => {
    const savedTasks = taskStorage.load();
    savedTasks.forEach((task) => taskManager.addTask(task.title));
    renderTasks();
};

// DOM elements
const addButton = document.getElementById('add-task-btn') as HTMLButtonElement;
const taskInput = document.getElementById('task-input') as HTMLInputElement;
const taskList = document.getElementById('task-list') as HTMLUListElement;

// Add task event
addButton.addEventListener('click', () => {
    const title = taskInput.value.trim();
    if (!title) {
        alert('Task title cannot be empty!');
        return;
    }
    taskManager.addTask(title);
    taskStorage.save(taskManager.getTasks());
    taskInput.value = '';
    renderTasks();
});

// Render tasks to the DOM
function renderTasks() {
    taskList.innerHTML = ''; // Clear current list
    taskManager.getTasks().forEach((task) => {
        const li = document.createElement('li');
        li.className = 'flex justify-between items-center p-2 border-b';
        li.innerHTML = `
            <span class="${task.completed ? 'line-through text-gray-500' : ''}">${task.title}</span>
            <div>
                <button class="complete-btn text-green-500 mr-2" data-id="${task.id}">Complete</button>
                <button class="delete-btn text-red-500" data-id="${task.id}">Delete</button>
            </div>
        `;
        taskList.appendChild(li);
    });

    // Add event listeners to buttons
    document.querySelectorAll('.complete-btn').forEach((btn) => {
        btn.addEventListener('click', (e) => {
            const id = (e.target as HTMLButtonElement).dataset.id!;
            taskManager.completeTask(id);
            taskStorage.save(taskManager.getTasks());
            renderTasks();
        });
    });

    document.querySelectorAll('.delete-btn').forEach((btn) => {
        btn.addEventListener('click', (e) => {
            const id = (e.target as HTMLButtonElement).dataset.id!;
            taskManager.deleteTask(id);
            taskStorage.save(taskManager.getTasks());
            renderTasks();
        });
    });
}