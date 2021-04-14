import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { DialogTableProyectosSeleccionadosComponent } from '../dialog-table-proyectos-seleccionados/dialog-table-proyectos-seleccionados.component';
import { AsociadaComponent } from '../asociada/asociada.component';
import { ProyectoGrilla } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';



@Component({
  selector: 'app-tabla-resultados',
  templateUrl: './tabla-resultados.component.html',
  styleUrls: ['./tabla-resultados.component.scss']
})

export class TablaResultadosComponent implements OnInit {

  @Input() listaResultados: ProyectoGrilla[];
  @Input() esMultiproyecto: boolean;

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'numeroSolicitud',
    'sede',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  elementosSelecciondos: any[] = [];
   

  constructor ( public dialog: MatDialog ) { }

  ngOnInit(): void {
    console.log('lista', this.esMultiproyecto );
    let lista = [];
    if ( this.elementosSelecciondos.length > 0 ) {
      this.elementosSelecciondos.forEach( seleccionados => {
        this.listaResultados.forEach( ( proyecto, value ) => {
          if ( proyecto.proyectoId === seleccionados.proyectoId ) {
            this.listaResultados.splice( value, 1 );
          };
        } );
      } );
      lista = this.listaResultados;
    } else {
      lista = this.listaResultados
    }

    this.dataSource = new MatTableDataSource(lista);

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addElement(seleccionado: boolean, elemento: any) {
    if (seleccionado) {
      this.elementosSelecciondos.push(elemento);
    } else {
      this.elementosSelecciondos.forEach( ( value, index ) => {
        if ( value.proyectoId === elemento.proyectoId ) {
          this.elementosSelecciondos.splice( index, 1 );
        };
      } );
    };
  };

  verSeleccionados() {
    //console.log(this.elementosSelecciondos);
    const dialogRef = this.dialog.open(DialogTableProyectosSeleccionadosComponent, {
      data: this.elementosSelecciondos
    });

    dialogRef.afterClosed().subscribe( console.log );
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openPopup() {

    const tieneInterventoria = this.elementosSelecciondos.filter( registro => registro.tieneInterventoria === true );
    const tieneObra = this.elementosSelecciondos.filter( registro => registro.tieneObra === true );

    if (this.esMultiproyecto === true ) {
      /*
        - Se tienen que seleccionar todos los proyectos asociados a una solicitud "Para continuar con la solicitud debe seleccionar todos los proyectos relacionados a la solicitud número 'numero Solicitud' "
        - Si se selecciono un proyecto que no esta asociado a la misma solicitud se le muestra una ventana emergente con el siguiente mensaje
        "Algunos de los proyectos seleccionados ya cuentan con solicitudes en trámite asociadas a obra o interventoría, por tal razon no puede continuar con esta solicitud"
      */
      let tieneSolicitudDistinta: boolean;
      let contratacionIdAnterior: number;
      let totalSolicitudRelacionada = 0;

      this.elementosSelecciondos.forEach( registro => {
        if ( contratacionIdAnterior === undefined ) {
          contratacionIdAnterior = registro.contratacionId;
          totalSolicitudRelacionada++;
        } else {
          if ( registro.contratacionId !== contratacionIdAnterior ) {
            tieneSolicitudDistinta = true;
            return;
          } else {
            totalSolicitudRelacionada++;
          }
          contratacionIdAnterior = registro.contratacionId;
        }
      } );

      if ( tieneSolicitudDistinta === undefined ) {
        const listaSolicitud = this.listaResultados.filter( registro => registro[ 'contratacionId' ] === contratacionIdAnterior );
        console.log( totalSolicitudRelacionada, listaSolicitud.length )
        if ( totalSolicitudRelacionada !== listaSolicitud.length ) {
          this.openDialog( '', `<b>Para continuar con la solicitud debe seleccionar todos los proyectos relacionados a la solicitud número ${ this.elementosSelecciondos[ this.elementosSelecciondos.length -1 ].numeroSolicitud }.</b>` );
          return;
        }
      }

      if ( tieneSolicitudDistinta === true ) {
        this.openDialog( '', '<b>Algunos de los proyectos seleccionados ya cuentan con solicitudes en trámite asociadas a obra o interventoría, por tal razon no puede continuar con esta solicitud.</b>' );
        return;
      }
    }

    if ( tieneInterventoria.length > 0 && tieneObra.length > 0 ) {
      this.openDialog( '', '<b>Algunos de los proyectos seleccionados ya cuentan con solicitudes en trámite asociadas a obra o interventoría, por tal razon no puede continuar con esta solicitud.</b>' );
      return;
    }
    this.dialog.open(AsociadaComponent, {
      data: { 
        data: this.elementosSelecciondos,
        tieneObra: tieneObra.length > 0 ? true : false,
        tieneInterventoria: tieneInterventoria.length > 0 ? true : false,
        esMultiproyecto: this.esMultiproyecto
      }
    });

  }
}
