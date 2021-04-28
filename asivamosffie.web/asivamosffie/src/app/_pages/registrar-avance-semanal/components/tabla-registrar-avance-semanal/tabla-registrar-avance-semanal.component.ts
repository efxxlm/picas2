import { MenuPerfil } from 'src/app/_interfaces/menu-perfil';
import { Router, ActivatedRoute } from '@angular/router';
import { AutenticacionService } from './../../../../core/_services/autenticacion/autenticacion.service';
import { DialogCargarActaComponent } from './../dialog-cargar-acta/dialog-cargar-acta.component';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-registrar-avance-semanal',
  templateUrl: './tabla-registrar-avance-semanal.component.html',
  styleUrls: ['./tabla-registrar-avance-semanal.component.scss']
})
export class TablaRegistrarAvanceSemanalComponent implements OnInit {
  tablaRegistro = new MatTableDataSource();
  dataTable: any = [];
  estadoAvanceSemanal: any;
  primeraSemana = 1;
  permisos: MenuPerfil;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'llaveMen',
    'numeroContrato',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'fechaUltimoReporte',
    'estadoObra',
    'gestion'
  ];

  constructor(
    private avanceSemanalSvc: RegistrarAvanceSemanalService,
    private autenticacionSvc: AutenticacionService,
    private dialog: MatDialog,
    private routes: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.avanceSemanalSvc.estadosAvanceSemanal().subscribe(estados => {
      this.estadoAvanceSemanal = estados;
      console.log( estados )
    });
    this.getDataTable();
    this.permisos = {
      tienePermisoCrear:
        this.activatedRoute.snapshot.data.tienePermisoCrear !== undefined
          ? this.activatedRoute.snapshot.data.tienePermisoCrear
          : false,
      tienePermisoLeer:
        this.activatedRoute.snapshot.data.tienePermisoLeer !== undefined
          ? this.activatedRoute.snapshot.data.tienePermisoLeer
          : false,
      tienePermisoEditar:
        this.activatedRoute.snapshot.data.tienePermisoEditar !== undefined
          ? this.activatedRoute.snapshot.data.tienePermisoEditar
          : false,
      tienePermisoEliminar:
        this.activatedRoute.snapshot.data.tienePermisoEliminar !== undefined
          ? this.activatedRoute.snapshot.data.tienePermisoEliminar
          : false
    };
  }

  ngOnInit(): void {}

  getDataTable() {
    this.avanceSemanalSvc.getVRegistrarAvanceSemanal().subscribe(listas => {
      // console.log( this.permisos, listas );
      this.dataTable = listas;
      this.dataTable.sort((a, b) => {
        if (a.fechaUltimoReporte > b.fechaUltimoReporte) return 1;
        if (a.fechaUltimoReporte < b.fechaUltimoReporte) return -1;
        return 0;
      });
      this.dataTable.forEach(element => {
        element.fechaUltimoReporte =
          element.fechaUltimoReporte !== 'Sin registro'
            ? element.fechaUltimoReporte.split('T')[0].split('-').reverse().join('/')
            : 'Sin registro';
      });
      console.log( this.dataTable )
      this.tablaRegistro = new MatTableDataSource(this.dataTable);
      this.tablaRegistro.sort = this.sort;
      this.tablaRegistro.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
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
      this.paginator._intl.previousPageLabel = 'Anterior';
      /*
        if ( this.permisos.tienePermisoCrear === false ) {
            document.getElementsByName( 'crearBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
        }
        if ( this.permisos.tienePermisoEditar === false ) {
            document.getElementsByName( 'editarBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
        }
        if ( this.permisos.tienePermisoLeer === false ) {
            document.getElementsByName( 'leerBtn' ).forEach( ( value: HTMLElement ) => value.classList.add( 'd-none' ) );
        }
      */
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tablaRegistro.filter = filterValue.trim().toLowerCase();
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openDialogCargarActa(registro: any) {
    this.dialog.open(DialogCargarActaComponent, {
      width: '70em',
      data: { registro }
    });
  }

  enviarVerificacion(contratacionProyectoId: number) {
    this.avanceSemanalSvc
      .changueStatusSeguimientoSemanal(contratacionProyectoId, this.estadoAvanceSemanal.enviadoAVerificacion.codigo)
      .subscribe(response => {
        this.openDialog('', `<b>${response.message}</b>`);
        this.dataTable = [];
        this.tablaRegistro = new MatTableDataSource();
        this.getDataTable();
      });
  }
}
