import './styles/main.css';
import { TaskService } from './services/TaskService';
import { TaskForm } from './components/TaskForm';
import { TaskList } from './components/TaskList';

// Initialize the application when DOM is fully loaded
document.addEventListener('DOMContentLoaded', () => {
    // Create services
    const taskService = new TaskService();
    
    // Initialize components
    const taskList = new TaskList('taskList', taskService, (task) => {
        taskForm.editTask(task);
    });
    
    const taskForm = new TaskForm('taskFormContainer', taskService, () => {
        taskList.refresh();
    });
});