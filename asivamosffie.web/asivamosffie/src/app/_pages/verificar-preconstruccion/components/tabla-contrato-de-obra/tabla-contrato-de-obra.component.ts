import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { Router } from '@angular/router';
import { FaseUnoVerificarPreconstruccionService } from '../../../../core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { estadosPreconstruccion } from '../../../../_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-tabla-contrato-de-obra',
  templateUrl: './tabla-contrato-de-obra.component.html',
  styleUrls: ['./tabla-contrato-de-obra.component.scss']
})
export class TablaContratoDeObraComponent implements OnInit {

  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];
  dataSource = new MatTableDataSource();
  tipoSolicitudCodigoObra: string = '1';
  estadosPreconstruccionObra: estadosPreconstruccion;

  @ViewChild( MatPaginator, {static: true} ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort        : MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
                private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
                private routes: Router ) 
  {
    this.faseUnoVerificarPreconstruccionSvc.listaEstadosVerificacionContrato( 'obra' )
      .subscribe(
        response => {
          this.estadosPreconstruccionObra = response;
          this.faseUnoVerificarPreconstruccionSvc.getListContratacion()
          .subscribe( listas => {
            const dataTable = [];
            listas.forEach( value => {
              console.log( this.estadosPreconstruccionObra );
              if (  (value[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.conReqTecnicosAprobados.codigo
                    || value[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.enProcesoAprobacionReqTecnicos.codigo
                    || value[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.conReqTecnicosVerificados.codigo
                    || value[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.enviadoAlSupervisor.codigo)
                    && value[ 'tipoSolicitudCodigo' ] === this.tipoSolicitudCodigoObra )
              {
                dataTable.push( value );
              };
            } );
            console.log( listas );
            this.dataSource = new MatTableDataSource( dataTable );
            this.dataSource.sort = this.sort;
            this.dataSource.paginator = this.paginator;
            this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
            this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
              if (length === 0 || pageSize === 0) {
                return '0 de ' + length;
              }
              length = Math.max(length, 0);
              const startIndex = page * pageSize;
              // If the start index exceeds the list length, do not try and fix the end index to the end.
              const endIndex = startIndex < length ?
                Math.min(startIndex + pageSize, length) :
                startIndex + pageSize;
              return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
            };
          } );
        }
      );
  }

  ngOnInit(): void {
  }

  getForm ( id: number, fechaPoliza: string ) {
    this.routes.navigate( [ '/verificarPreconstruccion/obraGestionarRequisitos', id ], { state: { fechaPoliza } } )
  };

  enviarSupervisor ( contratoId: number ) {
    console.log( contratoId );
  };

};