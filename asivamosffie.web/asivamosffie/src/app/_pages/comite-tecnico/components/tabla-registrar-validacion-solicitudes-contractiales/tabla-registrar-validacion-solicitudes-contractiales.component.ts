import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-tabla-registrar-validacion-solicitudes-contractiales',
  templateUrl: './tabla-registrar-validacion-solicitudes-contractiales.component.html',
  styleUrls: ['./tabla-registrar-validacion-solicitudes-contractiales.component.scss']
})
export class TablaRegistrarValidacionSolicitudesContractialesComponent implements OnInit {

  @Input() ObjetoComiteTecnico: ComiteTecnico;

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'votacion', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    public dialog: MatDialog
  ) { }

  openDialogValidacionSolicitudes() {
    this.dialog.open(VotacionSolicitudComponent, {
      width: '70em'
    });
  }

  openDialogValidacionSolicitudesMultiple() {
    this.dialog.open(VotacionSolicitudMultipleComponent, {
      width: '70em'
    });
  }

  ngOnInit(): void {
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
  }

  cargarRegistro(){
    this.dataSource = new MatTableDataSource( this.ObjetoComiteTecnico.sesionComiteSolicitud );    
  }

}
