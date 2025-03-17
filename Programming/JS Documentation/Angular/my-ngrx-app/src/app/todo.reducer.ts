import { createReducer, on } from '@ngrx/store';
import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { Todo } from './todo.model';
import { addTodo, removeTodo, toggleTodo, loadTodosSuccess } from './todo.actions';

export interface TodoState extends EntityState<Todo> { }

export const adapter: EntityAdapter<Todo> = createEntityAdapter<Todo>();

export const initialState: TodoState = adapter.getInitialState();

export const selectAll = adapter.getSelectors().selectAll;

const _todoReducer = createReducer(

  initialState,

  on(addTodo, (state, { todo }) => adapter.addOne(todo, state)),

  on(removeTodo, (state, { id }) => adapter.removeOne(id, state)),

  on(toggleTodo, (state, { id }) => {

    const todo = state.entities[id];

    return adapter.updateOne(

      { id, changes: { completed: !todo?.completed } },

      state

    );
  }),

  on(loadTodosSuccess, (state, { todos }) => adapter.addMany(todos, state))

);

export function todoReducer(state: TodoState | undefined, action: any) {

  return _todoReducer(state, action);
}

// export const { selectAll } = adapter.getSelectors();