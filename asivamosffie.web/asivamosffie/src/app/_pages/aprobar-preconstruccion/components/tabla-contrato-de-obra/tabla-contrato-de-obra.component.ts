import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { estadosPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoAprobarPreconstruccionService } from '../../../../core/_services/faseUnoAprobarPreconstruccion/fase-uno-aprobar-preconstruccion.service';

@Component({
  selector: 'app-tabla-contrato-de-obra',
  templateUrl: './tabla-contrato-de-obra.component.html',
  styleUrls: ['./tabla-contrato-de-obra.component.scss']
})
export class TablaContratoDeObraComponent implements OnInit {

  estadosPreconstruccionObra: estadosPreconstruccion;
  tipoSolicitudCodigoObra: string = '1';
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];

  @ViewChild( MatPaginator, {static: true} ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort        : MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
                private faseUnoAprobarPreconstruccionSvc: FaseUnoAprobarPreconstruccionService ) 
  {
    this.faseUnoAprobarPreconstruccionSvc.listaEstadosAprobarContrato( 'obra' )
      .subscribe(
        response => {
          this.estadosPreconstruccionObra = response;
          this.faseUnoAprobarPreconstruccionSvc.getListContratacion()
          .subscribe( listas => {
            console.log( this.estadosPreconstruccionObra, listas );
            const dataTable = [];
            listas.forEach( lista => {
              if (  ( lista[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.enviadoAlSupervisor.codigo
                    || lista[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.enProcesoValidacionReqTecnicos.codigo
                    || lista[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.conReqTecnicosValidados.codigo
                    || lista[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.conReqTecnicosAprobadosPorSupervisor.codigo
                    || lista[ 'estadoCodigo' ] === this.estadosPreconstruccionObra.enviadoAlInterventor.codigo )
                    && lista[ 'tipoSolicitudCodigo' ] === this.tipoSolicitudCodigoObra )
              {
                dataTable.push( lista );
              };
            } )
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
  };

  aprobarInicio () {
    console.log( 'Aprobar inicio' );
  };

  enviarInterventor () {
    console.log( 'Enviar al interventor' );
  };

};