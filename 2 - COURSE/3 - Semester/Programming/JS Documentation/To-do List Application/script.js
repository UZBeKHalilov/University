const taskInput = document.getElementById('taskInput');
const addTaskBtn = document.getElementById('addTaskBtn');
const taskList = document.getElementById('taskList');
// Add Task Function
function addTask() {
    const task = taskInput.value.trim();
    if (task === '') {
        alert('Please enter a task!');
        return;
    }
    // Create new list item
    const li = document.createElement('li');
    li.textContent = task;
    // Add delete button
    const deleteBtn = document.createElement('button');
    deleteBtn.textContent = 'Delete';
    deleteBtn.addEventListener('click', () => {
        taskList.removeChild(li);
    });
    // Append button to list item
    li.appendChild(deleteBtn);
    // Append list item to task list
    taskList.appendChild(li);
    // Clear input field
    taskInput.value = '';
}
// Event Listener for Add Task Button
addTaskBtn.addEventListener('click', addTask);

// Optional: Add task on pressing 'Enter'
taskInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        addTask();
    }
});