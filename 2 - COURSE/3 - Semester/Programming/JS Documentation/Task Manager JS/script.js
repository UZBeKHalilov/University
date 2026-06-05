const taskInput = document.getElementById('taskInput');
const addTaskBtn = document.getElementById('addTaskBtn');
const taskList = document.getElementById('taskList');
const allBtn = document.getElementById('allBtn');
const pendingBtn = document.getElementById('pendingBtn');
const completedBtn = document.getElementById('completedBtn');



let tasks = JSON.parse(localStorage.getItem('tasks')) || [];


function renderTasks(filter = 'all') {
    taskList.innerHTML = '';
    const filteredTasks = tasks.filter(task => {
        if (filter === 'all') return true;
        if (filter === 'pending') return !task.completed;
        if (filter === 'completed') return task.completed;
    });



    filteredTasks.forEach((task, index) => {
        const li = document.createElement('li');
        li.textContent = task.text;
        if (task.completed) li.classList.add('completed');
        


        const deleteBtn = document.createElement('button');
        deleteBtn.textContent = 'Delete';
        deleteBtn.onclick = () => deleteTask(index);



        const completeBtn = document.createElement('button');


        completeBtn.textContent = task.completed ? 'Undo' : 'Complete';
        completeBtn.onclick = () => toggleComplete(index);



        li.appendChild(deleteBtn);
        li.appendChild(completeBtn);
        taskList.appendChild(li);
    });
}

addTaskBtn.addEventListener('click', () => {
    const taskText = taskInput.value.trim();
    if (taskText) {
        tasks.push({ text: taskText, completed: false });
        taskInput.value = '';
        saveTasks();
        renderTasks();
    }
});

function toggleComplete(index) {
    tasks[index].completed = !tasks[index].completed;
    saveTasks();
    renderTasks();
}

function deleteTask(index) {
    tasks.splice(index, 1);
    saveTasks();
    renderTasks();
}

function saveTasks() {
    localStorage.setItem('tasks', JSON.stringify(tasks));
}

allBtn.addEventListener('click', () => renderTasks('all'));
pendingBtn.addEventListener('click', () => renderTasks('pending'));
completedBtn.addEventListener('click', () => renderTasks('completed'));

renderTasks();

// Select all filter buttons
const filterButtons = document.querySelectorAll('#allBtn, #pendingBtn, #completedBtn');

// Add click event listeners to each button
filterButtons.forEach(button => {
  button.addEventListener('click', () => {
    // Reset styles for all buttons
    filterButtons.forEach(btn => {
      btn.classList.remove('bg-violet-600', 'text-white');
      btn.classList.add('bg-gray-100', 'hover:bg-gray-200', 'text-gray-700');
    });

    // Apply active styles to the clicked button
    button.classList.remove('bg-gray-100', 'hover:bg-gray-200', 'text-gray-700');
    button.classList.add('bg-violet-600', 'text-white');
  });
});