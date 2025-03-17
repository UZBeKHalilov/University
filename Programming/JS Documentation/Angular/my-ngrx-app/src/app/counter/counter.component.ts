import { Component } from '@angular/core';
import { AsyncPipe } from '@angular/common';

import { Store } from '@ngrx/store';

import { Observable } from 'rxjs';

import { increment, decrement } from '../counter.actions';

@Component({
  selector: 'app-counter',
  standalone: true,
  imports: [AsyncPipe],
  template: `<button (click)="decrement()">-</button>

<span>{{ count$ | async }}</span>

<button (click)="increment()">+</button>`,
  styles: [`span { margin: 0 10px; }`]
})

export class CounterComponent {
  count$: Observable<number>;
  constructor(private store: Store<{ counter: number }>) {
    this.count$ = this.store.select('counter');
  }
  increment() {
    this.store.dispatch(increment());
  }
  decrement() {
    this.store.dispatch(decrement());
  }
}