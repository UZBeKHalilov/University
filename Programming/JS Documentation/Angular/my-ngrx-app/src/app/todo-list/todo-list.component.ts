import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Todo } from '../todo.model';
import { addTodo, removeTodo, toggleTodo } from '../todo.actions';
import { selectAll } from '../todo.reducer';
import { loadTodos } from '../todo.actions';

// Define the state interface (you might already have this in todo.reducer.ts)
export interface TodoState {
  todos: Todo[];
}

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <input
      #todoInput
      type="text"
      placeholder="Add a todo"
      (keyup.enter)="addTodo(todoInput.value); todoInput.value = ''"
    />
    <ul>
      <li *ngFor="let todo of todos$ | async">
        <input
          type="checkbox"
          [checked]="todo.completed"
          (change)="toggleTodo(todo.id)"
        />
        {{ todo.description }}
        <button (click)="removeTodo(todo.id)">Remove</button>
      </li>
    </ul>
  `,
  styles: [`ul { list-style: none; padding: 0; } li { margin: 5px 0; }`]
})
export class TodoListComponent {
  todos$: Observable<Todo[]>;

  // Fix the Store typing
  constructor(private store: Store<{ todos: TodoState }>) {
    // Assuming your state shape is { todos: { todos: Todo[] } }
    this.todos$ = this.store.select(state => state.todos.todos);
    this.store.dispatch(loadTodos());
    // OR if using selectAll from entity adapter:
    // this.todos$ = this.store.select(selectAll);
  }

  addTodo(description: string) {
    const todo: Todo = {
      id: Date.now().toString(),
      description,
      completed: false
    };
    this.store.dispatch(addTodo({ todo }));
  }

  removeTodo(id: string) {
    this.store.dispatch(removeTodo({ id }));
  }

  toggleTodo(id: string) {
    this.store.dispatch(toggleTodo({ id }));
  }
}