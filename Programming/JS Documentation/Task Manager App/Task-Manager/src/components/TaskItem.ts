class TaskItem {
    private task: { title: string; completed: boolean };

    constructor(task: { title: string; completed: boolean }) {
        this.task = task;
    }

    render() {
        const listItem = document.createElement('li');
        listItem.className = 'flex justify-between items-center p-2 border-b';

        const taskTitle = document.createElement('span');
        taskTitle.textContent = this.task.title;
        taskTitle.className = this.task.completed ? 'line-through' : '';

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.className = 'bg-red-500 text-white p-1 rounded hover:bg-red-600';
        deleteButton.onclick = () => this.deleteTask();

        listItem.appendChild(taskTitle);
        listItem.appendChild(deleteButton);

        return listItem;
    }

    deleteTask() {
        // Logic to delete the task
        console.log(`Task "${this.task.title}" deleted.`);
    }
}

export default TaskItem;