import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { DataTable } from 'src/app/_interfaces/comiteFiduciario.interfaces';
import { SolicitudesContractuales } from 'src/app/_interfaces/technicalCommitteSession'

@Component({
  selector: 'app-tabla-solicitudes-contractuales',
  templateUrl: './tabla-solicitudes-contractuales.component.html',
  styleUrls: ['./tabla-solicitudes-contractuales.component.scss']
})
export class TablaSolicitudesContractualesComponent implements OnInit {

  @Input() solicitudesContractuales: SolicitudesContractuales[];
  @Output() sesionesSeleccionadas = new EventEmitter<DataTable>();
  @Input() verDetalle: boolean[];

  displayedColumns: string[];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(){}

  ngOnInit(): void {
        if(!this.verDetalle){
          this.displayedColumns = ['fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud','seleccionar'];

        }else{
          this.displayedColumns = ['fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud'];
        }
        this.dataSource = new MatTableDataSource( this.solicitudesContractuales );

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

    //Metodo para emitir las sesiones seleccionadas
    getSesionSeleccionada ( event: boolean, sesion: any ) {

      const data = { estado: event, solicitud: sesion };
      event ? this.sesionesSeleccionadas.emit( data ) : this.sesionesSeleccionadas.emit( data );
    }

}
