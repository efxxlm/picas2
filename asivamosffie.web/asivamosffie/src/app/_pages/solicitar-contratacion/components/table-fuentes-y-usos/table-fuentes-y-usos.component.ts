import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table-fuentes-y-usos',
  templateUrl: './table-fuentes-y-usos.component.html',
  styleUrls: ['./table-fuentes-y-usos.component.scss']
})
export class TableFuentesYUsosComponent implements OnInit {

  @Input() contratacion: Contratacion

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
  };

  cargarRegistros(){
    this.dataSource = new MatTableDataSource( this.contratacion.contratacionProyecto );
  };

  definirFuentes ( id: number, municipio: any ) {
    this.routes.navigate( [ '/solicitarContratacion/definir-fuentes', id ], { state: {municipio: municipio} } )
  };

  getSemaforo ( elemento: any[] ) {
    let registroCompletos = 0;
    let registroEnProceso = 0;
    let registroSinDiligenciar = 0;
    elemento.forEach( value => {
      let sinDiligenciar = 0;
      let enProceso = 0;
      let completo = 0;
      if ( value.componenteAportante.length === 0 ) {
        registroSinDiligenciar++;
      } else {
        value.componenteAportante.forEach( componente => {
          if ( componente.registroCompleto === undefined ) {
            sinDiligenciar++;
          };
          if ( componente.registroCompleto === false ) {
            enProceso++;
          };
          if ( componente.registroCompleto === true ) {
            completo++;
          };
        } );

        if ( sinDiligenciar > 0 && sinDiligenciar === value.componenteAportante.length ) {
          registroSinDiligenciar++;
        }
        if ( completo > 0 && completo === value.componenteAportante.length ) {
          registroCompletos++;
        }
        if ( enProceso > 0 && enProceso < value.componenteAportante.length || enProceso === value.componenteAportante.length ) {
          registroEnProceso++;
        }
        if ( enProceso === 0 && completo > 0 && sinDiligenciar > 0 && completo + sinDiligenciar === value.componenteAportante.length ) {
          registroEnProceso++;
        }
      };
    } );
    if ( registroSinDiligenciar > 0 && registroSinDiligenciar === elemento.length ) {
      // console.log( 'condicion 1' );
      return 'sin-diligenciar';
    };
    if ( registroEnProceso > registroSinDiligenciar && registroEnProceso < elemento.length || registroEnProceso === elemento.length ) {
      // console.log( 'condicion 2' );
      return 'en-proceso';
    };
    if ( registroCompletos > 0 && registroCompletos === elemento.length ) {
      // console.log( 'condicion 3' );
      return 'completo';
    };
  };

}
