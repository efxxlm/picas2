import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-form-aportantes',
  templateUrl: './form-aportantes.component.html',
  styleUrls: ['./form-aportantes.component.scss']
})
export class FormAportantesComponent implements OnInit {

  @Input() data: any[] = [];
  institucionesEducativa: any[] = []
  totalDdp: number = 0;
  tiposAportante = {
    ffie: 6,
    et: 9,
    tercero: 10
  }

  constructor() { }

  ngOnInit(): void {
    this.getData();
  }

  getData () {
    this.data.forEach( contratacion => {
      this.institucionesEducativa.push( contratacion.proyecto )
    } );
    console.log( this.institucionesEducativa );
  }

}
