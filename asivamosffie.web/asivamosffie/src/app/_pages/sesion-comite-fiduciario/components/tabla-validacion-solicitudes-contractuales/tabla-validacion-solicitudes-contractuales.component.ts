import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  tipo: string;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  { id: 0, fecha: '23/06/2020', numero: 'SA0006', tipo: 'Sin Apertura de proceso de selección' }
];

@Component({
  selector: 'app-tabla-validacion-solicitudes-contractuales',
  templateUrl: './tabla-validacion-solicitudes-contractuales.component.html',
  styleUrls: ['./tabla-validacion-solicitudes-contractuales.component.scss']
})
export class TablaValidacionSolicitudesContractualesComponent implements OnInit {
  @Input() ObjetoComiteTecnico: ComiteTecnico;

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'fechaComite', 'numeroComite', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService
  ) { }

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

  verSoporte(pTablaId: string, pRegistroId: number, numeroSolicitud: string) {

    //console.log(pTablaId, pRegistroId)
    this.fiduciaryCommitteeSessionService.getPlantillaByTablaIdRegistroId(pTablaId, pRegistroId)
      .subscribe(resp => { 
        console.log(resp);
        const documento = `FichaSolicitud ${numeroSolicitud}.pdf`;
        const text = documento,
          blob = new Blob([resp], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
      });

  }

  cargarRegistro() {
    console.log( this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario )
    this.dataSource = new MatTableDataSource(this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario);
  }

}
