import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-form-aportantes',
  templateUrl: './form-aportantes.component.html',
  styleUrls: ['./form-aportantes.component.scss']
})
export class FormAportantesComponent implements OnInit {

  @Input() data: any[] = [];
  @Input() tipoSolicitud: string;
  institucionesEducativa: any[] = []
  totalDdp: number = 0;
  tiposAportante = {
    ffie: 6,
    et: 9,
    tercero: 10
  };
  listaTipoSolicitud = {
    obra: '1',
    interventoria: '2'
  };

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

  getAportante( departamento: string, municipio: string ) {
    if ( departamento !== undefined && municipio === undefined ) {
      return departamento;
    }
    if ( departamento === undefined && municipio !== undefined ) {
      return municipio;
    }
    if ( departamento !== undefined && municipio !== undefined ) {
      return `${ departamento }/${ municipio }`;
    }
  }

}
