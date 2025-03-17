import { Component } from '@angular/core';

import { CounterComponent } from './counter/counter.component';

import { TodoListComponent } from './todo-list/todo-list.component';

@Component({

  selector: 'app-root',

  standalone: true,

  imports: [CounterComponent, TodoListComponent],

 template: ` <h1>NgRx with Angular 19</h1>

<app-counter></app-counter>

<app-todo-list></app-todo-list> `,

  styles: [`h1 { text-align: center; }`]})

export class AppComponent {}



// @Component({
//   selector: 'app-root',
//   imports: [RouterOutlet],
//   templateUrl: './app.component.html',
//   styleUrl: './app.component.scss'
// })
// export class AppComponent {
//   title = 'my-ngrx-app';
// }
