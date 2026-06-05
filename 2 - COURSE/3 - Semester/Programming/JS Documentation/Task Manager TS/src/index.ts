import { TaskManager, Task } from "./models/Task"; 
import { DataStorage } from "./utils/Storage"; 
// Initialize TaskManager and Storage 
const taskManager = new TaskManager(); 
const taskStorage = new DataStorage<Task>("tasks"); 
// Load tasks from local storage 
const savedTasks = taskStorage.load(); 
savedTasks.forEach((task: Task) => taskManager.addTask(task.title)); 
// Example usage 
taskManager.addTask("Learn TypeScript"); 
taskManager.addTask("Build a Task Manager App"); 
taskManager.completeTask(savedTasks[0]?.id || ""); // Complete the first task 
taskManager.deleteTask(savedTasks[1]?.id || ""); // Delete the second task 
// Save tasks to local storage 
taskStorage.save(taskManager.getTasks()); 
// Log all tasks 
console.log("All tasks:", taskManager.getTasks()); 