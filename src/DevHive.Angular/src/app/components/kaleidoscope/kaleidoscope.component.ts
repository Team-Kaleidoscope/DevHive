import { Component, OnInit } from '@angular/core';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-kaleidoscope',
  templateUrl: './kaleidoscope.component.html',
  styleUrls: ['./kaleidoscope.component.css']
})
export class KaleidoscopeComponent implements OnInit {

  public _component: Component;

  constructor(loginComponent: LoginComponent) {
    this._component = loginComponent as Component;
   }

  ngOnInit(): void {
    
  }

  assignComponent(component: Component) {
    this._component = component;
  }
}
