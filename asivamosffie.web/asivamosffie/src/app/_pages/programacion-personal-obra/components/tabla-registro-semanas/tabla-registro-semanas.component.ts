import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-registro-semanas',
  templateUrl: './tabla-registro-semanas.component.html',
  styleUrls: ['./tabla-registro-semanas.component.scss']
})
export class TablaRegistroSemanasComponent implements OnInit {

  @Input() registroSemanas: any[];
  registroSemanasTabla: any[] = [];
  displayedColumns: string[]  = [ 'semanas' ];
  tablaRegistro               = new MatTableDataSource();

  constructor () {
  };

  ngOnInit(): void {
    if ( this.registroSemanas !== undefined ) {
      let numeroregistros = 0;
      this.registroSemanasTabla.push( [] );
      this.registroSemanas.forEach( registro => {
        if ( this.registroSemanasTabla[ numeroregistros ].length < 20 ) {
          registro[ 'cantidadPersonal' ] = null;
          this.registroSemanasTabla[ numeroregistros ].push( [ registro ] );
        };
        if ( this.registroSemanasTabla[ numeroregistros ].length >= 20 ) {
          this.registroSemanasTabla.push( [] );
          numeroregistros++;
        };
      } );
      for ( let registro of this.registroSemanasTabla ) {
        if ( registro.length < 20 ) {
          const bucleLimite = 20 - registro.length
          for( let i=0; i<bucleLimite; i++ ) {
            registro.push( [] );
          };
        };
      };
      console.log( this.registroSemanasTabla );
    }
  };

  guardarRegistros () {
    console.log( this.registroSemanasTabla );
  };

};