import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity()
export class Task {
    @PrimaryGeneratedColumn()
    id?: number;

    @Column()
    name?: string;

    @Column({ default: false })
    completed?: boolean;
}

// export class TaskManager {
//     private tasks: Task[] = [];

//     // Add a task 
//     addTask(title: string): void {
//         const newTask: Task = {
//             id: Math.random().toString(36).substr(2, 9), // Generate a random ID 
//             title,
//             completed: false,
//         };
//         this.tasks.push(newTask);
//         console.log(`Task added: ${title}`);
//     }

//     // Mark a task as completed 
//     completeTask(id: string): void {
//         const task = this.tasks.find((t) => t.id === id);
//         if (task) {
//             task.completed = true;
//             console.log(`Task completed: ${task.title}`);
//         } else {
//             console.log("Task not found");
//         }
//     }



//     // Delete a task 
//     deleteTask(id: string): void {
//         const index = this.tasks.findIndex((t) => t.id === id);
//         if (index !== -1) {
//             const deletedTask = this.tasks.splice(index, 1)[0];
//             console.log(`Task deleted: ${deletedTask.title}`);
//         } else {
//             console.log("Task not found");
//         }
//     }



//     // Get all tasks 
//     getTasks(): Task[] {
//         return this.tasks;
//     }
// } 