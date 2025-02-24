"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Tasks_1 = require("./models/Tasks");
const Storage_1 = require("./utils/Storage");
const taskManager = new Tasks_1.TaskManager();
const taskStorage = new Storage_1.Storage('tasks');
// Load tasks from storage on page load
window.onload = () => {
    const savedTasks = taskStorage.load();
    savedTasks.forEach((task) => taskManager.addTask(task.title));
    renderTasks();
};
// DOM elements
const addButton = document.getElementById('add-task-btn');
const taskInput = document.getElementById('task-input');
const taskList = document.getElementById('task-list');
// Add task event
addButton.addEventListener('click', () => {
    const title = taskInput.value.trim();
    if (title) {
        taskManager.addTask(title);
        taskStorage.save(taskManager.getTasks());
        taskInput.value = ''; // Clear input
        renderTasks();
    }
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
            const id = e.target.dataset.id;
            taskManager.completeTask(id);
            taskStorage.save(taskManager.getTasks());
            renderTasks();
        });
    });
    document.querySelectorAll('.delete-btn').forEach((btn) => {
        btn.addEventListener('click', (e) => {
            const id = e.target.dataset.id;
            taskManager.deleteTask(id);
            taskStorage.save(taskManager.getTasks());
            renderTasks();
        });
    });
}
