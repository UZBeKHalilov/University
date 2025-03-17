import { bootstrapApplication } from '@angular/platform-browser'; 
import { provideStore } from '@ngrx/store';
import { AppComponent } from './app/app.component';
import { counterReducer } from './app/counter.reducer';
import { todoReducer } from './app/todo.reducer';
import { provideEffects } from '@ngrx/effects'; 
import { TodoEffects } from './app/todo.effects';
import { provideHttpClient } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  providers: [
    provideStore({ counter: counterReducer, todos: todoReducer }),
    provideEffects([TodoEffects]),
    provideHttpClient()
]
});