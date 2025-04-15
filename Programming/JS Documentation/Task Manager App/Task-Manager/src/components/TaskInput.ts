class TaskInput {
    private inputElement: HTMLInputElement;
    private buttonElement: HTMLButtonElement;
    private container: HTMLDivElement;

    constructor() {
        this.inputElement = document.createElement('input');
        this.inputElement.type = 'text';
        this.inputElement.placeholder = 'Enter a task';
        this.inputElement.className = 'flex-1 p-2 border rounded-l-lg focus:outline-none focus:ring-2 focus:ring-blue-500';
        
        this.buttonElement = document.createElement('button');
        this.buttonElement.innerText = 'Add Task';
        this.buttonElement.className = 'bg-blue-500 text-white p-2 rounded-r-lg hover:bg-blue-600';
        this.buttonElement.addEventListener('click', () => this.addTask());
        
        this.container = document.createElement('div');
        this.container.className = 'flex mb-4';
        this.container.appendChild(this.inputElement);
        this.container.appendChild(this.buttonElement);
    }

    addTask() {
        const taskTitle = this.inputElement.value.trim();
        if (taskTitle) {
            this.inputElement.value = '';
            // Logic to add the task to the task list goes here
        }
    }

    render() {
        return this.container;
    }
}

export default TaskInput;