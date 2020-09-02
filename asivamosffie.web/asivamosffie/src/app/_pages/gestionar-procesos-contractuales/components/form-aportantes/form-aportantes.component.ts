import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-aportantes',
  templateUrl: './form-aportantes.component.html',
  styleUrls: ['./form-aportantes.component.scss']
})
export class FormAportantesComponent implements OnInit {

  @Input() data: any[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}
