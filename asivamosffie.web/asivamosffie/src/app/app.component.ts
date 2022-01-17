import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
}) 
export class AppComponent {
  title = 'asivamosffie';
 
  ngOnInit(): void {
    console.log("Versión Front: 1  Back: 1"); 
  }
}
