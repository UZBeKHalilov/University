import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable, of } from 'rxjs';

import { Todo } from './todo.model';

@Injectable({

  providedIn: 'root'

})

export class TodoService {

  // Simulate an API call

  getTodos(): Observable<Todo[]> {

return of([

 { id: '1', description: 'Learn Angular 19', completed: false },

 { id: '2', description: 'Explore NgRx', completed: true }

]);}}