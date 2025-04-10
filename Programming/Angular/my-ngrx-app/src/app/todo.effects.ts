import { Injectable } from '@angular/core';

import { Actions, createEffect, ofType } from '@ngrx/effects';

import { of } from 'rxjs';

import { catchError, map, mergeMap } from 'rxjs/operators';

import { TodoService } from './todo.service';

import { loadTodos, loadTodosSuccess, loadTodosFailure } from './todo.actions';

@Injectable()

export class TodoEffects {

    loadTodos$ = createEffect(() =>

        this.actions$.pipe(

            ofType(loadTodos),

            mergeMap(() =>

                this.todoService.getTodos().pipe(map((todos) => loadTodosSuccess({ todos })), catchError((error) => of(loadTodosFailure({ error })))))));

    constructor(private actions$: Actions, private todoService: TodoService) { }
}